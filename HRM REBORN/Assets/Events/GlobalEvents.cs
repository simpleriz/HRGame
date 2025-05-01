using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GlobalEvent
{
    public abstract void Init();
    public virtual string DebugInfo()
    {
        return $"====={this.ToString()}=====";
    }

    public abstract GameEvent CreateEvent(int time);
}

public class GlobalGhostEvent : GlobalEvent
{
    int count = 0;
    const int requiredValue = 12;
    public override void Init()
    {
        StartWorkDayEvent.OnStartDay.AddListener(OnStartDay);
    }

    void OnStartDay()
    {
        count += PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).Where(i => i.GetModificator<BlackMod>() != null).Count();
    }

    public override GameEvent CreateEvent(int time)
    {
        if (count >= requiredValue)
        {
            count = 0;
            return new GhostEvent(time);
        }
        return null;
    }

    public override string DebugInfo()
    {
        string ret = $"\n прогресс: {count}/{requiredValue}";
        return base.DebugInfo() + ret;
    }
}

public class GlobalImmigrationCheckEvent: GlobalEvent
{
    int count = 0;
    const int requiredValue = 18;
    public override void Init()
    {
        StartWorkDayEvent.OnStartDay.AddListener(OnStartDay);
    }

    void OnStartDay()
    {
        count += PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).Where(i => i.GetModificator<ArabMod>() != null || i.GetModificator<AsianMod>() != null).Count();
    }

    public override GameEvent CreateEvent(int time)
    {
        if (count >= requiredValue)
        {
            count = 0;
            return new ImmigrationCheckEvent(time);
        }
        return null;
    }

    public override string DebugInfo()
    {
        string ret = $"\n прогресс: {count}/{requiredValue}";
        return base.DebugInfo() + ret;
    }
}

public class GlobalJihadEvent : GlobalEvent
{
    int count = 0;
    const int requiredValue = 10;
    public override void Init()
    {
        StartWorkDayEvent.OnStartDay.AddListener(OnStartDay);
    }

    void OnStartDay()
    {
        count += PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).Where(i => i.GetModificator<JewishMod>() != null).Count();
    }

    public override GameEvent CreateEvent(int time)
    {
        if (count >= requiredValue)
        {
            count = 0;
            return new JihadEvent(time);
        }
        return null;
    }

    public override string DebugInfo()
    {
        string ret = $"\n прогресс: {count}/{requiredValue}";
        return base.DebugInfo() + ret;
    }
}

public class GlobalLGBTMeetingEvent : GlobalEvent
{
    int count = 0;
    const int requiredValue = 12;
    public override void Init()
    {
        StartWorkDayEvent.OnStartDay.AddListener(OnStartDay);
    }

    void OnStartDay()
    {
        var _count = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive).Where(i => i.GetModificator<BasicStats>().orientation != PersonOrientation.Hetero).Count();
        if(_count == 0)
        {
            count++;
        }
    }

    public override GameEvent CreateEvent(int time)
    {
        if (count >= requiredValue)
        {
            count = 0;
            return new LGBTMeetingEvent(time);
        }
        return null;
    }

    public override string DebugInfo()
    {
        string ret = $"\n прогресс: {count}/{requiredValue}";
        return base.DebugInfo() + ret;
    }
}

public class GlobalUnionMeetingEvent : GlobalEvent
{
    int count = 0;
    const int requiredValue = 15;
    public override void Init()
    {
        PersonManager.Instance.FireEvent.AddListener(OnFire);
    }

    void OnFire()
    {
        count++;
    }

    public override GameEvent CreateEvent(int time)
    {
        if (count >= requiredValue)
        {
            count = 0;
            return new UnionMeetingEvent(time);
        }
        return null;
    }

    public override string DebugInfo()
    {
        string ret = $"\n прогресс: {count}/{requiredValue}";
        return base.DebugInfo() + ret;
    }
}