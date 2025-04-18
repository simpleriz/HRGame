using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PersonTransform : MonoBehaviour
{
    [SerializeField] PersonIdentety personIdentety;
    const float speed = 5;
    Coroutine coroutine;
    public TaskType taskType { get; private set; }

    private void Start()
    {
        WorkTask();
        Debug.Log(TaskType.imperative < TaskType.important);
    }
    bool SetCouutine(IEnumerator rutine, TaskType type)
    {
        if (type < taskType) return false;
        taskType = type;    
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(rutine);
        return true;
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


    public bool WorkTask()
    {
        IEnumerator _coroutine()
        {

            var point = WorldPoint.GetPoint(PointType.Work);

            while (true)
            {
                yield return null;
                if (MoveTo(point.pos, speed * Time.deltaTime))
                {
                    break;
                }
            }
        }
        return SetCouutine(_coroutine(), TaskType.common);
    }

    public bool DialogTask(PersonTransform companion)
    {
        if(taskType > TaskType.imperative || companion.taskType > TaskType.imperative)
        {
            return false;
        }
        var point = WorldPoint.GetPoint(PointType.Dialog);
        IEnumerator _coroutine()
        {   
            while (true)
            {
                yield return null;
                if (MoveTo(point.pos, speed * Time.deltaTime))
                {
                    break;
                }
            }

            yield return new WaitForSeconds(3f);

            float chance1 = personIdentety.CalculateConflictChance(companion.personIdentety);
            float chance2 = companion.personIdentety.CalculateConflictChance(companion.personIdentety);
            float dice = Random.Range(1, 101);
            bool isConflic = chance1 + chance2 >= dice;

            personIdentety.AddDebugNote($"===ConflicChanceInDialog===\nI am own of dialog\nmy chance = {chance1}\ncompanion chance = {chance2}\ndice = {dice}\nresult {isConflic}");
            companion.personIdentety.AddDebugNote($"===ConflicChanceInDialog===\nmy chance = {chance2}\ncompanion chance = {chance1}\ndice = {dice}\nresult: {isConflic}");

            if (isConflic)
            {

            }
        }
        SetCouutine(_coroutine(), TaskType.important);
        companion.DialogCompanionTask(point);
        return true;
    }

    public bool DialogCompanionTask(MapPoint point)
    {
        IEnumerator _coroutine()
        {
            while (true)
            {
                yield return null;
                if (MoveTo(point.additionalPos, speed * Time.deltaTime))
                {
                    break;
                }
            }
        }
        return SetCouutine(_coroutine(), TaskType.important);
    }
}


public enum TaskType
{
    common, // work, rest
    important, // dialog
    imperative, // death, wound
}
