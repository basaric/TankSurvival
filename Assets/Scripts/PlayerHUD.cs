using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private Camera cameraHUD;
    private Image crosshair; 

    private void Awake() {
        crosshair = transform.Find("Crosshair").GetComponent<Image>();
        cameraHUD = GameObject.FindWithTag("hudCamera").GetComponent<Camera>();
    }

    private void Update() {
        crosshair.transform.position = cameraHUD.ScreenToWorldPoint(Input.mousePosition);
    }
}
