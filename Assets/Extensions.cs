using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool IsPair (this int number)
    {
        var isPair = number % 2 == 0;
        Debug.Log($"IsPair {isPair}");
        return isPair;
    }

    public static GameObject GetClosesObject(this IEnumerable<GameObject> objs, Vector3 position)
    {
        if (objs == null) return null;


        var maxDistance = Mathf.Infinity;
        GameObject objectToReturn = null;
        foreach (var obj in objs)
        {
            var distance = Vector3.Distance(obj.transform.position, position);

            if (maxDistance == Mathf.Infinity || objectToReturn == null)
            {
                maxDistance = distance;
                objectToReturn = obj;
                continue;
            }


            if (distance <= maxDistance)
            {
                objectToReturn = obj;
                maxDistance = distance;
            }
        }
        Debug.Log($"Object To return {objectToReturn.name}");
        return objectToReturn;
    }

    public static IEnumerable<int> MapInts(this IEnumerable<int> myData, Func<int, int> myFunction)
    {
        foreach (var number in myData)
        {
            yield return myFunction(number);
        }
    }

    public static List<T> CustomToList<T>(this IEnumerable<T> myCollection)
    {
        var myList = new List<T>();

        foreach (var item in myCollection)
        {
            myList.Add(item);
        }
        return myList;
    }

    public static IEnumerable<Dst> CustomSelect<Src, Dst>(this IEnumerable<Src> collection, System.Func<Src, Dst> selector)
    {
        foreach (var item in collection)
        {
            yield return selector(item);
        }
    }
}
