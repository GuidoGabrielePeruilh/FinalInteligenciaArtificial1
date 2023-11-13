using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace IA_I
{
    public class Aggregate : MonoBehaviour
    {
        [SerializeField] int[] myInts = new int[] { 1, 2, 3, 4, 5 };
        private void Start()
        {
            int myIntsSum = myInts.Aggregate(0, (acum, current) => acum + current);
            Debug.Log(myIntsSum);

            int max = myInts.Aggregate(0, (max, current) => max < current ? current : max);
            Debug.Log(max);

            var mul = myInts.Aggregate(Tuple.Create(0,0), (acum, current) => {
                if (acum.Item1 < 3)
                    return Tuple.Create(acum.Item1 + 1, current);
                else
                    return Tuple.Create(acum.Item1 + 1, acum.Item2);
                });

            Debug.Log($"El tercer elemento es {mul.Item2}");
        }
    }
}
