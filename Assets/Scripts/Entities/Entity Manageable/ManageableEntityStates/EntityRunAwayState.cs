using IA_I.EntityNS.Manegeable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I
{
    public class EntityRunAwayState : IState
    {
        FSM<ManageableEntityStates> _fsm;
        ManageableEntities _entity;

        public EntityRunAwayState(FSM<ManageableEntityStates> fsm, ManageableEntities entity)
        {
            _fsm = fsm;
            _entity = entity;
        }
        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
