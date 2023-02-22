using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    public GameObject UIRacePanel;
    public Text UITextCurrentLap;
    public Text UITextCurrentTime;
    public Text UITexLastLapTime;
    public Text UITextBestLapTime;

    public LapManager UpdateUIForPlayer;

    private int currentLap;
    private float currentLapTime;
    private float lastLapTime;
    private float bestLapTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateUIForPlayer == null)
        {
            return;
        }
        if(UpdateUIForPlayer.CurrentLap != currentLap)
        {
            currentLap = UpdateUIForPlayer.CurrentLap;
            UITextCurrentLap.text = $"LAP: {currentLap}";
        }
        if(UpdateUIForPlayer.CurrentLapTime != currentLapTime)
        {
            currentLapTime = UpdateUIForPlayer.CurrentLapTime;
            UITextCurrentTime.text = $"TIME: {(int)currentLapTime / 60} : {(currentLapTime) % 60:00.000}";
        }
        if (UpdateUIForPlayer.LastLapTime != lastLapTime)
        {
            lastLapTime = UpdateUIForPlayer.LastLapTime;
            UITexLastLapTime.text = $"LAST: {(int)lastLapTime / 60} : {(lastLapTime) % 60:00.000}";
        }
        if (UpdateUIForPlayer.BestLapTime != bestLapTime)
        {
            bestLapTime = UpdateUIForPlayer.BestLapTime;
            UITextBestLapTime.text = bestLapTime  < 1000000 ? $"BEST: {(int)bestLapTime / 60} : {(bestLapTime) % 60:00.000}" : "BEST: NONE ";
        }

    }
}
