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
    [MenuItem("TankSurvival/Recompile Shaders")]
    private static void RecompileShaders() {
        Shader[] shaders = Resources.FindObjectsOfTypeAll<Shader>();

        foreach (Shader shader in shaders) {
            if (shader == null)
                continue;

            string shaderPath = AssetDatabase.GetAssetPath(shader);

            if (!string.IsNullOrEmpty(shaderPath)) {
                AssetDatabase.ImportAsset(shaderPath, ImportAssetOptions.ForceUpdate);
                Debug.Log("Recompiled: " + shader.name);
            }
        }

        Debug.Log("Shader recompilation complete.");
    }
}
