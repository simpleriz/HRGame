using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PersonManager : MonoBehaviour
{

    //events
    public UnityEvent FireEvent = new();
    public List<PersonIdentety> persons { get; private set; } = new List<PersonIdentety>();
    static public PersonManager Instance;
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
    
    public void RegisterPerson(PersonIdentety _person)
    {
        persons.Add(_person);
        _person.AddModificator(new TeamMember());
    }
}
