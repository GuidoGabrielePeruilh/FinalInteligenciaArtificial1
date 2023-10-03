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
        return numbers.Aggregate(5, (acum, current) => acum + current);
    }
    
    private int Ejercicio3() {
        
        return numbers.Aggregate(1, (acum, current) => acum * current);
    }

    private List<int> Ejercicio4() {
        //Reemplacen esta linea con su ejercicio
        return new List<int>();
    }

    private IEnumerable<int> Ejercicio5() {
        //Reemplacen esta linea con su ejercicio
        return Enumerable.Empty<int>();
    }

    private List<int> Ejercicio6() {
        //Reemplacen esta linea con su ejercicio
        return new List<int>();
    }

    
}