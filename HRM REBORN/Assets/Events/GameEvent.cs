
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent
{
    protected int activateTime;
    public GameEvent(int _activateTime)
    {
        activateTime = _activateTime;
        TimeManager.Instance.OnMinute.AddListener(CheckTime);
    }

    void CheckTime()
    {
        if(TimeManager.Instance.actuallyMinute == activateTime)
        {
            Start();
        }
    }

    protected abstract void Start();
}



public class StartWorkDayEvent : GameEvent
{
    public static UnityEvent OnStartDay = new UnityEvent();
    public StartWorkDayEvent(int _activateTime) : base(_activateTime)
    {

    }
    protected override void Start()
    {
        OnStartDay.Invoke();
    }
}

public class DialogEvent : GameEvent 
{
    public static int lastDailogBegin = 0;
    public static int lastDailogEnd = 0;
    public DialogEvent(int _activateTime) : base(_activateTime)
    {

    }
    protected override void Start()
    {
        if(lastDailogBegin > lastDailogEnd & !(lastDailogEnd == 0 & lastDailogEnd == 0))
        {
            return;
        }

        if (EventManager.Instance.state != EventStates.Free)
        {
            return;
        }

        lastDailogBegin = TimeManager.Instance.actuallyMinute;

        var persons = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).OrderBy(i=>Random.Range(0,1)).Select(i => i.personTransform).ToList();
        
    }
}