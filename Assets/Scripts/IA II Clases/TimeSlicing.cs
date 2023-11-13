using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

namespace IA_I
{
    public class TimeSlicing : MonoBehaviour
    {
        [SerializeField] string[] myStringArray = new string[0];
        public const float Target_FPS = 1f / 60f;

        IEnumerator Process()
        {
            Stopwatch myStopwatch = new Stopwatch();

            myStopwatch.Start();

            for (int i = 0; i < myStringArray.Length; i++)
            {
                if (myStopwatch.ElapsedMilliseconds > Target_FPS )
                {
                    yield return null;
                    myStopwatch.Restart(); //Para que vuelva a contar milisegundos
                }
            }
        }
    }
}
