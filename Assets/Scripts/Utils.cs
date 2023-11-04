using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    public static float remapAndClamp(float value, float fromMin, float fromMax, float toMin, float toMax) {
        value = Mathf.Clamp(value, fromMin, fromMax);
        float normalizedValue = Mathf.InverseLerp(fromMin, fromMax, value);
        float remappedValue = Mathf.Lerp(toMin, toMax, normalizedValue);
        return remappedValue;
    }
    public static void lookAtPoint(Transform transform, Vector3 point) {
        Vector3 aimDirection = (point - transform.position).normalized;
        Quaternion newRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        newRotation.x = 0;
        newRotation.z = 0;
        transform.rotation = newRotation;
    }
}
