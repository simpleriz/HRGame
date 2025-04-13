using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldPoint : MonoBehaviour
{
    [SerializeField] PointType type;
    [SerializeField] Transform additionalPosition;
    
    void Start()
    {
        if (points[type] == null)
        {
            points[type] = new List<MapPoint>();
        }
        points[type].Add(new MapPoint(transform.position, additionalPosition.position));
        Destroy(gameObject);
    }

    static Dictionary<PointType, List<MapPoint>> points;

    public static MapPoint GetPoint(PointType type, int exceptionCounter = 0)
    {
        if(exceptionCounter == points[type].Count)
        {
            return null;
        }
        var value = points[type][0];
        points[type].Add(value);
        points[type].RemoveAt(0);
        if (!value.isFree)
        {
            return GetPoint(type, exceptionCounter+1);
        }
        return value;
    }
}

public class MapPoint
{
    public Vector3 pos;
    public Vector3 additionalPos;
    public bool isFree;
    public MapPoint(Vector3 point,Vector3 additionalPoint)
    {
        this.pos = point;
        this.additionalPos = additionalPoint;
    }
}

public enum PointType
{
    Rest,
    Work,
    Enter,
}
