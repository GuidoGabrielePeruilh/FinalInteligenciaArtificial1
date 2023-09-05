using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var modifiedNumbers = myNumbers.MapInts(x => x + 1).ToList();

        foreach (int number in modifiedNumbers)
        {
            Debug.Log(number);
        }

        modifiedNumbers.ToList();
    }
}
