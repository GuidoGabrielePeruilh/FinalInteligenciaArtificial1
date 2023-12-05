using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IA_I
{
    public abstract class Factory<T> : Singleton<Factory<T>> where T : Enum
    {
        [Serializable]
        public class PrefabData
        {
            public T name;
            public GameObject prefab;
        }

        [SerializeField] private List<PrefabData> _data;
        public List<PrefabData> FactoryData => _data;

        protected override void Awake()
        {
            itDestroyOnLoad = true;
            base.Awake();
        }
        public GameObject CreateObject(T objectName)
        {
            var objectData = _data.Where(obj => obj.name.ToString() == objectName.ToString()).FirstOrDefault();
            if (objectData != null)
            {
                return Instantiate(objectData.prefab);
            }

            Debug.LogWarning("No existe el nombre " + objectName);
            return null;
        }
    }
}
