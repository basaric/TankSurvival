using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : TankEnemyController {
    [Header("Behaviour")]
    public float shootIntervalMin = 2f;
    public float shootIntervalMax = 6f;

    public float moveInterval = 5f;

    [Range(6f, 18f)]
    public float radius = 20f;

    protected override void Start() {
        base.Start();
        Invoke("shootTimer", Random.Range(shootIntervalMin, shootIntervalMax));
        InvokeRepeating("moveTimer", 0.0f, moveInterval);
    }
    private void shootTimer() {
        tankWeapon.fire();
        Invoke("shootTimer", Random.Range(shootIntervalMin, shootIntervalMax));
    }
    private void moveTimer() {
        if (player != null) {
            setDestination(getRandomOffset(player.transform.position, radius));
        }
    }
}
