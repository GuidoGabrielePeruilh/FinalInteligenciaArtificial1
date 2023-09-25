using IA_I.FSM.StatesBehaviour;
using UnityEngine;

namespace IA_I.EntityNS.Manegeable
{
    public class ManageableEntities : Entity
    {
        FSM<ManageableEntityStates> _fsm;
        [SerializeField] Animator myAnimator;

        private void Awake()
        {
            UpdateTargetPosition(transform.position);
            CurrentLife = MyEntityData.maxLife;
            _fsm = new FSM<ManageableEntityStates>();

            IState findPath = new EntityFindPathState(_fsm, this);
            IState attack = new EntityAttackState(_fsm, this, myAnimator, "Attack", MyEntityData.attackCooldown);
            IState idle = new EntityIdleState(_fsm, this);
            _fsm.AddState(ManageableEntityStates.FindPath, findPath);
            _fsm.AddState(ManageableEntityStates.Attack, attack);
            _fsm.AddState(ManageableEntityStates.Idle, idle);

        }

        private void Start()
        {
            _fsm.ChangeState(ManageableEntityStates.FindPath);
        }

        private void Update()
        {
            _fsm.Update();

        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        public Vector3 UpdateTargetPosition(Vector3 targetPosition)
        {
            HasToMove = true;
            return TargetPosition = targetPosition;
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, MyEntityData.attackRadius);
        }
    }
}

