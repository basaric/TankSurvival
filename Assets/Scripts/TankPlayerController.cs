using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    [Header("Input")]
    public string horizontalAxisName = "Vertical1";
    public string verticalAxisName = "Horizontal1";
    
    [Header("Camera Shake")]
    public float shakeDuration = 0.15f;
    public float shakeMagnitude = 1f;
    public float shakeFadeInDuration = 0.2f;
    public float shakeFadeOutDuration = 0.2f;
    private Coroutine shakeCoroutine;

    private Vector3 movementInput = Vector3.zero;
    private Camera cameraMain;
    private TankMovement tankMovement;
    private WeaponManager weaponManager;

    private void Awake() {
        cameraMain = Camera.main;
        tankMovement = gameObject.GetComponent<TankMovement>();
        weaponManager = gameObject.GetComponent<WeaponManager>();
        Cursor.visible = false;
    }
    void Update() {
        movementInput.x = Input.GetAxis(horizontalAxisName);
        movementInput.y = Input.GetAxis(verticalAxisName);

        pointerAim();

        if (Input.GetMouseButtonDown(0)) {
            weaponManager.triggerOn();
        }
        if (Input.GetMouseButtonUp(0)) {
            weaponManager.triggerOff();
        }

        if (Input.mouseScrollDelta.y != 0f) {
            if (Input.mouseScrollDelta.y > 0f) {
                weaponManager.equipNext();
            } else {
                weaponManager.equipPrevious();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            cameraShake(shakeDuration, shakeMagnitude, shakeFadeInDuration, shakeFadeOutDuration);
        }
    }
    private void FixedUpdate() {
        Vector3 moveInput = cameraMain.transform.forward * movementInput.x + cameraMain.transform.right * movementInput.y;
        moveInput.y = 0;
        if (tankMovement != null) {
            tankMovement.onMoveInput(moveInput.normalized);
        }
    }
    private void pointerAim() {
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (tankMovement != null) {
                tankMovement.aimAt(hit.point);
            } else {
                Utils.lookAtPoint(transform, hit.point);
            }
        }
    }
    public void cameraShake(float duration, float magnitude, float fadeinTime, float fadeoutTime) {
        if (shakeCoroutine != null) {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude, fadeinTime, fadeoutTime));
    }
    public IEnumerator Shake(float duration, float magnitude, float fadeinTime, float fadeoutTime) {
        Vector3 startPosition = cameraMain.transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration) {
            Vector3 randomPosition = new Vector3(
                Random.Range(-magnitude, magnitude),
                Random.Range(-magnitude, magnitude),
                startPosition.z
            );
            float progress = elapsed / duration;
            float fadeIn = Utils.remapAndClamp(progress, 0f, fadeinTime, 0f, 1f);
            float fadeOut = Utils.remapAndClamp(progress, (1 - fadeoutTime), 1f, 1f, 0f);
            randomPosition = randomPosition * fadeIn * fadeOut;
            randomPosition.z = startPosition.z;
            cameraMain.transform.localPosition = randomPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraMain.transform.localPosition = startPosition;
        shakeCoroutine = null;
    }
}
