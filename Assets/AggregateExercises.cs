using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AggregateExercises : MonoBehaviour {

    public int[] numbers = {1, 0, 5, 3, 4, 4, 4, 0, 8, 100, 25, 2, 8, 6, 3, 5, 99, 23};
    
    
    void Start() {
        #region Inicialización de Items

        #endregion

        var ej1 = Ejercicio1();
        var ej2 = Ejercicio2();
        var ej3 = Ejercicio3();
        var ej4 = Ejercicio4();
        var ej5 = Ejercicio5();
        var ej6 = Ejercicio6();
        
        Debug.Log($"Ejercicio 1: {ej1}");
        
        Debug.Log($"Ejercicio 2: {ej2}");
        
        Debug.Log($"Ejercicio 3: {ej3}");

        Debug.Log("Ejercicio 4: ");
        foreach (var num in ej4) {
            Debug.Log(num);
        }
        
        Debug.Log("Ejercicio 5: ");
        foreach (var num in ej5) {
            Debug.Log(num);
        }
        
        Debug.Log("Ejercicio 6: ");
        foreach (var num in ej6) {
            Debug.Log(num);
        }
        
    }

    private int Ejercicio1() {
        return numbers.Aggregate(0, (acum, current) => acum + current);
    }
    
    private int Ejercicio2() {
        return numbers.Aggregate(new Tuple<int, int>(0, 0), (myTuple, number) =>
        {
            var total = myTuple.Item2;
            if (myTuple.Item1 >= 5)
            {
                total += number;
            }
            Tuple<int, int> newTuple = Tuple.Create<int, int>(myTuple.Item1 + 1, total);
            return newTuple;
        }).Item2;
        //return numbers.Skip(5).Aggregate(0, (acum, current) => acum + current);
    }
    
    private int Ejercicio3() {
        
        return numbers.Aggregate(1, (acum, current) => acum * current);
    }

    private List<int> Ejercicio4() {

        return numbers.Aggregate(new List<int>(), (pairNumbers, current) =>
        {
            if (current % 2 == 0)
            {
                pairNumbers.Add(current);
            }
            return pairNumbers;
        });
    }

    private IEnumerable<int> Ejercicio5() {

        return numbers.Where(number => number % 2 == 0);
    }

    private List<int> Ejercicio6() {

        return numbers.Aggregate(new Tuple<int, List<int>>(0, new List<int>()), (myTuple, number) =>
        {
            if (myTuple.Item1 % 2 == 0)
            {
                myTuple.Item2.Add(number);
            }
            var newTuple = Tuple.Create<int, List<int>>(myTuple.Item1 + 1, myTuple.Item2); 
            return newTuple;
        }).Item2;
    }

    
}