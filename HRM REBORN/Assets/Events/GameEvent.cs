
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
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
        Console.Instance.PrintText("Start of the working day!");
        foreach (var p in PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).ToList())
        {
            p.personTransform.WorkTask();
            Console.Instance.PrintText($"{p.GetModificator<BasicStats>().name} was active and called to work");
        }
        OnStartDay.Invoke();
    }
}

public class EndWorkDayEvent : GameEvent
{
    public static UnityEvent OnEndDay = new UnityEvent();
    public EndWorkDayEvent(int _activateTime) : base(_activateTime)
    {

    }
    protected override void Start()
    {
        if(EventManager.Instance.state == EventStates.Free)
        {
            OnEndDay.Invoke();
        }
        else
        {
            activateTime += 20;
        }
    }
}

public class DialogEvent    : GameEvent 
{
    public static int lastDailogBegin = 0;
    public static int lastDailogEnd = 0;
    bool isMain;
    public DialogEvent(int _activateTime, bool isMain = true) : base(_activateTime)
    {
        this.isMain = isMain;
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

        EventManager.Instance.state = EventStates.Common;

        //lastDailogBegin = TimeManager.Instance.actuallyMinute;

        List<PersonTransform> persons;
        if (isMain)
        {
            persons = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).OrderBy(i => Random.Range(0, 1)).Select(i => i.personTransform).ToList();
        }
        else
        {
            persons = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive & i.GetModificator<JoillerMod>() != null).OrderBy(i => Random.Range(0, 1)).Select(i => i.personTransform).ToList();
        }

        if (persons.Count > 0)
        {

            foreach (var person in persons)
            {
                if (person.DialogTask())
                {
                    break;
                }
            }
        }


    }   
}

public class HireEvent : GameEvent
{
    public HireEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}

//globals
public class GhostEvent : GameEvent
{
    public GhostEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}

public class ImmigrationCheckEvent : GameEvent
{
    public ImmigrationCheckEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}
public class JihadEvent : GameEvent
{
    public JihadEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}
public class LGBTMeetingEvent : GameEvent
{
    public LGBTMeetingEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}
public class UnionMeetingEvent : GameEvent
{
    public UnionMeetingEvent(int _activateTime) : base(_activateTime) { }


    protected override void Start()
    {

    }
}

