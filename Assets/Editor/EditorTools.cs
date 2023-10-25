using UnityEditor;
using UnityEngine;

public class EditorTools {
    [MenuItem("TankSurSelectCameravival/Select Camera")]
    static void SelectCamera() {
        Selection.activeGameObject = Camera.main.gameObject;
    }
}
