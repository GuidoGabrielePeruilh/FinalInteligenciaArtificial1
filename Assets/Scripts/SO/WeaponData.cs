using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.Weapons
{
    public abstract class WeaponData : ScriptableObject
    {
        [field: SerializeField, Range(1f, 100f)] public float damage { get; private set; } = 25f;
    }
}
