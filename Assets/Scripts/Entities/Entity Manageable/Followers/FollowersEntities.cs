using IA_I.EntityNS.Manegeable;
using IA_I.StatesBehaviour;
using IA_I.Weapons.Guns;
using System.Linq;
using UnityEngine;

namespace IA_I.EntityNS.Follower
{
    public class FollowersEntities : Entity
    {
        FSM<FollowersEntitiesStates> _fsm;
        public ManageableEntities LeaderToFollow => _leaderToFollow;
        [SerializeField] private ManageableEntities _leaderToFollow;

        public bool HasToSeekLeader { get; protected set; }

        public Vector3 SeparationForce => _separationForce;
        public Vector3 AlignmentForce => _alignmentForce;
        public Vector3 CohesionForce => _cohesionForce;
        public Vector3 CombinedForce => _combinedForce;

        private Vector3 _separationForce;
        private Vector3 _alignmentForce;
        private Vector3 _cohesionForce;
        private Vector3 _combinedForce;

        new private void Awake()
        {
            base.Awake();
            _myGun = GetComponentInChildren<Gun>();
            _fsm = new FSM<FollowersEntitiesStates>();
            IState move = new FollowerMoveState(_fsm, this, _leaderToFollow);
            IState idle = new FollowerIdleState(_fsm, this);
            IState attack = new FollowerAttackState(_fsm, this, _myGun);
            IState seek = new FollowerSeekState(_fsm, this);
            IState runAway = new FollowerRunAwayState(_fsm, this);

            _fsm.AddState(FollowersEntitiesStates.Move, move);
            _fsm.AddState(FollowersEntitiesStates.Idle, idle);
            _fsm.AddState(FollowersEntitiesStates.Attack, attack);
            _fsm.AddState(FollowersEntitiesStates.Seek, seek);
            _fsm.AddState(FollowersEntitiesStates.RunAway, runAway);
        }

        private void Start()
        {
            FollowersManager.Instance.RegisterNewFollower(this, _leaderToFollow);
            TargetPosition = transform.position;
            HasToMove = false;
            _fsm.ChangeState(FollowersEntitiesStates.Idle);
        }

        private void Update()
        {
            if (_leaderToFollow != null)
            {
                UpdateTargetPosition(_leaderToFollow.TargetPosition);
            }
            _separationForce = Separation() * FollowersManager.Instance.SeparationWeight;
            _alignmentForce = Alignment() * FollowersManager.Instance.AlignmentWeight;
            _cohesionForce = Cohesion() * FollowersManager.Instance.CohesionWeight;
            _combinedForce = _separationForce + _alignmentForce + _cohesionForce;
            _fsm.Update();
        }

        private void LateUpdate()
        {
            _fsm.LateUpdate();
        }

        public Node GetRandomNodeToRun()
        {
            var myPosiblesNodes = NodesManager.Instance.GetAllNodes()
                .Where(node => node.IsBlocked)
                .SelectMany(node => node.GetNeighbors())
                .Where(node => !NodesManager.Instance.CheckExistingNode(NodesManager.NodesLists.Used, node))
                .OrderByDescending(node => Vector3.Distance(node.transform.position, transform.position))
                .Take(50)
                .ToList();
            if (myPosiblesNodes.Count == 0)
                return null;

            var randomNode = myPosiblesNodes[Random.Range(0, myPosiblesNodes.Count)];
            NodesManager.Instance.RegistrerNewUsedNode(randomNode);
            return randomNode;
        }

        public bool IsCloseFromLeader()
        {
            if (_leaderToFollow == null) return true;
            var distanceFromTarget = _leaderToFollow.transform.position - transform.position;
            return distanceFromTarget.sqrMagnitude <= FollowersManager.Instance.ViewRadius;
        }

        public override void OnDamageRecived(float dmg)
        {
            base.OnDamageRecived(dmg);
            FollowersManager.Instance.RemoveFollower(this, LeaderToFollow);
        }

        public override void UpdateTargetPosition(Vector3 targetPosition)
        {
            if (TargetPosition != targetPosition)
            {
                HasToMove = true;
                TargetPosition = targetPosition;
            }
            else
                HasToMove = false;
        }



        #region Movement
        public void FlockingMove(Vector3 dir)
        {
            BasicMove(dir);
        }

        protected override void BasicMove(Vector3 dir)
        {
            AddForce(CalculateSteering(dir, MyEntityData.Speed) + _combinedForce + AvoidObstacles(Mathf.Sqrt(FollowersManager.Instance.ViewRadius)) * 2, MyEntityData.Speed);
        }

        public void Stop()
        {
            _velocity = Vector3.zero;
        }
        #endregion

        #region Steering Behaviors

        public Vector3 Arrive(GameObject leader)
        {
            var desired = Vector3.zero;
            var speed = MyEntityData.Speed;
            var distanceFromTarget = leader.transform.position - transform.position;


            desired += distanceFromTarget;
            float arriveRadius = FollowersManager.Instance.ArriveRadius;
            if (distanceFromTarget.sqrMagnitude <= arriveRadius)
            {
                speed = MyEntityData.Speed * ((distanceFromTarget.sqrMagnitude + 1) / arriveRadius);
            }
            desired *= speed;

            if (desired.sqrMagnitude <= MyEntityData.DistanceToLowSpeed * MyEntityData.DistanceToLowSpeed)
            {
                _velocity = Vector3.zero;
                return Vector3.zero;
            }

            return CalculateSteering(desired, speed);
        }

        #endregion

        #region flocking

        public Vector3 Separation()
        {
            Vector3 desired = Vector3.zero;

            if (_leaderToFollow != null)
            {
                Vector3 dirToLeader = _leaderToFollow.transform.position - transform.position;
                if (dirToLeader.sqrMagnitude <= FollowersManager.Instance.SeparationRadius)
                {
                    desired -= dirToLeader;
                }
            }

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;

                Vector3 dirToEntity = follower.transform.position - transform.position;

                if (dirToEntity.sqrMagnitude <= FollowersManager.Instance.SeparationRadius)
                {
                    desired -= dirToEntity;
                }
            }

            if (desired == Vector3.zero) return desired;

            return CalculateSteering(desired, MyEntityData.Speed);
        }

        private Vector3 Alignment()
        {
            Vector3 desired = Vector3.zero;

            int count = 0;

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;

                Vector3 dirToBoid = follower.transform.position - transform.position;

                if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.AlignmentRadius)
                {
                    desired += follower.Velocity;
                    count++;
                }
            }

            if (count == 0) return desired;

            desired /= count;

            return CalculateSteering(desired, MyEntityData.Speed);
        }

        private Vector3 Cohesion()
        {
            Vector3 desired = Vector3.zero;
            int count = 0;

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;
                Vector3 dirToBoid = follower.transform.position - transform.position;

                if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.CohesionRadius)
                {
                    desired += follower.transform.position;

                    count++;
                }
            }

            if (count == 0) return desired;

            desired /= count;

            desired -= transform.position;

            return CalculateSteering(desired, MyEntityData.Speed);
        }

        #endregion
     
        #region gizmos
        new private void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (Application.isPlaying)
            {
                //Gizmos.color = Color.red;
                //Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.ArriveRadius));


                //Gizmos.color = Color.yellow;
                //Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.ViewRadius));


                //Gizmos.color = Color.blue;
                //Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.SeparationRadius));

            }
        }
        #endregion
    }
}

