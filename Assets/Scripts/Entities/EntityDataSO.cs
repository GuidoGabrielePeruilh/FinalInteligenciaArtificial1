using UnityEngine;

namespace IA_I.SO
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "SO/Entity Data", order = 0)]
    public class EntityDataSO : ScriptableObject
    {
        [field: SerializeField] public float speed { get; private set; } = 15f;
        [field: SerializeField] public float maxLife { get; private set; } = 500f;
        [field: SerializeField] public float distanceToLowSpeed { get; private set; } = 1f;
        [field: SerializeField, Range(0f, 1f)] public float percentageOfLifeToRunAway { get; private set; } = 0.1f;
        [field: SerializeField, Range(0f, 0.5f)] public float maxForce { get; private set; } = 0.1f;
        [field: SerializeField, Range(0.5f, 5)] public float attackCooldown { get; private set; } = 1f;
        [field: SerializeField, Range(0.5f, 5)] public float attackRadius { get; private set; } = 1f;
        [field: SerializeField, Range(0f, 360f)] public float viewAngle { get; private set; } = 45f;
        [field: SerializeField] public LayerMask obstacleLayerMask { get; private set; }
        [field: SerializeField] public LayerMask targetLayer { get; private set; }
    }
}
