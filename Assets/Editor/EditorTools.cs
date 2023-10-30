using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EditorTools {
    [MenuItem("TankSurvival/Bake Navigation")]
    static void BakeNavigation() {
        NavMeshSurface navMesh = GameObject.FindWithTag("Navigation").GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }
}
