using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class EventManager : MonoBehaviour
{
    static public EventManager Instance;
    public EventStates state = EventStates.Free;
    string lastDayDebug = "";
    List<GlobalEvent> globalEvents = new List<GlobalEvent>
    {
        new GlobalGhostEvent(),
        new GlobalLGBTMeetingEvent(),
        new GlobalImmigrationCheckEvent(),
        new GlobalJihadEvent(),
        new GlobalUnionMeetingEvent(),
    };
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitGame();
        foreach (GlobalEvent e in globalEvents)
        {
            e.Init();
        }
        StartWorkDayEvent.OnStartDay.AddListener(GenerateDayEvents);
        TimeManager.Instance.OnDay.AddListener(GenerateAfterDayEvent);
    }

    void GenerateAfterDayEvent()
    {
        Debug.Log("001!!");
        new StartWorkDayEvent(120);
    }

    void GenerateDayEvents()
    {

        lastDayDebug = "";

        int seed = UnityEngine.Random.Range(-999999, 999999);
        var rand = new System.Random(seed);

        lastDayDebug += $"{seed} - day seed\n";
        int time = 0;
        for (int _time = 150; _time < 600; _time+=150)
        {
            time = rand.Next(_time, _time + 120);
            lastDayDebug += $"{time} - hire event\n";
            new HireEvent(time);
            time = rand.Next(_time, _time + 120);
            lastDayDebug += $"{time} - dialog event\n";
            new DialogEvent(time);
        }

        time = rand.Next(300, 420);
        var eve = GenerateGlobalEvent(time);

        if (eve != null)
        {
            lastDayDebug += $"{time} - global event({eve.ToString()})\n";
        }

        var joilers = PersonManager.Instance.persons.Where(i => i.GetModificator<TeamMember>().isActive & i.GetModificator<JoillerMod>() != null).ToList();
        if(joilers.Count > 0)
        {
            lastDayDebug += $"{joilers.Count} - count of joilers";
            var dice = rand.Next(1, 101);
            var result = dice <= JoillerMod.dialogChance;
            lastDayDebug += $"additional dialog: dice={dice} chance={JoillerMod.dialogChance} result={result}\n";
            if (result)
            {
                if (rand.Next(1, 101) >= 50)
                {
                    new DialogEvent(rand.Next(240, 270));
                }
                else
                {
                    new DialogEvent(rand.Next(420, 450));
                }
            }
        }
    }

    public string DebugInfo()
    {
        var ret = "";

        ret += "=====Event Manager===== \n\n===last day=== \n" + lastDayDebug +"\n";

        foreach (var e in globalEvents)
        {
            ret += e.DebugInfo() ;
            ret += "\n";
        }

        return ret;
    }

    GameEvent GenerateGlobalEvent(int time)
    {
        foreach (GlobalEvent e in globalEvents)
        {
            var eve = e.CreateEvent(time);
            if (eve != null)
            {
                return eve;
            }
        }
        return null;
    }

    void InitGame()
    {
        for (int i = 0; i < 3; i++)
        {
            var person = PersonGenerator.Instance.CreateNewPerson();
            person.transform.position = WorldPoint.GetPoint(PointType.Rest).pos;
            PersonManager.Instance.RegisterPerson(person);
        }
    }
}


public enum EventStates
{
    Free,
    Common,
    GlobalPreparing,
    Global,
}