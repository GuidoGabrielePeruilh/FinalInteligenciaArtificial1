using UnityEngine;

namespace IA_I.SO
{
    [CreateAssetMenu(fileName = "EntityData", menuName = "SO/Entity Data", order = 0)]
    public class EntityDataSO : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; } = 15f;
        [field: SerializeField] public float MaxLife { get; private set; } = 500f;
        [field: SerializeField] public float DistanceToLowSpeed { get; private set; } = 1f;
        [field: SerializeField, Range(0f, 1f)] public float PercentageOfLifeToRunAway { get; private set; } = 0.1f;
        [field: SerializeField, Range(0f, 0.5f)] public float MaxForce { get; private set; } = 0.1f;
        [field: SerializeField, Range(0.5f, 5)] public float AttackCooldown { get; private set; } = 1f;
        [field: SerializeField, Range(0.5f, 5)] public float AttackRadius { get; private set; } = 1f;
        [field: SerializeField, Range(0f, 360f)] public float ViewAngle { get; private set; } = 45f;
        [field: SerializeField] public LayerMask ObstacleLayerMask { get; private set; }
        [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }
    }
}
