using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IA_I
{
    public class TuplesAndAnonymus : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var tuple1 = new Tuple<string, int, Sprite>("Name", 1, null);
            var tuple2 = Tuple.Create<string, int, Sprite>("name", 1, null);

            //no puedo modificar las variables de la tupla
            Debug.Log($"{tuple1.Item1} {tuple1.Item2} {tuple1.Item3}");

            //no se puede hacer
            //tuple1.Item3 = null;
            //Forma de modificar un tuple
            tuple1 = new Tuple<string, int, Sprite>("New Name", 1, null);

            FindFile(tuple1);

            //No se puede usar como parametro en una funcion
            //var anonymusType = new { Name = "MyName", IdNumber = "1", MyImage = null}; No puede ser null
            var anonymusType = new { Name = "MyName", IdNumber = "1", MyImage = Sprite.Create(null, Rect.zero, Vector3.zero)}; //No puede ser null

            Debug.Log($"{anonymusType.Name} {anonymusType.IdNumber} {anonymusType.MyImage}");

            //Al igual que tuplas no se pueden asignar los valores individualmente pero si se puede crear un nuevo anonymusType

        }


        public void FindFile(Tuple<string, int, Sprite> tupleId)
        {

        }
    }
}
