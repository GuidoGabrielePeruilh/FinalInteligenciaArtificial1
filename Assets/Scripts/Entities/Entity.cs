using IA_I.SO;
using IA_I.Weapons.Guns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IA_I.EntityNS
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityDataSO MyEntityData;
        protected List<Transform> _enemiesInView = new List<Transform>();
        [SerializeField] protected Gun _myGun;


        public Teams Team => _team;
        [SerializeField] protected Teams _team;
        public Vector3 Velocity => _velocity;
        protected Vector3 _velocity;    

        public Vector3 TargetPosition { get; protected set; }
        public bool HasToMove { get; protected set; }
        public bool HasArriveToDestiny { get; protected set; }
        public bool HasToRunAway { get; protected set; }
        public float CurrentLife { get; protected set; }
        public Transform AttackTarget { get; protected set; }

        protected void Awake()
        {
            gameObject.tag = _team.ToString();
            foreach (Transform child in transform)
            {
                child.gameObject.tag = _team.ToString();
            }
            CurrentLife = MyEntityData.MaxLife;
            HasToRunAway = false;
        }

        protected Vector3 AvoidObstacles(float radius)
        {
            Vector3 desired = Vector3.zero;

            Collider[] allObstacles = Physics.OverlapSphere(transform.position, radius, MyEntityData.ObstacleLayerMask);

            foreach (var obstacle in allObstacles)
            {
                var angle = Vector3.SignedAngle(transform.forward, obstacle.transform.position - transform.position, Vector3.up);

                if (Mathf.Abs(angle) <= MyEntityData.ViewAngle)
                {
                    var sign = Mathf.Sign(angle);

                    return CalculateSteering(transform.right * -sign, MyEntityData.Speed);
                }
            }

            return CalculateSteering(desired, MyEntityData.Speed);
        }

        protected virtual void AddForce(Vector3 force, float speed)
        {
            _velocity += force;
            _velocity = Vector3.ClampMagnitude(_velocity, speed);
            transform.position += _velocity * Time.deltaTime;
            if (_velocity.magnitude > 0.001f)
            {
                transform.forward = _velocity.normalized;
            }
        }

        protected Vector3 CalculateSteering(Vector3 desired, float speed)
        {
            return desired == Vector3.zero ? desired :
                Vector3.ClampMagnitude((desired.normalized * speed) - _velocity, MyEntityData.MaxForce);
        }

        public void FOV()
        {
            var targets = Physics.OverlapSphere(transform.position, MyEntityData.AttackRadius, MyEntityData.TargetLayerMask);

            _enemiesInView = targets
                .Select(target => target.GetComponent<Entity>())
                .Where(enemy =>
                    enemy != null &&
                    Vector3.Distance(enemy.transform.position, transform.position) <= MyEntityData.AttackRadius &&
                    Vector3.Angle(transform.forward, (enemy.transform.position - transform.position).normalized) < MyEntityData.ViewAngle / 2 &&
                    !Physics.Linecast(transform.position, enemy.transform.position, MyEntityData.ObstacleLayerMask) &&
                    enemy.Team != _team)
                .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
                .Select(enemy => enemy.transform)
                .ToList();
        }

        public bool HaveTargetToAttack()
        {
            AttackTarget = _enemiesInView.FirstOrDefault();
            if (_enemiesInView.Count > 0)
                return true;
            return false;
        }

        public abstract void UpdateTargetPosition(Vector3 targetPosition);

        protected abstract void BasicMove(Vector3 dir);

        public void FollowPath(Stack<Node> pathToFollow)
        {
            HasArriveToDestiny = false;
            if (pathToFollow.Count == 0 || pathToFollow == null)
            {
                HasArriveToDestiny = true;
                HasToMove = false;
                HasToRunAway = false;
                return;
            }

            Vector3 nextPos = pathToFollow.Peek().transform.position;
            Vector3 dir = nextPos - transform.position;
            dir.y = 0;

            BasicMove(dir);

            if (dir.sqrMagnitude < MyEntityData.DistanceToLowSpeed * MyEntityData.DistanceToLowSpeed)
            {
                pathToFollow.Pop();
            }
        }

        public virtual void OnDamageRecived(float dmg)
        {
            CurrentLife -= dmg;
            if (CurrentLife <= MyEntityData.MaxLife * MyEntityData.PercentageOfLifeToRunAway)
            {
                HasToRunAway = true;
            }
            if (CurrentLife > 0) return;
            Destroy(gameObject);
        }

        protected virtual void OnDrawGizmos()
        {
            Vector3 leftDir = Quaternion.Euler(0, -MyEntityData.ViewAngle / 2, 0) * transform.forward;
            Vector3 rightDir = Quaternion.Euler(0, -MyEntityData.ViewAngle / 2, 0) * transform.forward;

            Gizmos.color = Color.grey ;
            Gizmos.DrawRay(transform.position, leftDir * MyEntityData.AttackRadius);
            Gizmos.DrawRay(transform.position, rightDir * MyEntityData.AttackRadius);

            float halfFOV = MyEntityData.ViewAngle / 2f;
            float viewRadius = MyEntityData.AttackRadius * Mathf.Tan(halfFOV * Mathf.Deg2Rad);
            Vector3 viewAngleA = Quaternion.Euler(0, -halfFOV, 0) * transform.forward *
                                 MyEntityData.AttackRadius;
            Vector3 viewAngleB = Quaternion.Euler(0, halfFOV, 0) * transform.forward *
                                 MyEntityData.AttackRadius;

            Gizmos.color = Color.grey;
            Gizmos.DrawRay(transform.position, viewAngleA);
            Gizmos.DrawRay(transform.position, viewAngleB);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, transform.forward * MyEntityData.AttackRadius);
        }
    }

}
