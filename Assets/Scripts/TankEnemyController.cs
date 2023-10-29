using Complete;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemyController : MonoBehaviour
{
    public float shootInterval = 5f;

    private NavMeshAgent agent;
    private GameObject player;
    private TankMovement tankMovement;
    private TankShooting tankShooting;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        tankMovement = GetComponent<TankMovement>();
        tankShooting = GetComponent<TankShooting>();
    }

    private void Start() {
        player = GameObject.FindWithTag("Player");
        //InvokeRepeating("shootTimer", 0.0f, shootInterval);
    }
    void Update() {
        //goToPosition(player.transform.position);
        tankMovement.aimAt(player.transform.position);
    }
    private void goToPosition(Vector3 position) {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(position, out navHit, 1.0f, NavMesh.AllAreas)) {
            Vector3 nearestPoint = navHit.position;
            agent.isStopped = false;
            agent.SetDestination(nearestPoint);
        }
    }
    private void shootTimer() {
        tankShooting.Fire();
    }
}
