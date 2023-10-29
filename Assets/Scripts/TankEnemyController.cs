using Complete;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemyController : MonoBehaviour
{
    public float shootInterval = 3f;
    public float moveInterval = 5f;
    public float radius = 20f;

    private NavMeshAgent agent;
    private GameObject player;
    private TankMovement tankMovement;
    private TankWeapon tankWeapon;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        tankMovement = GetComponent<TankMovement>();
        tankWeapon = GetComponent<TankWeapon>();
    }
    private void Start() {
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("shootTimer", 0.0f, shootInterval);
        InvokeRepeating("moveTimer", 0.0f, shootInterval);
    }
    void Update() {
        if (player != null) {
            tankMovement.aimAt(player.transform.position);
        }
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
        tankWeapon.Fire();
    }
    private void moveTimer() {
        if (player != null) {
            Vector3 offset = UnityEngine.Random.onUnitSphere;
            offset.y = 0;
            offset = offset.normalized * radius;

            goToPosition(player.transform.position + offset);
        }
    }
}
