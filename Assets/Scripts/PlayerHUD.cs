using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private Camera cameraHUD;
    private Image crosshair;

    public Text missionName, missionTime, missionDescription, missionTxt, missionStatus;
    private string stringOnSuccessText = "Mission <color=#00ff00>success!</color>";
    private string stringOnFailureText = "Mission <color=#ff0000>failed!</color>";

    private void Awake() {
        crosshair = transform.Find("Crosshair").GetComponent<Image>();
        cameraHUD = GameObject.FindWithTag("hudCamera").GetComponent<Camera>();

        stopMission();
        hideMissionStatus();
    }

    private void Update() {
        crosshair.transform.position = cameraHUD.ScreenToWorldPoint(Input.mousePosition);
    }
    public void startMission(Mission mission) {
        missionName.enabled = true;
        missionTime.enabled = true;
        missionDescription.enabled = true;
        missionTxt.enabled = true;

        missionName.text = mission.missionName;
        missionDescription.text = mission.missionDescription;
        missionTime.text = mission.missionDuration.ToString();
    }
    public void stopMission() {
        missionName.enabled = false;
        missionTime.enabled = false;
        missionDescription.enabled = false;
        missionTxt.enabled = false;
    }
    public void refreshMissionTime(int time) {
        missionTime.text = time.ToString();
    }
    public void showMissionStatus(bool isSuccess) {
        missionStatus.enabled = true;
        missionStatus.text = isSuccess ? stringOnSuccessText : stringOnFailureText;
        Invoke("hideMissionStatus", 5f);
    }
    public void hideMissionStatus() {
        missionStatus.enabled = false;
    }
}
