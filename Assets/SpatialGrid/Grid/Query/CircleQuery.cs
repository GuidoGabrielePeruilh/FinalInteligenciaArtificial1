using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I
{
    public class CircleQuery : MonoBehaviour, IQuery
    {
        [SerializeField] private SpatialGrid targetGrid = null;
        [SerializeField] private float radious = 5;

        public IEnumerable<IGridEntity> Query()
        {
            return targetGrid.Query(
                                    transform.position + new Vector3(-radious, 0, -radious),
                                    transform.position + new Vector3(radious, 0, radious), //Esto me devuelve un cuadrado
                                    x => (x - transform.position).sqrMagnitude <= radious * radious); //Si esto me devuelve true es porque queda en la lista
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radious);
        }
    }
}
