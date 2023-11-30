using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T GetClosestObject<T>(this IEnumerable<T> objs, Vector3 position) where T : Component
    {
        if (objs == null) return default(T);

        var maxDistance = Mathf.Infinity;
        T objectToReturn = default(T);

        foreach (var obj in objs)
        {
            var objTransform = obj.transform;
            if (objTransform == null)
                continue;

            var distance = Vector3.Distance(objTransform.position, position);

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

        return objectToReturn;
    }

    public static bool InLineOfSight(Vector3 start, Vector3 end, LayerMask blockViewLayer)
    {
        Vector3 dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, blockViewLayer);
    }
}
