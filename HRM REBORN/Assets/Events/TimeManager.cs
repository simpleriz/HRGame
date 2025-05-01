using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public int day = -1;
    public int actuallyMinute;
    public UnityEvent OnMinute = new UnityEvent();
    public const float minuteDuration = 15/10;
    static public TimeManager Instance;

    //events
    public UnityEvent OnNight = new UnityEvent();
    public UnityEvent OnDay = new UnityEvent();

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
        StartCoroutine(TimeUpdater());
    }

    IEnumerator TimeUpdater()
    {
        yield return null;
        actuallyMinute = 0;
        day++;
        OnDay.Invoke();
        yield return null;
        while (true)
        {
            if (Console.Instance.skipTime > 0)
            {
                yield return new WaitForSeconds(minuteDuration/250);
                Console.Instance.skipTime--;
            }
            else
            {
                yield return new WaitForSeconds(minuteDuration);
            }
            
            actuallyMinute++;
            OnMinute.Invoke();
        }

    }
}
