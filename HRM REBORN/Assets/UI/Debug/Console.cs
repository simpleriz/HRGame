using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using TMPro;
using TreeEditor;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Console : MonoBehaviour
{

    static public Console Instance;
    public int skipTime;

    [SerializeField] TextMeshProUGUI history;
    [SerializeField] TextMeshProUGUI lastCommand;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject window;
    [SerializeField] ScrollRect scroll;
    
    Dictionary<string, Action<string>> commands = new Dictionary<string, Action<string>>();
    List<string> commandsHistory = new List<string>();
    int historyIndex;

    public void AddCommand(string name, Action<string> action)
    {
        commands.Add(name, action);
    }

    public void PrintText(string content)
    {
        history.text += content;
        history.text += "\n";
        history.rectTransform.sizeDelta = new Vector2(
            history.preferredWidth,
            history.preferredHeight - 125   
        );
        lastCommand.text += content;
        lastCommand.text += "\n";
        lastCommand.rectTransform.sizeDelta = new Vector2(
            lastCommand.preferredWidth,
            lastCommand.preferredHeight - 125
        );
        scroll.verticalNormalizedPosition = 1;
    }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        AddCommand("edit", EditPerson);
        AddCommand("info", PersonInfo);
        AddCommand("setname", PersonSetName);
        AddCommand("cls", ClearHistory);
        AddCommand("settask", PersonSetTask);
        AddCommand("list", PersonList);
        AddCommand("activelist", PersonActiveList);
        AddCommand("setactive", PersonSetActive);
        AddCommand("event", EventInvoker);
        AddCommand("new", NewPerson);
        AddCommand("seed", CopySeed);
        AddCommand("skip", TimeSkip);
        PrintText("");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) & window.activeInHierarchy == false)
        {
            window.SetActive(true);
        }
        else if(window.activeInHierarchy == false)
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            if(inputField.text == "")
            {
                PrintText("\n");
                return;
            }
            lastCommand.text = string.Empty;
            PrintText(inputField.text);
            var command = inputField.text.Split(' ')[0].ToLower();
            commandsHistory.Insert(0, inputField.text);
            historyIndex = -1;
            
            if (commands.ContainsKey(command)){
                commands[command].Invoke(inputField.text);
            }
            else
            {
                PrintText("Command not found");
            }
            inputField.text = string.Empty;
            inputField.ActivateInputField();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            historyIndex++;
            if (historyIndex >= commandsHistory.Count)
            {
                historyIndex = commandsHistory.Count - 1;
            }
            if(commandsHistory.Count != 0)
            {
                inputField.text = commandsHistory[historyIndex];
                inputField.caretPosition = inputField.text.Length;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            historyIndex--;
            if (historyIndex <= -1 )
            {
                historyIndex = -1;
                inputField.text = string.Empty;
            }
            else if(commandsHistory.Count != 0)
            {
                inputField.text = commandsHistory[historyIndex];
                inputField.caretPosition = inputField.text.Length;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            window.SetActive(false);
        }
    }

    //base commands
    void ClearHistory(string args)
    {
        history.text = string.Empty;
    }

    void TimeSkip(string arg)
    {
        var args = arg.Split(' ').ToList();
        if (args.Count < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }
        int.TryParse(args[1], out skipTime);
    }
    //person edit
    PersonIdentety editable;
    void EditPerson(string arg)
    {
        List<string> args = arg.Split(' ').ToList();
        if (args.Count < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }
        PersonIdentety identety = PersonManager.Instance.persons.Where(i => i.GetModificator<BasicStats>().name.Contains(args[1])).LastOrDefault();
        if (identety != null)
        {
            editable = identety;
            PrintText($"{identety.GetModificator<BasicStats>().name} is the target for editing");
        }
        else
        {
            PrintText($"Person not found");
        }
    }

    void PersonInfo(string arg)
    {
        if (editable == null)
        {
            PrintText("Person isn't set");
            return;
        }
        PrintText(editable.debug);
    }

    void PersonSetName(string arg)
    {
        if (editable == null)
        {
            PrintText("Person isn't set");
            return;
        }
        var args = arg.Split(' ');
        if (args.Length < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }

        editable.GetModificator<BasicStats>().name = args[1];
        PrintText("name reset");
    }

    public void PersonSetTask(string arg)
    {
        if (editable == null)
        {
            PrintText("Person isn't set");
            return;
        }
        var args = arg.Split(' ');
        if (args.Length < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }
        typeof(PersonTransform).GetMethod(args[1])
            ?.Invoke(editable.personTransform, null);
    }

    public void PersonSetActive(string arg)
    {
        var args = arg.Split(' ');
        if (args.Length > 2)
        {
            MassivePersonSetActive(arg);
            return;
        }
        if (editable == null)
        {
            PrintText("Person isn't set");
            return;
        }
        if (args.Length < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }

        var mod = editable.GetModificator<TeamMember>();
        if (args[1] == "+")
        {
            mod.isActive = true;
            if (editable.Is—apacity())
            {
                PrintText("Person set active");
            }
            else
            {
                PrintText("Person set active, but it can't be called according to the rules");
            }
        }
        else if (args[1] == "-")
        {
            mod.isActive = false;
            PrintText("Person set inactive");
            
        }   
        else
        {
            PrintText("Argument is incorrect");
        }
            
    }

    public void CopySeed(string arg)
    {
        if (editable == null)
        {
            PrintText("Person isn't set");
            return;
        }
        GUIUtility.systemCopyBuffer = editable.GetModificator<BasicStats>().seed.ToString();
        PrintText("seed copied");
    }

    //common persons

    public void NewPerson(string arg)
    {
        var args = arg.Split(' ');
        PersonIdentety person;
        if (args.Length == 1)
        {
            person = PersonGenerator.Instance.CreateNewPerson();
            PrintText("A random character was created");
        }
        else if(args.Length == 2) 
        {
            int seed;
            if (args[1] == "+")
            {
                person = PersonGenerator.Instance.CreateNewPerson();
                editable = person;
                PrintText("A random character has been created and is ready for editing");
            }
            else if(int.TryParse(args[1], out seed))
            {
                person = PersonGenerator.Instance.CreateNewPerson(seed);
                PrintText($"A character was created with seed({seed})");
            }
            else
            {
                PrintText("Argument is incorrect");
                return;
            }
        }
        else
        {
            int seed;
            if (args[1] == "+" & int.TryParse(args[2], out seed))
            {
                person = PersonGenerator.Instance.CreateNewPerson(seed);
                editable = person;
                PrintText($"A character has been created with seed({seed}) and is ready for editing");
            }
            else
            {
                PrintText("Argument is incorrect");
                return;
            }
        }
        person.transform.position = WorldPoint.GetPoint(PointType.Rest).pos;
        PersonManager.Instance.RegisterPerson(person);
    }

    public void PersonList(string arg)
    {
        var args = arg.Split(' ');
        if (args.Length == 2)
        {
            switch (args[1])
            {
                case "#full":
                    PrintText("PersonList:");
                    foreach (var stat in PersonManager.Instance.persons.Select(i => i.debug))
                    {
                        PrintText("\n" + stat);
                    }
                    break;
                default:
                    PrintText("Argument is incorrect");
                    return;
            }
            
        }
        else
        {
            PrintText("PersonList:");
            foreach (var stat in PersonManager.Instance.persons.Select(i => i.GetModificator<BasicStats>()))
            {
                PrintText("\n" + stat.DebugInfo());
            }
        }
        
    }

    public void PersonActiveList(string arg)
    {
        var args = arg.Split(' ');
        PrintText("PersonActiveList:");
        foreach (var stat in PersonManager.Instance.persons)
        {
            var name = stat.GetModificator<BasicStats>().name;
            PrintText("\n" + name + new string(' ',15-name.Length) + stat.GetModificator<TeamMember>().isActive);
        }
    }

    public void MassivePersonSetActive(string arg)
    {
        var args = arg.Split(' ');
        if (args.Length < 3)
        {
            PrintText("Argument is incorrect");
            return;
        }
        bool target;
        if (args[1] == "+")
        {
            target = true;
        }
        else if (args[1] == "-")
        {
            target = false;
        }
        else
        {
            PrintText("Argument is incorrect");
            return ;
        }
        if (args[2] == "#all")
        {
            foreach (var _person in PersonManager.Instance.persons)
            {
                _person.GetModificator<TeamMember>().isActive = target;
                var name = _person.GetModificator<BasicStats>().name;
                if (target)
                {
                    if (_person.Is—apacity())
                    {
                        PrintText($"{name} set active");
                    }
                    else
                    {
                        PrintText($"{name} set active, but it can't be called according to the rules");
                    }
                }
                else
                {
                    PrintText($"{name} set inactive");
                }
            }
            return;
        }
        foreach (var name in args[2..])
        {
            var _person = PersonManager.Instance.persons.Where(i => i.GetModificator<BasicStats>().name.Contains(name)).LastOrDefault();
            if (_person != null)
            {
                _person.GetModificator<TeamMember>().isActive = target;
                if (target) {
                    if (_person.Is—apacity())
                    {
                        PrintText($"{name} set active");
                    }
                    else
                    {
                        PrintText($"{name} set active, but it can't be called according to the rules");
                    }
                }
                else
                {
                    PrintText($"{name} set inactive");
                }
            }
            else
            {
                PrintText($"{name} not found!");
            }
        }
    }
    //events
    public void EventInvoker(string arg)
    {
        var args = arg.Split(' ');
        if (args.Length < 2)
        {
            PrintText("Argument is incorrect");
            return;
        }
        string eventName = args[1].ToLower();
        switch (eventName)
        {
            case "debug":
                PrintText(EventManager.Instance.DebugInfo());
                break;
            case "workday":
                new StartWorkDayEvent(TimeManager.Instance.actuallyMinute + 2);
                PrintText("The event will be completed in 2 minutes");
                break;
            default:
                PrintText("Event not found");
                break;
        }
    }
}

