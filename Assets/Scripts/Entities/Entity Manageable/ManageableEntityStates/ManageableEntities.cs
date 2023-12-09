using IA_I.EntityNS.Follower;
using IA_I.FSM.StatesBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.EntityNS.Manegeable
{
    public class ManageableEntities : Entity
    {
        FSM<ManageableEntityStates> _fsm;
        [SerializeField] Animator myAnimator;
        [SerializeField] List<FollowersEntities> _myFollowers;

        private void Awake()
        {
            base.Awake();
            UpdateTargetPosition(transform.position);
            _fsm = new FSM<ManageableEntityStates>();

            IState move = new EntityMoveState(_fsm, this);
            IState attack = new EntityAttackState(_fsm, this, myAnimator, "Attack", MyEntityData.attackCooldown);
            IState idle = new EntityIdleState(_fsm, this);
            _fsm.AddState(ManageableEntityStates.Move, move);
            _fsm.AddState(ManageableEntityStates.Attack, attack);
            _fsm.AddState(ManageableEntityStates.Idle, idle);

        }

        private void Start()
        {
            HasToMove = false;
            _fsm.ChangeState(ManageableEntityStates.Move);
        }

        private void Update()
        {
            _fsm.Update();

        }

        private void LateUpdate()
        {
            _fsm.LateUpdate();
        }

        public override void UpdateTargetPosition(Vector3 targetPosition)
        {
            HasToMove = true;
            TargetPosition = targetPosition;
        }

        protected override void BasicMove(Vector3 dir)
        {
            AddForce(CalculateSteering(dir, MyEntityData.speed), MyEntityData.speed);
        }

        public void AddFollower(FollowersEntities follower)
        {
            if (!_myFollowers.Contains(follower))
                _myFollowers.Add(follower);
        }

        public void RemoveFollower(FollowersEntities follower)
        {
            if (_myFollowers.Contains(follower))
                _myFollowers.Remove(follower);
        }

        private void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, MyEntityData.attackRadius);
        }
    }
}

