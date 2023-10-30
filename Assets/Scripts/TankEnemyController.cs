using Complete;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;

public class TankEnemyController : MonoBehaviour
{
    public float shootInterval = 3f;
    public float moveInterval = 5f;
    public float radius = 20f;

    private bool agentUpdatePosition = false;
    private bool agentUpdateRotation = false;
    public bool agentCustomMovement = true;

    private NavMeshAgent agent;
    private GameObject player;
    private TankMovement tankMovement;
    private TankWeapon tankWeapon;

    public bool AgentUpdatePosition {
        get { return agent.updatePosition; }
        set {
            agent.updatePosition = value;
        }
    }

    public bool AgentUpdateRotation {
        get { return agent.updateRotation; }
        set {
            agent.updateRotation = value;
        }
    }

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        tankMovement = GetComponent<TankMovement>();
        tankWeapon = GetComponent<TankWeapon>();

        agent.updatePosition = agentUpdatePosition;
        agent.updateRotation = agentUpdateRotation;
    }
    private void Start() {
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("shootTimer", 0.0f, shootInterval);
        InvokeRepeating("moveTimer", 0.0f, moveInterval);
    }
    private void FixedUpdate() {
        if (!agent.isStopped && agentCustomMovement) {
            agent.nextPosition = transform.position;
            tankMovement.onMoveInput(agent.desiredVelocity);
        }
        aim();
    }
    private void aim() {
        tankMovement.aimAt(player.transform.position);
        
        /*if (agent.isStopped) {
            if (player != null) {
                tankMovement.aimAt(player.transform.position);
            }
        }
        else {
            tankMovement.aimAt(agent.destination);
        }*/
    }
    private void setDestination(Vector3 position) {
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(position, out navHit, 1.0f, NavMesh.AllAreas)) {
            agent.isStopped = false;
            agent.SetDestination(navHit.position);
        }
    }
    private void shootTimer() {
        tankWeapon.Fire();
    }
    private void moveTimer() {
        if (player != null) {
            setDestination(getRandomOffset(player.transform.position, radius));
        }
    }
    private Vector3 getRandomOffset(Vector3 position, float radius) {
        Vector3 offset = UnityEngine.Random.onUnitSphere;
        offset.y = 0;
        offset = offset.normalized * radius;
        return position + offset;
    }
}
