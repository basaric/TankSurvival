using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankNavigation : MonoBehaviour
{
    public Camera playerCamera;
    public NavMeshAgent agent;

    void Update()
    {
        if (Input.GetMouseButton(0) ) {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                agent.isStopped = false;
                agent.SetDestination(hit.point);
            }
        } else {
            agent.isStopped = true;
        }
    }
}
