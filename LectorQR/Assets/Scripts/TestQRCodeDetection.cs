using Microsoft.MixedReality.OpenXR;
using TMPro;
using UnityEngine;

/// Lab 2: detecta codigos QR con HoloLens 2 (ARMarkerManager) y muestra su contenido.
public class TestQRCodeDetection : MonoBehaviour
{
    [SerializeField] private ARMarkerManager markerManager;
    [SerializeField] private TMP_Text output;

    private void OnEnable()
    {
        if (markerManager != null) markerManager.markersChanged += OnMarkersChanged;
    }

    private void OnDisable()
    {
        if (markerManager != null) markerManager.markersChanged -= OnMarkersChanged;
    }

    private void OnMarkersChanged(ARMarkersChangedEventArgs args)
    {
        foreach (var marker in args.added)
        {
            string text = marker.GetDecodedString();
            Debug.Log("[QR] Detectado: " + text);
            if (output != null) output.text = text;
        }
    }

    // Solo para probar la UI en el editor sin HoloLens.
    [ContextMenu("Simular lectura de QR")]
    private void Simulate()
    {
        if (output != null) output.text = "QR simulado: https://utec.edu.pe";
    }
}
