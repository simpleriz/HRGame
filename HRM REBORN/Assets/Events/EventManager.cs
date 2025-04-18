using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    static public EventManager Instance;

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
        var person1 = PersonGenerator.Instance.CreateNewPerson();
        person1.transform.position = WorldPoint.GetPoint(PointType.Rest).pos;
        PersonManager.Instance.RegisterPerson(person1);

        var person2 = PersonGenerator.Instance.CreateNewPerson();
        person2.transform.position = WorldPoint.GetPoint(PointType.Rest).pos;
        PersonManager.Instance.RegisterPerson(person2);

        person1.personTransform.DialogTask(person2.personTransform);

        for (int i = 0; i < 4; i++)
        {
            var person = PersonGenerator.Instance.CreateNewPerson();
            person.transform.position = WorldPoint.GetPoint(PointType.Rest).pos;
            PersonManager.Instance.RegisterPerson(person);
        }
    }

    public void GenerateDayEvents()
    {
        new StartWorkDayEvent(30);
    }
}
