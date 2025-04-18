using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{

    public int actuallyMinute;
    public UnityEvent OnMinute = new UnityEvent();
    public const float minuteDuration = 1;
    static public TimeManager Instance;

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
        while (true)
        {
            yield return new WaitForSeconds(minuteDuration);
            actuallyMinute++;
            OnMinute.Invoke();
        }

    }
}
