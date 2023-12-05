using UnityEngine;

namespace IA_I.Weapons
{
    [CreateAssetMenu(fileName = "Bow", menuName = "SO/Weapons/Bow", order = 0)]

    public class GunData : ScriptableObject
    {
        [field: SerializeField, Range(1f, 100f)] public float rateOfFire { get; private set; } = 2f;
    }
}
