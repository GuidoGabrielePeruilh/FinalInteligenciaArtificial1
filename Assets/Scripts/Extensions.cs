using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static GameObject GetClosesObject(this IEnumerable<Collider> objs, Vector3 position)
    {
        if (objs == null) return null;


        var maxDistance = Mathf.Infinity;
        GameObject objectToReturn =  null;
        foreach (var obj in objs)
        {
            var distance = Vector3.Distance(obj.transform.position, position);

            if (maxDistance == Mathf.Infinity || objectToReturn == null)
            {
                maxDistance = distance;
                objectToReturn = obj.gameObject;
                continue;
            }


            if (distance <= maxDistance)
            {
                objectToReturn = obj.gameObject;
                maxDistance = distance;
            }
        }

        return objectToReturn;
    }
}
