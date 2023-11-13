using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    [SerializeField] int myNumber = 2;
    [SerializeField] GameObject[] myObjects;
    [SerializeField] int[] myNumbers;
    private void Start()
    {
        myNumber.IsPair();
        myObjects.GetClosesObject(transform.position);

        //myNumbers es una y array y le paso el mismo como parametro y ademas le agrego el Func<int, int> = x => x+1. Func<int es el x que recibe, int x+1 es el int que devuelve>
        //Al agregarle el metodo ToList deja de ser Lazy porque ese metodo ya llama al yield return
        var modifiedNumbers = myNumbers.MapInts(x => x + 1).CustomToList();

        foreach (int number in modifiedNumbers)
        {
            Debug.Log(number);
        }

        modifiedNumbers.ToList();

        IEnumerable<string> myStringCollection = myNumbers.Select(number => number.ToString()); //LinQ
        IEnumerable<string> myStringCollection2 = myNumbers.CustomSelect(number => number.ToString()); //Custom

        myStringCollection.ToArray();
        myStringCollection.ToList();


    }



}
