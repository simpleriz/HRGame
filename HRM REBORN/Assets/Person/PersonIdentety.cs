using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonIdentety : MonoBehaviour
{
    [TextArea(10, 100), SerializeField] string debug;
    public List<PersonModificator> modificators { get; private set; } = new List<PersonModificator>();
    public PersonTransform personTransform;

    private void Start()
    {
        personTransform = GetComponent<PersonTransform>();
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

    public void AddModificator(PersonModificator _mod)
    {
        _mod.identety = this;
        modificators.Add(_mod);
    }

    private void FixedUpdate()
    {
        debug = "";
        foreach (var mod in modificators)
        {
            debug += mod.DebugInfo();
            debug += "\n\n";
        }
        
    }
}
