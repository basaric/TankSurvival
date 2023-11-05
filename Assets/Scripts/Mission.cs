using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mission : MonoBehaviour
{
    public int missionDuration = 30;
    public string missionName;
    [TextArea(3, 5)]
    public string missionDescription;
    public UnityEvent onStarted, onFinished;

    private PlayerHUD hud;
    private int elapsed = 0;
    private bool inProgress = false;

    private void OnEnable() {
        hud = GameObject.FindWithTag("HUD").GetComponent<PlayerHUD>();
        startMission();
    }
    void startMission() {
        inProgress = true;
        InvokeRepeating("missionTimer", 0f, 1f);
        hud.startMission(this);
        onStarted?.Invoke();
    }
    private void missionTimer() {
        elapsed += 1;
        if (elapsed > missionDuration) {
            onFinished?.Invoke();
            fail();
        } else {
            hud.refreshMissionTime(missionDuration - elapsed);
        }
    }
    private void stop() {
        inProgress = false;
        CancelInvoke("missionTimer");
        hud.stopMission();
    }
    public void complete() {
        if (inProgress) {
            stop();
            hud.showMissionStatus(true);
        }
    }
    public void fail() {
        if (inProgress) {
            stop();
            hud.showMissionStatus(false);
        }
    }

}
