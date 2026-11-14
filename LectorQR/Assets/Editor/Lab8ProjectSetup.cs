using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Editor;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR.OpenXR;

/// Menu: Lab8 > Configurar proyecto para HoloLens 2
/// Hace en un clic lo de la guia: perfil MRTK, OpenXR (Standalone + UWP), perfiles de
/// interaccion, input system y capacidades UWP (WebCam, Microphone...).
public static class Lab8ProjectSetup
{
    const string ProfilePath = "Packages/com.microsoft.mrtk.core/Configuration/Default Profiles/MRTKProfile.asset";

    // Features de OpenXR a habilitar (por nombre de clase).
    static readonly HashSet<string> Common = new HashSet<string>
    {
        "MixedRealityFeaturePlugin",        // Windows Mixed Reality / HoloLens feature group
        "HandTrackingFeaturePlugin",
        "MotionControllerFeaturePlugin",
        "EyeGazeInteraction",               // Eye Gaze Interaction Profile
        "MicrosoftHandInteraction",         // Microsoft Hand Interaction Profile
        "MicrosoftMotionControllerProfile", // Microsoft Motion Controller Profile
    };
    // Holographic Remoting NO se activa aqui: se activa en Mixed Reality > Remoting > Holographic Remoting for Play Mode.
    static readonly HashSet<string> StandaloneOnly = new HashSet<string>();

    [MenuItem("Lab8/Configurar proyecto para HoloLens 2")]
    public static void Configure()
    {
        // 1) Input: "Both" (old + new Input System) como pide MRTK3.
        var ps = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset");
        if (ps.Length > 0)
        {
            var so = new SerializedObject(ps[0]);
            var p = so.FindProperty("activeInputHandler");
            if (p != null && p.intValue != 2) { p.intValue = 2; so.ApplyModifiedProperties(); }
        }

        // 2) Capacidades UWP (manifest) para camara/QR, microfono, mapeo espacial y red.
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.WebCam, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.Microphone, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.SpatialPerception, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.InternetClient, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.PrivateNetworkClientServer, true);
        PlayerSettings.WSA.SetCapability(PlayerSettings.WSACapability.GazeInput, true);

        // 3) OpenXR como loader en Standalone y UWP + features.
        SetupXR(BuildTargetGroup.Standalone);
        SetupXR(BuildTargetGroup.WSA);

        // 4) Perfil MRTK por defecto en Standalone y UWP.
        var profile = AssetDatabase.LoadAssetAtPath<MRTKProfile>(ProfilePath);
        if (profile != null)
        {
            MRTKSettings.ProfileForBuildTarget(BuildTargetGroup.Standalone); // crea Assets/MRTK.Generated/MRTKSettings.asset
            var mrtk = AssetDatabase.LoadAssetAtPath<MRTKSettings>("Assets/MRTK.Generated/MRTKSettings.asset");
            if (mrtk != null)
            {
                mrtk.SetProfileForBuildTarget(BuildTargetGroup.Standalone, profile);
                mrtk.SetProfileForBuildTarget(BuildTargetGroup.WSA, profile);
                EditorUtility.SetDirty(mrtk);
            }
        }
        else Debug.LogWarning("[Lab8] No se encontro MRTKProfile en " + ProfilePath);

        AssetDatabase.SaveAssets();
        Debug.Log("[Lab8] Proyecto configurado. Si Unity pide reiniciar por el Input System, acepta.");
    }

    static void SetupXR(BuildTargetGroup group)
    {
        if (!EditorBuildSettings.TryGetConfigObject(XRGeneralSettings.k_SettingsKey, out XRGeneralSettingsPerBuildTarget perTarget) || perTarget == null)
        {
            System.IO.Directory.CreateDirectory("Assets/XR");
            perTarget = ScriptableObject.CreateInstance<XRGeneralSettingsPerBuildTarget>();
            AssetDatabase.CreateAsset(perTarget, "Assets/XR/XRGeneralSettingsPerBuildTarget.asset");
            EditorBuildSettings.AddConfigObject(XRGeneralSettings.k_SettingsKey, perTarget, true);
        }
        if (!perTarget.HasSettingsForBuildTarget(group)) perTarget.CreateDefaultSettingsForBuildTarget(group);
        if (!perTarget.HasManagerSettingsForBuildTarget(group)) perTarget.CreateDefaultManagerSettingsForBuildTarget(group);

        var general = perTarget.SettingsForBuildTarget(group);
        general.InitManagerOnStart = true;
        XRPackageMetadataStore.AssignLoader(perTarget.ManagerSettingsForBuildTarget(group), typeof(OpenXRLoader).FullName, group);
        EditorUtility.SetDirty(perTarget);

        UnityEditor.XR.OpenXR.Features.FeatureHelpers.RefreshFeatures(group);
        var oxr = OpenXRSettings.GetSettingsForBuildTargetGroup(group);
        if (oxr == null) { Debug.LogWarning("[Lab8] Sin OpenXRSettings para " + group + " (falta el modulo?)"); return; }
        foreach (var f in oxr.GetFeatures())
        {
            string n = f.GetType().Name;
            if (Common.Contains(n) || (group == BuildTargetGroup.Standalone && StandaloneOnly.Contains(n)))
            {
                f.enabled = true;
                EditorUtility.SetDirty(f);
            }
        }
        EditorUtility.SetDirty(oxr);
        Debug.Log("[Lab8] OpenXR configurado para " + group);
    }
}
