using Complete;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayerController : MonoBehaviour
{
    public string horizontalAxisName = "Vertical1";
    public string verticalAxisName = "Horizontal1";

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
        tankMovement.onMoveInput(moveInput);
    }
}
