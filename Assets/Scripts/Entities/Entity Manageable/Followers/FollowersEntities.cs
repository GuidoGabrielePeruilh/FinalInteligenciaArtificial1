using IA_I.EntityNS.Manegeable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.EntityNS.Follower
{
    public class FollowersEntities : Entity
    {
        [SerializeField] private ManageableEntities _leaderToFollow;

        private void Start()
        {
            FollowersManager.Instance.RegisterNewFollower(this, _leaderToFollow);
        }

        private void Update()
        {
            if (_leaderToFollow == null) return;

            var separationForce = Separation() * FollowersManager.Instance.SeparationWeight;
            var alignmentForce = Alignment() * FollowersManager.Instance.AlignmentWeight;
            var cohesionForce = Cohesion() * FollowersManager.Instance.CohesionWeight;

            if (IsCloseFromLeader(_leaderToFollow.gameObject))
            {
                if (_leaderToFollow.Velocity.sqrMagnitude < 0.1f)
                {
                    AddForce(Separation() * FollowersManager.Instance.SeparationWeight, MyEntityData.speed);
                }
                else
                {
                    AddForce(Arrive(_leaderToFollow.gameObject) + separationForce, MyEntityData.speed);
                }
            }
            else
            {
                var combinedForces = separationForce + alignmentForce + cohesionForce;
                AddForce(Seek(_leaderToFollow.gameObject) + combinedForces, MyEntityData.speed);
            }
        }

        private bool IsCloseFromLeader(GameObject leader)
        {
            var distanceFromTarget = leader.transform.position - transform.position;
            return distanceFromTarget.sqrMagnitude <= FollowersManager.Instance.ViewRadius;
        }

        Vector3 Arrive(GameObject leader)
        {
            var desired = Vector3.zero;
            var speed = MyEntityData.speed;
            var distanceFromTarget = leader.transform.position - transform.position;


            desired += distanceFromTarget;
            float arriveRadius = FollowersManager.Instance.ArriveRadius;
            if (distanceFromTarget.sqrMagnitude <= arriveRadius)
            {
                speed = MyEntityData.speed * ((distanceFromTarget.sqrMagnitude + 1) / arriveRadius);
            }
            desired *= speed;

            if (desired.sqrMagnitude <= MyEntityData.distanceToLowSpeed * MyEntityData.distanceToLowSpeed)
            {
                _velocity = Vector3.zero;
                return Vector3.zero;
            }

            return CalculateSteering(desired, speed);
        }

        Vector3 Seek(GameObject target)
        {
            Vector3 desired = Vector3.zero;

            Vector3 dirToTarget = target.transform.position - transform.position;
            desired += dirToTarget;

            if (desired.sqrMagnitude <= FollowersManager.Instance.SeparationRadius)
            {
                _velocity = Vector3.zero;
                desired = Vector3.zero;
            }

            if (desired == Vector3.zero) return desired;
            return CalculateSteering(desired, MyEntityData.speed);
        }

        private Vector3 Separation()
        {
            Vector3 desired = Vector3.zero;

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;

                Vector3 dirToEntity = follower.transform.position - transform.position;

                if (dirToEntity.sqrMagnitude <= FollowersManager.Instance.SeparationRadius)
                {
                    desired -= dirToEntity;
                }
            }

            if (desired == Vector3.zero) return desired;

            return CalculateSteering(desired, MyEntityData.speed);
        }

        private Vector3 Alignment()
        {
            Vector3 desired = Vector3.zero;

            int count = 0;

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;

                Vector3 dirToBoid = follower.transform.position - transform.position;

                if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.AlignmentRadius)
                {
                    desired += follower.Velocity;
                    count++;
                }
            }

            if (count == 0) return desired;

            desired /= count;

            return CalculateSteering(desired, MyEntityData.speed);
        }

        private Vector3 Cohesion()
        {
            Vector3 desired = Vector3.zero;
            int count = 0;

            foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
            {
                if (follower == this) continue;
                Vector3 dirToBoid = follower.transform.position - transform.position;

                if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.CohesionRadius)
                {
                    desired += follower.transform.position;

                    count++;
                }
            }

            if (count == 0) return desired;

            desired /= count;

            desired -= transform.position;

            return CalculateSteering(desired, MyEntityData.speed);
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.ArriveRadius));


            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.ViewRadius));


            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(FollowersManager.Instance.SeparationRadius));
        }
    }
}

