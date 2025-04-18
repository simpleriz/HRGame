
using System.Collections;
using System.Collections.Generic;
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
        if(TimeManager.Instance.actuallyMinute >= activateTime)
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
