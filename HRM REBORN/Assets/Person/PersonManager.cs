using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PersonManager : MonoBehaviour
{
    List<PersonIdentety> persons = new List<PersonIdentety>();
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
    
    public void RegisterNewPerson(PersonIdentety _person)
    {
        persons.Add(_person);
    }
}
