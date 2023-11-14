using IA_I.SO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IA_I.EntityNS
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityDataSO MyEntityData;
        [SerializeField] private LayerMask _targetLayer;

        public Teams Team => _team;
        [SerializeField] protected Teams _team;
        public Vector3 Velocity => _velocity;
        protected Vector3 _velocity;
        protected Collider[] _targets;
        public GameObject AttackTarget => _attackTarget;
        protected GameObject _attackTarget;

        public Vector3 TargetPosition { get; protected set; }
        public bool HasToMove { get; protected set; }
        public bool HasLowLife { get; protected set; }
        public float CurrentLife { get; protected set; }
        public void Initialize(EntityDataSO entityData)
        {
            MyEntityData = entityData;
        }

        protected virtual void AddForce(Vector3 force, float speed)
        {
            _velocity += force;
            _velocity = Vector3.ClampMagnitude(_velocity, speed);
            transform.position += _velocity * Time.deltaTime;
            transform.forward = _velocity;
        }

        protected Vector3 CalculateSteering(Vector3 desired, float speed)
        {
            desired.Normalize();
            desired *= speed;
            return Vector3.ClampMagnitude(desired - _velocity, MyEntityData.maxForce);
        }

        public Node GetRandomNodeToRun()
        {
            var myPosiblesNodes = NodesManager.Instance.GetAllNodes()
                .Where(node => node.IsBlocked)
                .SelectMany(node => node.GetNeighbors())
                .OrderByDescending(node => Vector3.Distance(node.transform.position, transform.position))
                .Take(50)
                .ToList();


            var randomNode = Random.Range(0, myPosiblesNodes.Count);
            return myPosiblesNodes[randomNode];
        }

        public bool HaveTargetToAttack()
        {
            _targets = Physics.OverlapSphere(transform.position, MyEntityData.attackRadius, _targetLayer);

            if (_targets.Length == 0) return false;
        
            var filteredTargets = _targets.Where(target => 
            {
                var teamIdentifier = target.GetComponent<Entity>();
                return teamIdentifier == null || teamIdentifier.Team != _team; 
            })
                .ToArray();

            var myTarget = filteredTargets.GetClosesObject(transform.position);

            if (myTarget != null)
            {
                _attackTarget = myTarget.gameObject;
                return true;
            }
            _attackTarget = null;
            return false;
        }

        public void FollowPath(Stack<Node> pathToFollow)
        {
            if (pathToFollow.Count == 0)
            {
                HasToMove = false;
                HasLowLife = false;
                return;
            }

            Vector3 nextPos = pathToFollow.Peek().transform.position;
            Vector3 dir = nextPos - transform.position;
            dir.y = 0;

            AddForce(CalculateSteering(dir, MyEntityData.speed), MyEntityData.speed);


            if (dir.sqrMagnitude < MyEntityData.distanceToLowSpeed * MyEntityData.distanceToLowSpeed)
            {
                pathToFollow.Pop();
            }
        }

        public void OnDamageRecived(float dmg)
        {
            CurrentLife -= dmg;
            if (CurrentLife <= MyEntityData.maxLife * MyEntityData.percentageOfLifeToRunAway)
            {
                HasLowLife = true;
                HasToMove = true;
            }
            if (CurrentLife > 0) return;
            Destroy(gameObject);
        }
    }

}
