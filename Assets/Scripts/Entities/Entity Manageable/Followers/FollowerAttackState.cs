using IA_I.EntityNS.Follower;
using IA_I.Weapons.Guns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.StatesBehaviour
{
    public class FollowerAttackState : IState
    {

        FSM<FollowersEntitiesStates> _fsm;
        FollowersEntities _entity;
        Gun _myGun;
        float _timer;
        GameObject _target;

        public FollowerAttackState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity, Gun gun)
        {
            _fsm = fsm;
            _entity = entity;
            _myGun = gun;
        }

        public void OnEnter()
        {
            Debug.Log("On Enter Follower Attack State");
            _timer = _myGun.GunData.rateOfFire;
            _target = _entity.AttackTarget;
        }

        public void OnExit()
        {
            _timer = 0;
            _target = null;
            Debug.Log("On Exit Follower Attack State");
        }

        public void OnFixedUpdate()
        {
        }

        public void OnUpdate()
        {
            Attack();
            if (_entity.HasToMoveInPath)
            {
                /* PARA QUE EL ENTITY SE VAYA EN CASO DE TENER QUE ESCAPAR
                if (_myEntity.HasLowLife)
                {
                    _myEntity.UpdateTargetPosition(_myEntity.GetRandomNodeToRun().transform.position);
                }
                */
                _fsm.ChangeState(FollowersEntitiesStates.Move);
                return;
            }

            if (!_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Idle);
                return;
            }
        }

        private void Attack()
        {
            _timer += Time.deltaTime;
            Vector3 lookDirection = (_target.transform.position - _entity.transform.position).normalized;
            _entity.transform.forward = lookDirection;

            if (_timer >= _myGun.GunData.rateOfFire)
            {
                _myGun.Attack(_target.transform.position);
                _timer = 0;
            }
        }
    }
}
