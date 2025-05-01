using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PersonTransform : MonoBehaviour
{
    [SerializeField] PersonIdentety personIdentety;
    const float speed = 5;
    Coroutine coroutine;
    public TaskType taskType { get; private set; }

    private void Start()
    {

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

    public bool RestTask()
    {
        IEnumerator _coroutine()
        {

            var point = WorldPoint.GetPoint(PointType.Rest);

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

    public bool DialogTask()
    {
        PersonTransform companion = personIdentety.GetDialogCompanion().personTransform;
        if (taskType > TaskType.imperative || companion.taskType > TaskType.imperative)
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

            yield return new WaitForSeconds(1.5f);
            //couple
            float chance1 = personIdentety.CalculateCoupleChance(companion.personIdentety);
            float chance2 = companion.personIdentety.CalculateCoupleChance(companion.personIdentety);
            float dice = Random.Range(1, 101);
            bool isCouple = chance1 + chance2 >= dice;

            personIdentety.AddDebugNote($"===CoupleChanceInDialog===\nI am own of dialog\ncompanion={companion.gameObject.name}\nmy chance = {chance1}\ncompanion chance = {chance2}\ndice = {dice}\nresult {isCouple}");
            companion.personIdentety.AddDebugNote($"===CoupleChanceInDialog===\ncompanion={gameObject.name}\nmy chance = {chance2}\ncompanion chance = {chance1}\ndice = {dice}\nresult: {isCouple}");

            if (isCouple)
            {
                var _mod = new CoupleMod();
                _mod.person = companion.personIdentety;
                personIdentety.AddModificator(_mod);

                _mod = new CoupleMod();
                _mod.person = personIdentety;
                companion.personIdentety.AddModificator(_mod);
            }


            yield return new WaitForSeconds(1.5f);
            //conflic
            chance1 = personIdentety.CalculateConflictChance(companion.personIdentety);
            chance2 = companion.personIdentety.CalculateConflictChance(companion.personIdentety);
            dice = Random.Range(1, 101);
            bool isConflic = chance1 + chance2 >= dice;

            personIdentety.AddDebugNote($"===ConflicChanceInDialog===\nI am own of dialog\ncompanion={companion.gameObject.name}\nmy chance = {chance1}\ncompanion chance = {chance2}\ndice = {dice}\nresult {isConflic}");
            companion.personIdentety.AddDebugNote($"===ConflicChanceInDialog===\ncompanion={gameObject.name}\nmy chance = {chance2}\ncompanion chance = {chance1}\ndice = {dice}\nresult: {isConflic}");

            if (isConflic)
            {

            }
            else
            {
                companion.WorkTask();
                WorkTask();
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
