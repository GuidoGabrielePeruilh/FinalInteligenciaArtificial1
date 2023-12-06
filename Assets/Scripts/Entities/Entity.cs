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
        public bool HasToMoveInPath { get; protected set; }
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
            CurrentLife = MyEntityData.maxLife;
            HasToRunAway = false;
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

        public void FOV()
        {
            var targets = Physics.OverlapSphere(transform.position, MyEntityData.attackRadius, MyEntityData.targetLayer);

            _enemiesInView = targets
                .Select(target => target.GetComponent<Entity>())
                .Where(enemy =>
                    enemy != null &&
                    Vector3.Distance(enemy.transform.position, transform.position) <= MyEntityData.attackRadius &&
                    Vector3.Angle(transform.forward, (enemy.transform.position - transform.position).normalized) < MyEntityData.viewAngle / 2 &&
                    !Physics.Linecast(transform.position, enemy.transform.position, MyEntityData.obstacleLayerMask) &&
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

        public void UpdateTargetPosition(Vector3 targetPosition)
        {
            if (TargetPosition != targetPosition)
            {
                HasToMoveInPath = true;
                TargetPosition = targetPosition;
            }
        }

        protected abstract void BasicMove(Vector3 dir);

        public void FollowPath(Stack<Node> pathToFollow)
        {
            if (pathToFollow.Count == 0)
            {
                HasToMoveInPath = false;
                HasToRunAway = false;
                return;
            }

            Vector3 nextPos = pathToFollow.Peek().transform.position;
            Vector3 dir = nextPos - transform.position;
            dir.y = 0;

            BasicMove(dir);

            if (dir.sqrMagnitude < MyEntityData.distanceToLowSpeed * MyEntityData.distanceToLowSpeed)
            {
                pathToFollow.Pop();
            }
        }

        public virtual void OnDamageRecived(float dmg)
        {
            CurrentLife -= dmg;
            if (CurrentLife <= MyEntityData.maxLife * MyEntityData.percentageOfLifeToRunAway)
            {
                HasToMoveInPath = true;
                HasToRunAway = true;
            }
            if (CurrentLife > 0) return;
            Destroy(gameObject);
        }

        protected virtual void OnDrawGizmos()
        {
            // Calcular los puntos extremos del cono de visión
            Vector3 leftDir = Quaternion.Euler(0, -MyEntityData.viewAngle / 2, 0) * transform.forward;
            Vector3 rightDir = Quaternion.Euler(0, -MyEntityData.viewAngle / 2, 0) * transform.forward;

            // Dibujar los lados del cono de visión
            Gizmos.color = Color.grey ;
            Gizmos.DrawRay(transform.position, leftDir * MyEntityData.attackRadius);
            Gizmos.DrawRay(transform.position, rightDir * MyEntityData.attackRadius);

            // Dibujar el arco para visualizar el ángulo de visión
            float halfFOV = MyEntityData.viewAngle / 2f;
            float viewRadius = MyEntityData.attackRadius * Mathf.Tan(halfFOV * Mathf.Deg2Rad);
            Vector3 viewAngleA = Quaternion.Euler(0, -halfFOV, 0) * transform.forward *
                                 MyEntityData.attackRadius;
            Vector3 viewAngleB = Quaternion.Euler(0, halfFOV, 0) * transform.forward *
                                 MyEntityData.attackRadius;

            Gizmos.color = Color.grey;
            Gizmos.DrawRay(transform.position, viewAngleA);
            Gizmos.DrawRay(transform.position, viewAngleB);

            // Dibujar el arco que conecta los lados del cono de visión
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position + leftDir * MyEntityData.attackRadius, viewAngleA - leftDir * MyEntityData.attackRadius);
            Gizmos.DrawRay(transform.position + rightDir * MyEntityData.attackRadius, viewAngleB - rightDir * MyEntityData.attackRadius);
            Gizmos.DrawRay(transform.position, transform.forward * MyEntityData.attackRadius);
        }
    }

}
