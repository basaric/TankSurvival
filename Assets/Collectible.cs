using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Collectible : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //play sound
            Destroy(gameObject);
        }
    }
}