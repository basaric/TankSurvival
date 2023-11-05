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
    public float infiniteShakeFadeInDuration = 0f;
    private Coroutine shakeCoroutine;
    private Vector3 shakeStartPosition;

    private PlayerHUD hud;
    private Vector3 movementInput = Vector3.zero;
    private Camera cameraMain;
    private TankMovement tankMovement;
    private WeaponManager weaponManager;

    private GroundTrigger _overlapingTrigger;
    private Vector3 tooltipPosition;

    public GroundTrigger overlapingTrigger {
        get {
            return _overlapingTrigger;
        }
        set {
            _overlapingTrigger = value;
        }
    }

    private void Awake() {
        cameraMain = Camera.main;
        tankMovement = gameObject.GetComponent<TankMovement>();
        weaponManager = gameObject.GetComponent<WeaponManager>();
        hud = GameObject.FindWithTag("HUD").GetComponent<PlayerHUD>();
        Cursor.visible = false;
    }
    void Update() {
        if (_overlapingTrigger != null) {
            hud.showTooltip(tooltipPosition, _overlapingTrigger.tooltip);
        }
        else {
            hud.hideTooltip();
        }

        movementInput.x = Input.GetAxis(horizontalAxisName);
        movementInput.y = Input.GetAxis(verticalAxisName);

        pointerAim();

        if (Input.GetMouseButtonDown(0)) {
            weaponManager.triggerOn();
            cameraShakeInfinite(0.3f, infiniteShakeFadeInDuration);
        }
        if (Input.GetMouseButtonUp(0)) {
            weaponManager.triggerOff();
            stopShaking();
        }

        if (Input.mouseScrollDelta.y != 0f) {
            if (Input.mouseScrollDelta.y > 0f) {
                weaponManager.equipNext();
            } else {
                weaponManager.equipPrevious();
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            //cameraShake(shakeDuration, shakeMagnitude, shakeFadeInDuration, shakeFadeOutDuration);
        }

        if (Input.GetButtonDown("Interact")) {
            if (_overlapingTrigger != null) {
                _overlapingTrigger.trigger();
            }
        }
    }
    private void FixedUpdate() {
        Vector3 moveInput = cameraMain.transform.forward * movementInput.x + cameraMain.transform.right * movementInput.y;
        moveInput.y = 0;
        if (tankMovement != null) {
            tankMovement.onMoveInput(moveInput.normalized);
        }
        if (_overlapingTrigger != null) {
            tooltipPosition = cameraMain.WorldToScreenPoint(_overlapingTrigger.transform.position);
        }
        
    }
    private void pointerAim() {
        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Ground"))) {
            if (tankMovement != null) {
                tankMovement.aimAt(hit.point);
            } else {
                Utils.lookAtPoint(transform, hit.point);
            }
        }
    }
    public void cameraShake(float duration, float magnitude, float fadeinTime, float fadeoutTime) {
        stopShaking();
        shakeStartPosition = cameraMain.transform.localPosition;
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude, fadeinTime, fadeoutTime));
    }
    public IEnumerator Shake(float duration = 0.1f, float magnitude = 0.5f, float fadeinTime = 0.5f, float fadeoutTime = 0.5f) {
        float elapsed = 0f;
        while (elapsed < duration) {
            Vector3 randomPosition = new Vector3(
                Random.Range(-magnitude, magnitude),
                Random.Range(-magnitude, magnitude),
                shakeStartPosition.z
            );
            float progress = elapsed / duration;
            float fadeIn = Utils.remapAndClamp(progress, 0f, fadeinTime, 0f, 1f);
            float fadeOut = Utils.remapAndClamp(progress, (1 - fadeoutTime), 1f, 1f, 0f);
            randomPosition = randomPosition * fadeIn * fadeOut;
            randomPosition.z = shakeStartPosition.z;
            cameraMain.transform.localPosition = randomPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraMain.transform.localPosition = shakeStartPosition;
        shakeCoroutine = null;
    }
    public void cameraShakeInfinite(float magnitude, float fadeinTime) {
        shakeStartPosition = cameraMain.transform.localPosition;
        stopShaking();
        shakeCoroutine = StartCoroutine(ShakeInfinite(magnitude, fadeinTime));
    }
    public void stopShaking() {
        if (shakeCoroutine != null) {
            StopCoroutine(shakeCoroutine);
        }
        cameraMain.transform.localPosition = shakeStartPosition;
        shakeCoroutine = null;
    }
    public IEnumerator ShakeInfinite(float magnitude = 0.5f, float fadeinTime = 0.5f) {
        float elapsed = 0f;
        while (true) {
            Vector3 randomPosition = new Vector3(
                Random.Range(-magnitude, magnitude),
                Random.Range(-magnitude, magnitude),
                shakeStartPosition.z
            );
            float progress = elapsed / fadeinTime;
            float fadeIn = Utils.remapAndClamp(progress, 0f, fadeinTime, 0f, 1f);
            randomPosition = randomPosition * fadeIn;
            randomPosition.z = shakeStartPosition.z;
            cameraMain.transform.localPosition = randomPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
