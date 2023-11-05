using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundTrigger : MonoBehaviour
{
    public string tooltip;
    public UnityEvent onTriggered;
    private bool isTriggered = false;
    private TankPlayerController playerController;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            playerController = other.gameObject.GetComponent<TankPlayerController>();
            playerController.overlapingTrigger = this;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<TankPlayerController>().overlapingTrigger = null;
            playerController = null;
        }
    }
    public void trigger() {
        if (!isTriggered) {
            playerController.overlapingTrigger = null;
            gameObject.SetActive(false);
            onTriggered?.Invoke();
        }
    }
}
