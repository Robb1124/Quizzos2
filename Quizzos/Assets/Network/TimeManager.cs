using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class TimeManager : MonoBehaviour
{
    string currentDate;
    string currentTime;
    string dailyAdsCompletionDate;
    const string defaultDate = "10-18-2019";

    public string DailyAdsCompletionDate { get => dailyAdsCompletionDate; set => dailyAdsCompletionDate = value; }

    private void Start()
    {
        StartCoroutine(GetText());
    }

    public void SendTimeAndDateRequest()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://quizzos.000webhostapp.com/timeanddate.php");
        yield return webRequest.SendWebRequest();

        if (webRequest.isNetworkError || webRequest.isHttpError)
        {
            Debug.Log(webRequest.error);
        }
        else
        {
            // Show results as text
            string timeAndDate = webRequest.downloadHandler.text;
            string[] words = timeAndDate.Split('/');
            currentDate = words[0];
            currentTime = words[1];
        }
    }

    public void SaveCompletionDate()
    {
        DailyAdsCompletionDate = currentDate;
    }

    public bool IsDailyAdsRefreshable()
    {
        DateTime completionDate = (DailyAdsCompletionDate is null) ? DateTime.Parse(defaultDate) : DateTime.Parse(DailyAdsCompletionDate);
        print(completionDate);
        print(currentDate);
        if(completionDate < DateTime.Parse(currentDate))
        {
            //check if its a specific hour ? ie : unlock only at 9PM on next day.
            print("daily ads are refreshable");
            return true;
        }
        else
        {
            print("daily ads are not refreshable");
            return false;
        }
    }
}
