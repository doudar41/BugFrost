using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    private float StartTime, StopTime, MinuteTime;
    [SerializeField]
    private float HowLongAnHour = 10.0f;

    public int GivingTime;

    private int CurrentMinute = 24;
    [SerializeField]
    private TextMeshProUGUI hoursText, minuteText;



    void Start()
    {
        StartTime = Time.time; 
        StopTime = Time.time;
        MinuteTime = Time.time;
    }

    private void FixedUpdate()
    {
        CountHours();
        hoursText.text = GivingTime.ToString();
        minuteText.text = CurrentMinute.ToString();
    }
    public void CountHours()
    {
        if (GivingTime > 0)
        {
            if (Time.time - StopTime > HowLongAnHour)
            {
                GivingTime--;
                CurrentMinute = 24;
                StopTime = Time.time;
            }
            if (Time.time - MinuteTime > HowLongAnHour / 25)
            {
                CurrentMinute--;
                MinuteTime = Time.time;
            }
        }
        else { GivingTime = 0; CurrentMinute = 0; }
    }

}
