using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

public class COMP_Collectible : MonoBehaviour {
    public float lerpStrength = 0.1f;
    public float lerpStrengthColor = 0.5f;
    public Color targetColor = Color.white;
    public float rotationSpeed = 100f;
    public float rotationSpeedVariationRange = 0.3f;

    private GameObject picker;
    private bool isCollected = false;

    private void Start() {
        rotationSpeed = rotationSpeed * (1 + Random.value * (2 * rotationSpeedVariationRange) - rotationSpeedVariationRange);
    }
    private void Update() {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        if (isCollected) {
            Renderer renderer = gameObject.GetComponentInChildren<Renderer>();

            if (Vector3.Magnitude(picker.transform.position - gameObject.transform.position) > 2f) {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, picker.transform.position, lerpStrength);
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, lerpStrengthColor);
            } else {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (!isCollected && other.CompareTag("Player")) {
            gameObject.GetComponent<AudioSource>().Play();
            picker = other.gameObject;
            isCollected = true;
        }
    }
}