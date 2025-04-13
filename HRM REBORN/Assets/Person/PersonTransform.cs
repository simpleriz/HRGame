using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PersonTransform : MonoBehaviour
{
    const float speed = 5;
    Coroutine coroutine;
    public void SetCouutine(IEnumerator rutine)
    {
        StopCoroutine(coroutine);
        coroutine = StartCoroutine(rutine);
    }
    
    bool MoveTo(Vector3 pos, float speed)
    {
        if (Vector2.Distance(pos, transform.position) < speed)
        {
            transform.position = pos;
            return true;
        }
        else
        {
            Vector2 direction = pos - transform.position;

            direction = direction.normalized;
            Vector2 offset = direction * speed;

            transform.position = (Vector2)transform.position + offset;
            return false;
        }

    }
    IEnumerator WorkTask()
    {
        var point = WorldPoint.GetPoint(PointType.Work);

        while (true)
        {
            if (MoveTo(point.pos, speed * Time.deltaTime))
            {
                break;
            }
            yield return null;
        }
    }
}


