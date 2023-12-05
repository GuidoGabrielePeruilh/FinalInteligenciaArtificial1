using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I.Bullets
{
    public class BulletPool : ObjectPool
    {
        public BulletName BulletName;
        protected override GameObject IntantiateObject()
        {
            return BulletFactory.Instance.CreateObject(BulletName);
        }
    }
}
