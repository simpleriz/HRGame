
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


