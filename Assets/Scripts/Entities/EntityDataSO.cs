using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "SO/Entity Data", order = 0)]

public class EntityDataSO : ScriptableObject
{
    [field: SerializeField] public float speed { get; private set; } = 15f;
    [field: SerializeField] public float distanceToLowSpeed { get; private set; } = 1f;
    [field: SerializeField, Range(0f, 0.1f)] public float maxForce { get; private set; } = 0.1f;
    [field: SerializeField, Range(0.5f, 5)] public float attackCooldown { get; private set; } = 1f;
    [field: SerializeField, Range(0.5f, 5)] public float attackRadius { get; private set; } = 1f;
}
