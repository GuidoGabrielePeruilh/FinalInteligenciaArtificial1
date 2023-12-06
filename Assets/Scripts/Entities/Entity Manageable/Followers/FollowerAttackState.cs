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
        Transform _target;

        public FollowerAttackState(FSM<FollowersEntitiesStates> fsm, FollowersEntities entity, Gun gun)
        {
            _fsm = fsm;
            _entity = entity;
            _myGun = gun;
        }

        public void OnEnter()
        {
            Debug.Log("Enter Attack");
            _timer = _myGun.GunData.rateOfFire;
            _target = _entity.AttackTarget;
        }

        public void OnExit()
        {
            _timer = 0;
            _target = null;
        }

        public void OnLateUpdate()
        {
            _entity.FOV();
        }

        public void OnUpdate()
        {


            if (_entity.HasLowLife)
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

            if (_target !=  null)
            {
                Attack();
            }
        }

        private void Attack()
        {
            _timer += Time.deltaTime;

            Vector3 lookDirection = (_target.transform.position - _entity.transform.position).normalized;
            _entity.transform.forward = lookDirection;

            if (_timer >= _myGun.GunData.rateOfFire)
            {
                _myGun.Attack(_target.transform.position, _entity);
                _timer = 0;
            }
        }
    }
}
