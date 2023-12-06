using IA_I.EntityNS.Manegeable;
using IA_I.StatesBehaviour;
using IA_I.Weapons.Guns;
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

        private void Awake()
        {
            base.Awake();
            _myGun = GetComponentInChildren<Gun>();
            UpdateTargetPosition(transform.position);
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
            _fsm.ChangeState(FollowersEntitiesStates.Move);
        }

        private void Update()
        {
            if (_leaderToFollow == null) return;
            UpdateTargetPosition(_leaderToFollow.TargetPosition);
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

        public bool IsCloseFromLeader()
        {
            var distanceFromTarget = _leaderToFollow.transform.position - transform.position;
            return distanceFromTarget.sqrMagnitude <= FollowersManager.Instance.ViewRadius;
        }

        public override void OnDamageRecived(float dmg)
        {
            base.OnDamageRecived(dmg);
            FollowersManager.Instance.RemoveFollower(this, LeaderToFollow);
        }

        #region Movement
        public void FlockingMove(Vector3 dir)
        {
            BasicMove(dir);
        }

        protected override void BasicMove(Vector3 dir)
        {
            AddForce(CalculateSteering(dir, MyEntityData.speed) + _combinedForce, MyEntityData.speed);
        }


        #endregion

        #region Steering Behaviors

        public Vector3 Arrive(GameObject leader)
        {
            var desired = Vector3.zero;
            var speed = MyEntityData.speed;
            var distanceFromTarget = leader.transform.position - transform.position;


            desired += distanceFromTarget;
            float arriveRadius = FollowersManager.Instance.ArriveRadius;
            if (distanceFromTarget.sqrMagnitude <= arriveRadius)
            {
                speed = MyEntityData.speed * ((distanceFromTarget.sqrMagnitude + 1) / arriveRadius);
            }
            desired *= speed;

            if (desired.sqrMagnitude <= MyEntityData.distanceToLowSpeed * MyEntityData.distanceToLowSpeed)
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

            return CalculateSteering(desired, MyEntityData.speed);
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

            return CalculateSteering(desired, MyEntityData.speed);
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

            return CalculateSteering(desired, MyEntityData.speed);
        }

        #endregion
     
        #region gizmos
        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                base.OnDrawGizmos();
                Gizmos.color = Color.red;
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

