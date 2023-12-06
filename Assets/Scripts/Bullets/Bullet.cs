using IA_I.Damageables;
using IA_I.EntityNS;
using IA_I.EntityNS.Follower;
using IA_I.EntityNS.Manegeable;
using IA_I.Weapons;
using UnityEngine;

namespace IA_I.Bullets
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected BulletData _bulletData;
        public ObjectPool BulletPool { set; private get; }
        private Rigidbody _myRB;
        private Vector3 _direction;
        private float _timer = 0f;
        private FollowersEntities _owner;

        private void Awake()
        {
            _myRB = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _bulletData.lifeTime)
            {
                _timer = 0f;
                DestroyBullet();
            }
        }

        private void FixedUpdate()
        {
            _myRB.MovePosition(transform.position + _direction.normalized * (_bulletData.speed * Time.fixedDeltaTime));
        }

        public void Shoot(Vector3 dir, Vector3 initialPosition, FollowersEntities owner)
        {
            _direction = dir - initialPosition;
            transform.position = initialPosition;
            transform.rotation = Quaternion.LookRotation(_direction);
            _owner = owner;
        }

        private void DestroyBullet()
        {
            if (BulletPool == null)
                Destroy(gameObject);
            else
                BulletPool.ReturnObject(gameObject);
        }

        private void OnTriggerEnter(Collider collider)
        {
            var other = collider.GetComponentInParent<Entity>();

            if (other == null || other == _owner.LeaderToFollow || other == _owner) return;
            DestroyBullet();

            other.gameObject.GetComponent<IDamageable>()?.TakeDamage(_bulletData.damage);

        }
    }
}
