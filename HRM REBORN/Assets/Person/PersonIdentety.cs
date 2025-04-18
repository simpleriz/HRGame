using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PersonIdentety : MonoBehaviour
{
    [TextArea(10, 100), SerializeField] string debug;
    public List<PersonModificator> modificators { get; private set; } = new List<PersonModificator>();
    public PersonTransform personTransform;

    List<string> debugNotes = new List<string>();

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
