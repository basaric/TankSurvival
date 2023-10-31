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
    public float cameraShakeDuration = 0.15f;
    public float cameraShakeMagnitude = 1f;
    public float cameraShakeFadeinTime = 0.2f;
    public float cameraShakeFadeoutTime = 0.8f;
    private Coroutine shakeCoroutine;

    private Vector3 movementInput = Vector3.zero;
    private Camera cameraMain;
    private TankMovement tankMovement;
    private TankWeapon tankWeapon;

    private void Awake() {
        cameraMain = Camera.main;
        tankMovement = gameObject.GetComponent<TankMovement>();
        tankWeapon = gameObject.GetComponent<TankWeapon>();
        Cursor.visible = false;
    }
    void Update() {
        movementInput.x = Input.GetAxis(horizontalAxisName);
        movementInput.y = Input.GetAxis(verticalAxisName);

        if (Input.GetMouseButtonDown(0)) {
            tankWeapon.Fire();
        }
        if (Input.GetMouseButtonDown(1)) {
            //GetComponent<TankHealth>().kill();
            if (shakeCoroutine != null) {
                StopCoroutine(shakeCoroutine);
            }
            shakeCoroutine = StartCoroutine(Shake(cameraShakeDuration, cameraShakeMagnitude));
        }

        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            tankMovement.aimAt(hit.point);
        }
    }
    private void FixedUpdate() {
        Vector3 moveInput = cameraMain.transform.forward * movementInput.x + cameraMain.transform.right * movementInput.y;
        moveInput.y = 0;
        tankMovement.onMoveInput(moveInput.normalized);
    }
    public IEnumerator Shake(float duration, float magnitude) {
        Vector3 startPosition = cameraMain.transform.localPosition;
        //Vector3 shakePosition = cameraMain.transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration) {
            Vector3 randomPosition = new Vector3(
                Random.Range(-magnitude, magnitude),
                Random.Range(-magnitude, magnitude),
                startPosition.z
            );
            //shakePosition += randomPosition;
            //cameraMain.transform.localPosition = Vector3.Lerp(cameraMain.transform.localPosition, shakePosition, cameraShakeLerpStrength);
            float progress = elapsed / duration;
            float fadeIn = Utils.remapAndClamp(progress, 0f, cameraShakeFadeinTime, 0f, 1f);
            float fadeOut = Utils.remapAndClamp(progress, cameraShakeFadeoutTime, 1f, 1f, 0f);
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
