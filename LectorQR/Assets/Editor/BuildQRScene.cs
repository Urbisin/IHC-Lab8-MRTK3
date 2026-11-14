using Microsoft.MixedReality.OpenXR;
using Microsoft.MixedReality.Toolkit.SpatialManipulation;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// Menu: Lab8 > Lab2 - Construir escena LectorQRScene
/// Arma la escena del lector de QR y el cubo manipulable (parte "Tarea") con un clic.
public static class BuildQRScene
{
    const string XRRig  = "Packages/com.microsoft.mrtk.input/Assets/Prefabs/MRTK XR Rig.prefab";
    const string Dialog = "Packages/com.microsoft.mrtk.uxcomponents/Dialog/CanvasDialog.prefab";
    const string Bounds = "Packages/com.microsoft.mrtk.spatialmanipulation/BoundsControl/Prefabs/BoundingBox.prefab";
    const string MarkerPrefabPath = "Assets/Prefabs/QRCodeMarker.prefab";

    [MenuItem("Lab8/Lab2 - Construir escena LectorQRScene")]
    public static void Build()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        var mainCam = Camera.main;
        if (mainCam != null) Object.DestroyImmediate(mainCam.gameObject);

        var rig = Instantiate(XRRig, Vector3.zero);
        var rigCamera = rig != null ? rig.GetComponentInChildren<Camera>(true) : null;

        // Prefab del marcador
        System.IO.Directory.CreateDirectory("Assets/Prefabs");
        var markerGo = new GameObject("QRCodeMarker");
        markerGo.AddComponent<ARMarker>();
        var markerPrefab = PrefabUtility.SaveAsPrefabAsset(markerGo, MarkerPrefabPath);
        Object.DestroyImmediate(markerGo);

        // Dialogo para mostrar el contenido del QR
        var dialog = Instantiate(Dialog, new Vector3(0f, 1.5f, 1.2f));
        var label = dialog != null ? dialog.GetComponentInChildren<TMP_Text>(true) : null;

        // Objeto QRCodeReading
        var reading = new GameObject("QRCodeReading");
        var origin = reading.AddComponent<XROrigin>();
        origin.Camera = rigCamera;
        origin.Origin = reading; // campo "Origin Base GameObject" en el Inspector
        var manager = reading.AddComponent<ARMarkerManager>();
        manager.markerPrefab = markerPrefab;
        var detector = reading.AddComponent<TestQRCodeDetection>();
        var so = new SerializedObject(detector);
        so.FindProperty("markerManager").objectReferenceValue = manager;
        so.FindProperty("output").objectReferenceValue = label;
        so.ApplyModifiedPropertiesWithoutUndo();

        BuildCube();

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/LectorQRScene.unity");
        Debug.Log("[Lab8] LectorQRScene creada.");
    }

    static void BuildCube()
    {
        var parent = new GameObject("cuboparent");
        parent.transform.position = new Vector3(0.5f, 1.2f, 1f);
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.name = "Cubo";
        cube.transform.SetParent(parent.transform, false);
        cube.transform.localScale = Vector3.one * 0.15f;
        var col = parent.AddComponent<BoxCollider>();
        col.size = Vector3.one * 0.15f;

        parent.AddComponent<ObjectManipulator>();
        var bc = parent.AddComponent<BoundsControl>();
        bc.BoundsVisualsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Bounds);
        parent.AddComponent<MinMaxScaleConstraint>();
    }

    static GameObject Instantiate(string path, Vector3 pos)
    {
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab == null) { Debug.LogError("[Lab8] No se encontro el prefab: " + path); return null; }
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = pos;
        return go;
    }
}
