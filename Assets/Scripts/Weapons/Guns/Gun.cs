using IA_I.Bullets;
using IA_I.EntityNS.Follower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.Weapons.Guns
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] private ObjectPool _objectPool;
        [SerializeField] private Transform _shootPosition;
        [SerializeField] private GunData _gunData;
        public GunData GunData => _gunData;
        public Transform ShootPosition => _shootPosition;

        public void Attack(Vector3 dir, FollowersEntities owner)
        {
            var bulletObject = _objectPool.GetObject();
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.enabled = true;
            bullet.BulletPool = _objectPool;
            bullet.Shoot(dir, _shootPosition.position, owner);
        }

    }
}
