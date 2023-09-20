using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.SO
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "SO/Weapons", order = 0)]

    public class WeaponData : ScriptableObject
    {
        [field: SerializeField, Range(1f, 100f)] public float damage { get; private set; } = 25f;
    }
}
