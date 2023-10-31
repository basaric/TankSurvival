using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : TankEnemyController {
    [Header("Behaviour")]
    public float shootInterval = 3f;
    public float moveInterval = 5f;

    [Range(6f, 18f)]
    public float radius = 20f;

    protected override void Start() {
        base.Start();
        InvokeRepeating("shootTimer", 0.0f, shootInterval);
        InvokeRepeating("moveTimer", 0.0f, moveInterval);
    }
    private void shootTimer() {
        tankWeapon.Fire();
    }
    private void moveTimer() {
        if (player != null) {
            setDestination(getRandomOffset(player.transform.position, radius));
        }
    }
}
