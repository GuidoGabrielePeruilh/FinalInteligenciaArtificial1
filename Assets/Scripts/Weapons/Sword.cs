using IA_I.Damageables;
using IA_I.EntityNS;
using UnityEngine;

namespace IA_I.Weapons
{
    public class Sword : MonoBehaviour
    {
        [SerializeField] private WeaponData _swordData;
        private void OnTriggerEnter(Collider collider)
        {
            var other = collider.GetComponentInParent<Entity>();
            if (other == null) return;

            if (other.Team != GetComponentInParent<Entity>().Team)
               other.gameObject.GetComponent<IDamageable>()?.TakeDamage(_swordData.damage);

        }
    }
}
