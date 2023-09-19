using IA_I.Damageables;

namespace IA_I.Entity
{
    public class EntityDamageable : Damageable, IDamageable
    {
        private Entity _entity;

        protected override void Awake()
        {
            base.Awake();
            _entity = GetComponent<Entity>();
        }

        public void TakeDamage(float damage)
        {
            _entity.OnDamageRecived(damage);
            _slider.UpdateSlider(_entity.CurrentLife, _entity.MyEntityData.maxLife);
        }
    }
}
