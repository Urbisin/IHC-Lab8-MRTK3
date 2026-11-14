using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// Menu: Lab8 > Lab1 - Construir escena CameraRigScena
/// Arma la escena del Lab 1 con un clic: MRTK XR Rig, Input Simulator, Slate y boton.
public static class BuildLab1Scene
{
    const string XRRig   = "Packages/com.microsoft.mrtk.input/Assets/Prefabs/MRTK XR Rig.prefab";
    const string InputSim = "Packages/com.microsoft.mrtk.input/Simulation/Prefabs/MRTKInputSimulator.prefab";
    const string Slate   = "Packages/com.microsoft.mrtk.uxcomponents.noncanvas/Slates/Slate.prefab";
    const string Button  = "Packages/com.microsoft.mrtk.uxcomponents.noncanvas/Button/128x32/PressableButton_128x32mm_TextOnly.prefab";

    [MenuItem("Lab8/Lab1 - Construir escena CameraRigScena")]
    public static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        var cam = Camera.main;
        if (cam != null) Object.DestroyImmediate(cam.gameObject); // el XR Rig ya trae camara

        Place(XRRig, Vector3.zero);
        Place(InputSim, Vector3.zero);
        Place(Slate, new Vector3(0f, 1.65f, 1.2f));
        var btn = Place(Button, new Vector3(0f, 1.25f, 0.9f));
        if (btn != null && btn.GetComponent<ButtonTextChanger>() == null)
            btn.AddComponent<ButtonTextChanger>();

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/CameraRigScena.unity");
        Debug.Log("[Lab8] Escena CameraRigScena creada. Presiona Play y haz clic en el boton.");
    }

    static GameObject Place(string path, Vector3 pos)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null) { Debug.LogError("[Lab8] No se encontro el prefab: " + path); return null; }
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = pos;
        return go;
    }
}
