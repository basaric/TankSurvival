using UnityEditor;
using UnityEngine;

public class EditorTools {
    [MenuItem("TankSurvival/Select Camera")]
    static void SelectCamera() {
        Selection.activeGameObject = Camera.main.gameObject;
    }
}
