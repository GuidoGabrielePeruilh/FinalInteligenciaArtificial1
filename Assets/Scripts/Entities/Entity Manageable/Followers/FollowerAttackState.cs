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

        public FollowerAttackState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity, Gun gun)
        {
            _fsm = fsm;
            _entity = entity;
            _myGun = gun;
        }

        public void OnEnter()
        {
            _timer = _myGun.GunData.rateOfFire;
        }

        public void OnExit()
        {
            _timer = 0;
        }

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {

            if (_entity.HasToRunAway)
            {
                _fsm.ChangeState(FollowersEntitiesStates.RunAway);
                return;
            }

            if (!_entity.HaveTargetToAttack())
            {
                _fsm.ChangeState(FollowersEntitiesStates.Idle);
                return;
            }

            if (_entity.HasToMove)
            {
                _fsm.ChangeState(FollowersEntitiesStates.Seek);
                return;
            }

            if (_entity.AttackTarget !=  null)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _timer += Time.deltaTime;

            Vector3 lookDirection = (_entity.AttackTarget.transform.position - _entity.transform.position).normalized;
            _entity.transform.forward = lookDirection;

            if (_timer >= _myGun.GunData.rateOfFire)
            {
                _myGun.Attack(_entity.AttackTarget.transform.position, _entity);
                _timer = 0;
            }
        }
    }
}
