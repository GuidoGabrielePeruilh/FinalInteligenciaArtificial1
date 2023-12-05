using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace IA_I
{
    public abstract class ObjectPool : MonoBehaviour
    {
        [SerializeField] private int _initialPoolSize;
        [SerializeField] private Transform parentTransform;
        private List<GameObject> objects;

        private void Awake()
        {
            objects = new List<GameObject>(_initialPoolSize);
            StartCoroutine(ProcessCreationOfPools(objects));
        }

        IEnumerator ProcessCreationOfPools(List<GameObject> goToCreate)
        {
            Stopwatch myStopWatch = new Stopwatch();
            myStopWatch.Start();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewObject();
                if (myStopWatch.ElapsedMilliseconds > 1f / 60f)
                {
                    yield return new WaitForEndOfFrame();
                    myStopWatch.Restart();
                }
            }
        }

        public GameObject GetObject()
        {
            GameObject obj = objects.FirstOrDefault(o => !o.activeInHierarchy);

            if (obj == null)
            {
                obj = CreateNewObject();
            }

            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(parentTransform);
        }

        private GameObject CreateNewObject()
        {
            GameObject obj = IntantiateObject();
            obj.SetActive(false);
            obj.transform.SetParent(parentTransform);
            objects.Add(obj);
            return obj;
        }

        protected abstract GameObject IntantiateObject();
    }
}
