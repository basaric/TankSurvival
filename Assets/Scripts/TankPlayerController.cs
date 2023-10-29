using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankPlayerController : MonoBehaviour
{
    public string horizontalAxisName = "Vertical1";
    public string verticalAxisName = "Horizontal1";
    public Image crosshair;

    private Vector3 movementInput = Vector3.zero;
    private Camera cameraMain;
    private Camera cameraHUD;

    private TankMovement tankMovement;
    private TankShooting tankShooting;

    private void Awake() {
        cameraMain = Camera.main;
        cameraHUD = GameObject.FindWithTag("hudCamera").GetComponent<Camera>();
        tankMovement = gameObject.GetComponent<TankMovement>();
        tankShooting = gameObject.GetComponent<TankShooting>();
    }

    void Update() {
        movementInput.x = Input.GetAxis(horizontalAxisName);
        movementInput.y = Input.GetAxis(verticalAxisName);

        Vector3 crosshairPosition = Input.mousePosition;
        crosshair.transform.position = cameraHUD.ScreenToWorldPoint(crosshairPosition);
        Cursor.visible = false;

        if (Input.GetMouseButtonDown(0)) {
            tankShooting.Fire();
        }
    }

    private void FixedUpdate() {
        Vector3 moveInput = cameraMain.transform.forward * movementInput.x + cameraMain.transform.right * movementInput.y;
        moveInput.y = 0;
        tankMovement.onMoveInput(moveInput);

        Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            tankMovement.aimAt(hit.point);
        }
    }
}
