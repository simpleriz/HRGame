using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class PersonIdentety : MonoBehaviour
{
    [TextArea(10, 100), SerializeField] string debug;
    public List<PersonModificator> modificators { get; private set; } = new List<PersonModificator>();
    public PersonTransform personTransform;

    List<string> debugNotes = new List<string>();

    //Events
    public UnityEvent FireEvent = new UnityEvent();
    public UnityEvent DeathEvent = new UnityEvent();

    private void Start()
    {
        if(personTransform == null)
        {
            personTransform = GetComponent<PersonTransform>();
        }
        var stats = GetModificator<BasicStats>();
        if (stats != null)
        {
            transform.name = $"person({stats.name})";
        }
    }

    public void Fire()
    {
        foreach (var mod in modificators) 
        { 
            mod.OnFire();
        }
        FireEvent.Invoke();
        DestroyPerson();
    }

    public void Death()
    {
        foreach (var mod in modificators)
        {
            mod.OnDeath();
        }
        DeathEvent.Invoke();
        DestroyPerson();
    }

    public void DestroyPerson()
    {
        Destroy(gameObject);
    }

    public void AddDebugNote(string note)
    {
        debugNotes.Add(note);
        if (debugNotes.Count > 10)
        {
            debugNotes.RemoveAt(0);
        }
    }
    public T GetModificator<T>() where T : PersonModificator
    {
        foreach (var modificator in modificators)
        {
            if (modificator is T matched)
                return matched;
        }
        return null;
    }

    public List<T> GetAllModificator<T>() where T : PersonModificator
    {
        List<T> list = new List<T>();
        foreach (var modificator in modificators)
        {
            if (modificator is T matched)
                list.Add(matched);
        }
        return list;
    }

    public bool Is—apacity()
    {
        foreach(var mod in modificators)
        {
            if (!(mod.IsCapacity()))
            {
                return false;
            }
        }
        return true;
    }

    public float CalculateConflictChance(PersonIdentety person)
    {
        float chance = 0;

        foreach (var mod in modificators)
        {
            chance += mod.CalculateConflictChance(person);
        }

        return chance;
    }

    public float CalculateCoupleChance(PersonIdentety person)
    {
        float chance = 0;

        foreach (var mod in modificators)
        {
            chance += mod.CalculateCoupleChance(person);
        }

        return chance;
    }

    public float CalculateWorkEffencity()
    {
        float effencity = 0;

        foreach (var mod in modificators)
        {
            effencity += mod.CalculateWorkEffencity();
        }

        return effencity;
    }

    public int CalculateMaxEnergy()
    {
        int energy = 0;

        foreach (var mod in modificators)
        {
            energy += mod.CalculateMaxEnergy();
        }

        return energy;
    }
    public void AddModificator(PersonModificator _mod)
    {
        _mod.identety = this;
        modificators.Add(_mod);
    }

    private void FixedUpdate()
    {
        debug = "=====MODIFICATORS=====\n\n";
        foreach (var mod in modificators)
        {
            debug += mod.DebugInfo();
            debug += "\n\n";
        }

        debug += "\n=====NOTES=====\n\n";

        foreach (var note in debugNotes)
        {
            debug += note;
            debug += "\n\n";
        }
    }
}
