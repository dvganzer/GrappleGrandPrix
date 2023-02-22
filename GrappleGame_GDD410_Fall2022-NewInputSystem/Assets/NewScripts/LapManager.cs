using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LapManager : MonoBehaviour
{
    public float BestLapTime { get; private set; } = Mathf.Infinity;
    public float LastLapTime { get; private set; } = 0;
    public float CurrentLapTime { get; private set; } = 0;
    public int CurrentLap { get; private set; } = 0;

    public float lapTimerTimeStamp;
    public int lastCheckpointPassed = 0;

    public Transform checkpointsParent;
    public int checkpointCount;
    public int checkpointLayer;
    public NewMovement playerController;

    public GameObject pressStart;

    void Awake()
    {
        checkpointsParent = GameObject.Find("Checkpoints").transform;
        checkpointCount = checkpointsParent.childCount;
        Debug.Log(checkpointCount);
        checkpointLayer = LayerMask.NameToLayer("Checkpoint");
        playerController = GetComponent<NewMovement>();
        pressStart.SetActive(false);
    }

    void StartLap()
    {
        Debug.Log("StartLap!");
        CurrentLap++;
        lastCheckpointPassed = 1;
        lapTimerTimeStamp = Time.time;
        Debug.Log(CurrentLap);
    }
    void EndLap()
    {
        LastLapTime = Time.time - lapTimerTimeStamp;
        BestLapTime = Mathf.Min(LastLapTime, BestLapTime);
        Debug.Log("EndLap - LapTime was " + LastLapTime + "seconds");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.layer != checkpointLayer)
        {
            return;
        }

        if(collider.gameObject.name == "1")
        {
            if(lastCheckpointPassed == checkpointCount)
            {
                EndLap();
            }
            if(CurrentLap == 0 || lastCheckpointPassed == checkpointCount)
            {
                StartLap();
            }
            return;
        }

        if(collider.gameObject.name == (lastCheckpointPassed + 1).ToString())
        {
            lastCheckpointPassed++;
        }

    }
    void Update()
    {
        CurrentLapTime = lapTimerTimeStamp > 0 ? Time.time - lapTimerTimeStamp : 0;

        if(CurrentLap == 4)
        {
            SceneManager.LoadScene(0);
            pressStart.SetActive(true);
        }
    }
}
