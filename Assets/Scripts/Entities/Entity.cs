using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected EntityDataSO _myEntityData;
    [SerializeField] private LayerMask _targetLayer;

    public Teams MyTeam => _myTeam;
    [SerializeField] protected Teams _myTeam;
    public Vector3 Velocity => _velocity;
    protected Vector3 _velocity;
    protected Collider[] _targets;
    public GameObject AttackTarget => _attackTarget;
    protected GameObject _attackTarget;

    

    public void Initialize(EntityDataSO entityData)
    {
        _myEntityData = entityData;
    }

    protected virtual void AddForce(Vector3 force, float speed)
    {
        _velocity += force;
        _velocity = Vector3.ClampMagnitude(_velocity, speed);
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;
    }

    protected Vector3 CalculateSteering(Vector3 desired, float speed)
    {
        desired.Normalize();
        desired *= speed;
        return Vector3.ClampMagnitude(desired - _velocity, _myEntityData.maxForce);
    }

    public bool HaveTargetToAttack()
    {
        _targets = Physics.OverlapSphere(transform.position, _myEntityData.attackRadius, _targetLayer);

        if (_targets.Length == 0) return false;
        
        var filteredTargets = _targets.Where(target => 
        {
            var teamIdentifier = target.GetComponent<Entity>();
            return teamIdentifier == null || teamIdentifier.MyTeam != _myTeam; 
        })
            .ToArray();

        var myTarget = filteredTargets.GetClosesObject(transform.position);

        if (myTarget != null)
        {
            _attackTarget = myTarget.gameObject;
            return true;
        }
        _attackTarget = null;
        return false;
    }

    public void FollowPath(Stack<Node> pathToFollow)
    {
        if (pathToFollow.Count == 0) return;

        Vector3 nextPos = pathToFollow.Peek().transform.position;
        Vector3 dir = nextPos - transform.position;
        dir.y = 0;

        AddForce(CalculateSteering(dir, _myEntityData.speed), _myEntityData.speed);


        if (dir.sqrMagnitude < _myEntityData.distanceToLowSpeed * _myEntityData.distanceToLowSpeed)
        {
            pathToFollow.Pop();
        }
    }
}
