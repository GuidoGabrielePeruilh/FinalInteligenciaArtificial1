using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IA_I
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private SquareQuery query;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var myEnemies = GetWeekEnemies();

                foreach (var item in myEnemies)
                {
                    Debug.Log(item.name);
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var myEnemies = GetClosestEnemy();

                Debug.Log(myEnemies);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var myEnemies = GetClosestsEnemy();

                foreach (var item in myEnemies)
                {
                    Debug.Log(item.name);
                }
            }
        }

        public IEnumerable<Enemy> GetWeekEnemies()
        {
            return query.Query().Select(x => (Enemy)x).Where(x => x.hp < 5);
        }

        public Enemy GetClosestEnemy()
        {
            return query.Query()
                .Select(x => (Enemy)x) //Se podria sacar y que devuelva un IgridEntity
                .OrderBy(x => Vector3.Distance(x.Position, query.transform.position))
                .FirstOrDefault();
        }

        public IEnumerable<Enemy> GetClosestsEnemy()
        {
            return query.Query()
                .Select(x => (Enemy)x)
                .OrderBy(x => Vector3.Distance(x.Position, query.transform.position))
                .Take(3);
        }
    }
}
