using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowersEntities : Entity
{
    [SerializeField] private GameObject _leaderToFollow;

    private void Start()
    {
        FollowersManager.Instance.RegisterNewFollower(this);
    }

    private void Update()
    {

        if (IsCloseFromLeader(_leaderToFollow))
        {
            AddForce(Arrive(_leaderToFollow), _myEntityData.speed);
        }
        else
        {
            Vector3 separationForce = Separation() * FollowersManager.Instance.SeparationWeight;
            Vector3 alignmentForce = Alignment() * FollowersManager.Instance.AlignmentWeight;
            Vector3 cohesionForce = Cohesion() * FollowersManager.Instance.CohesionWeight;

            Vector3 combinedForces = separationForce + alignmentForce + cohesionForce;

            AddForce(Seek(_leaderToFollow) + combinedForces, _myEntityData.speed);
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
        var speed = _myEntityData.speed;
        var distanceFromTarget = leader.transform.position - transform.position;


        desired += distanceFromTarget;
        float arriveRadius = FollowersManager.Instance.ArriveRadius;
        if (distanceFromTarget.magnitude <= arriveRadius)
        {
            speed = _myEntityData.speed * ((distanceFromTarget.sqrMagnitude + 1) / arriveRadius);
        }
        desired *= speed;

        if (desired.magnitude <= _myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed)
        {
            _velocity = Vector3.zero;
            return Vector3.zero;
        }

        return CalculateSteering(desired, speed);
    }

    Vector3 Seek(GameObject target)
    {
        Vector3 desired = Vector3.zero;
        float speed = _myEntityData.speed;

        Vector3 dirToTarget = target.transform.position - transform.position;

        if (desired.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
        {
            desired += dirToTarget;
        }

        if (desired == Vector3.zero) return desired;

        return CalculateSteering(desired, speed);
    }

    private Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
        {
            Vector3 dirToEntity = follower.transform.position - transform.position;

            if (dirToEntity.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
            {
                desired -= dirToEntity;
            }
        }
        Vector3 dirToLeader = _leaderToFollow.transform.position - transform.position;
        if (dirToLeader.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
        {
            desired -= dirToLeader;
        }

        if (desired.magnitude <= _myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed) return desired;

        return CalculateSteering(desired, _myEntityData.speed);
    }

    private Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;

        int count = 0;

        foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
        {
            if (follower == this) continue;

            Vector3 dirToBoid = follower.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
            {
                desired += follower._velocity;
                count++;
            }
        }

        if (count == 0) return desired;

        desired /= count;

        return CalculateSteering(desired, _myEntityData.speed);
    }

    private Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (FollowersEntities follower in FollowersManager.Instance.AllFollowers)
        {
            if (follower == this) continue;
            Vector3 dirToBoid = follower.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= FollowersManager.Instance.ViewRadius)
            {
                desired += follower.transform.position;

                count++;
            }
        }

        if (count == 0) return desired;

        desired /= count;

        desired -= transform.position;

        return CalculateSteering(desired, _myEntityData.speed);
    }

    private void OnDrawGizmosSelected()
    {
       
        Gizmos.color = Color.yellow; // Puedes cambiar el color a tu preferencia

        // Dibuja un gizmo de esfera para el rango de llegada (Arrive)
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(_myEntityData.distanceToLowSpeed));
    }
}
