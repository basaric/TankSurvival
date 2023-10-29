using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankColor : MonoBehaviour {
    public Color tankColor;

    void Start() {
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].material.color = tankColor;
        }
    }

}
