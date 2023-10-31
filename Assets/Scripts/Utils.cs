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
}
