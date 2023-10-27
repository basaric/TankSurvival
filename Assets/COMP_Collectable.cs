using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class COMP_Collectible : MonoBehaviour {
    private float colorChangeDuration = 0.3f;
    public float destroyXDelay = 0.3f;
    public Color targetColor = Color.white;
    public float rotationSpeed = 100f;
    public float rotationSpeedVariationRange = 0.3f;
    private void Start() {
        rotationSpeed = rotationSpeed * (1 + Random.value * (2 * rotationSpeedVariationRange) - rotationSpeedVariationRange);
    }
    private void Update() {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(ChangeColorAndDestroy());
        }
    }
    private IEnumerator ChangeColorAndDestroy() {
        float elapsedTime = 0f;
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        Transform childTransform = gameObject.transform.GetChild(0).transform;

        while (elapsedTime < colorChangeDuration) {
            childTransform.position = new Vector3(childTransform.position.x, childTransform.position.y + 0.05f, childTransform.position.z);
            float t = elapsedTime / colorChangeDuration;
            renderer.material.color = Color.Lerp(renderer.material.color, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}