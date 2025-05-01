using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    private void Start()
    {
        TimeManager.Instance.OnMinute.AddListener(UpdateTimer);
    }

    void UpdateTimer()
    {
        var _m = TimeManager.Instance.actuallyMinute;
        string hour = (Mathf.FloorToInt(_m / 60) + 8).ToString();
        string min = (_m % 60).ToString();
        if (hour.Length == 1)
        {
            hour = "0" + hour;
        }
        if (min.Length == 1)
        {
            min = "0" + min;
        }
        text.text = $"{hour}:{min}";
    }
}
