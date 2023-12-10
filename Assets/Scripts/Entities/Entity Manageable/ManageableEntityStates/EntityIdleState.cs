using IA_I.EntityNS.Manegeable;
using UnityEngine;

namespace IA_I.FSM.StatesBehaviour
{
    public class EntityIdleState : IState
    {
        FSM<ManageableEntityStates> _fsm;
        ManageableEntities _entity;

        public EntityIdleState(FSM<ManageableEntityStates> fsm, ManageableEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }
        public void OnEnter()
        {

        }

        public void OnExit()
        {

        }

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {
            if (_entity.HaveTargetToAttack())
                _fsm.ChangeState(ManageableEntityStates.Attack);

            if (_entity.HasToMove)
                _fsm.ChangeState(ManageableEntityStates.Move);

        }

    }
}

