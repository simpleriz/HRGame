using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    static public EventManager Instance;
    public EventStates state = EventStates.Free;
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

    public void GenerateDayEvents()
    {
        new StartWorkDayEvent(120);

    }
}


public enum EventStates
{
    Free,
    Global,
}