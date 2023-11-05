using Complete;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;


public class TankEnemyController : MonoBehaviour
{
    [Header("Navigation")]
    [Tooltip("Choose between the default nav agent movement logic or the custom one")]
    public bool agentCustomMovement = true;
    [Range(0.5f, 10f)]
    [Tooltip("Different navigation speeds for different movement modes")]
    public float navigationSpeed = 0.5f;
    
    protected NavMeshAgent agent;
    protected GameObject player;
    protected TankMovement tankMovement;
    protected TankWeapon tankWeapon;

#if UNITY_EDITOR
    void OnValidate() {
        if (agent != null) {
            refreshAgent();
        }
    }
#endif
    private void refreshAgent() {
        agent.updatePosition = !agentCustomMovement;
        agent.updateRotation = !agentCustomMovement;
        agent.speed = navigationSpeed;
    }

    protected virtual void Awake() {
        agent = GetComponent<NavMeshAgent>();
        tankMovement = GetComponent<TankMovement>();
        tankWeapon = GetComponentInChildren<TankWeapon>();
        
        refreshAgent();
    }
    protected virtual void Start() {
        player = GameObject.FindWithTag("Player");
    }
    protected virtual void FixedUpdate() {
        if (!agent.isStopped && agentCustomMovement && agent.isOnNavMesh) {
            agent.nextPosition = transform.position;
            tankMovement.onMoveInput(agent.desiredVelocity);
        }
        aim();
    }
    protected void aim() {
        if (player != null) {
            tankMovement.aimAt(player.transform.position);
        }
    }
    protected void setDestination(Vector3 position) {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(position, out navHit, 1.0f, NavMesh.AllAreas)) {
            agent.isStopped = false;
            agent.SetDestination(navHit.position);
        }
    }
}
