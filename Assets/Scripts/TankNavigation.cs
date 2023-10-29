using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankNavigation : MonoBehaviour
{
    public Camera playerCamera;
    public NavMeshAgent agent;
    private GameObject player;

    private void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(player.transform.position, out navHit, 1.0f, NavMesh.AllAreas)) {
            Vector3 nearestPoint = navHit.position;
            agent.isStopped = false;
            agent.SetDestination(nearestPoint);
        }
    }
}
