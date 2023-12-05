using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.Weapons
{
    [CreateAssetMenu(fileName = "Bullet", menuName = "SO/Weapons/Arrow", order = 0)]

    public class BulletData : WeaponData
    {
        [field: SerializeField, Range(1f, 100f)] public float speed { get; private set; } = 2f;
        [field: SerializeField, Range(1f, 100f)] public float lifeTime { get; private set; } = 5f;
    }
}
