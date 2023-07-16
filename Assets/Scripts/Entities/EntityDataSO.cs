using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "SO/Entity Data", order = 0)]

public class EntityDataSO : ScriptableObject
{
    [field: SerializeField] public float speed { get; private set; } = 15f;
    [field: SerializeField] public float distanceToLowSpeed { get; private set; } = 1f;
    [field: SerializeField, Range(0f, 0.1f)] public float _maxForce { get; private set; } = 0.1f;
    [field: SerializeField, Range(25, 350)] public float _timeToStop { get; private set; } = 50f;
}
