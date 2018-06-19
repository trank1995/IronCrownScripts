using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: save and load time
public class TimeSystem : MonoBehaviour {
    public Text currentTime;
    public GameObject sun, moon, mapCenter;
    float timer;
    public TimeSave timeSave;
    public static int month, day, hour, minute;
    public static string season, monthString, dayString, hourString, minuteString;
    int[] daysOfMonths = new int[12] { 31, 28, 31, 30, 31, 30, 31, 31, 30,
    31, 30, 31};
    string[] monthNames = new string[12] { "January", "February", "March", "April",
        "May", "June", "July", "August", "September", "October", "November", "December" };

    Vector3 playerVelocity, oneFrameAgo;
    Vector3 zeroVector = new Vector3(0, 0, 0);
    public GameObject player;
    public static bool pause;
    

    // Use this for initialization
    void Awake () {
        //TODO: read from save
        timeSave = new TimeSave(1, 1, 0, 0);
        season = "winter";
        month = timeSave.month;
        day = timeSave.day;
        hour = timeSave.hour;
        minute = timeSave.minute;
        timer = 0.0f;
        pause = false;
        moon.SetActive(false);
    }

    private void Update()
    {
        if (!pause)
        {
            //sun.transform.RotateAround(mapCenter.transform.position, Vector3.right, 10f * Time.deltaTime);
            sun.transform.LookAt(mapCenter.transform);
        }
        
        //moon.transform.RotateAround(mapCenter.transform.position, Vector3.right, 10f * Time.deltaTime);
        //moon.transform.LookAt(mapCenter.transform);
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!pause)
        {
            timer += Time.deltaTime;
            if (timer >= 3.0f)
            {
                updateTime();
                timer -= 3.0f;
            }
        }
        if (player != null)
        {
            playerVelocity = player.transform.position - oneFrameAgo;
            oneFrameAgo = player.transform.position;
            if (playerVelocity == zeroVector)
            {
                pause = true;
            }
            else
            {
                pause = false;
            }
        }
        
	}

    void updateTime()
    {
        minute += 60; //change this to change time speed, more -> faster
        if (minute >= 60)
        {
            minute -= 60;
            hour++;
            if (hour == 24)
            {
                hour = 0;
                day++;
                if (day == daysOfMonths[month])
                {
                    day = 1;
                    month++;
                    if (month == 13)
                    {
                        month = 1;
                    }
                }
            }
        }
        if (month == 12 || month == 1 || month == 2)
        {
            season = "Winter";
        } else if (month == 3 || month == 4 || month == 5)
        {
            season = "Spring";
        } else if (month == 6 || month == 7 || month == 8)
        {
            season = "Summer";
        } else if (month == 9 || month == 10 || month == 11)
        {
            season = "Fall";
        }
        monthString = monthNames[month];
        if (day < 10) {
            dayString = "0" + day.ToString();
        } else
        {
            dayString = day.ToString();
        }
        if (hour < 10)
        {
            hourString = "0" + hour.ToString();
        } else
        {
            hourString = hour.ToString();
        }
        if (minute < 10)
        {
            minuteString = "0" + minute.ToString();
        } else
        {
            minuteString = minute.ToString();
        }
        //if (!pause)
        //{
        //    currentTime.text = season + "\t" + monthString + "\t" + dayString
        //    + "\t" + hourString + ":" + minuteString;
        //} else
        //{
        //    currentTime.text = "Pause";
        //}
        
    }
    public static string getTimeDisplay()
    {
        return season + " " + monthString + "/" + dayString + "th " + hourString + ":00";
    }
    
}

public class TimeSave
{
    public int month, day, hour, minute;
    public TimeSave(int monthI, int dayI, int hourI, int minuteI)
    {
        month = monthI;
        day = dayI;
        hour = hourI;
        minute = minuteI;
    }
}