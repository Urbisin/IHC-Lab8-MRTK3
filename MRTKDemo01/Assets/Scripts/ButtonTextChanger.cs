using Microsoft.MixedReality.Toolkit;
using TMPro;
using UnityEngine;

/// Lab 1: al presionar el boton MRTK3 cambia su texto (y vuelve al original al presionar de nuevo).
[RequireComponent(typeof(StatefulInteractable))]
public class ButtonTextChanger : MonoBehaviour
{
    [SerializeField] private string newText = "Boton presionado!";

    private StatefulInteractable interactable;
    private TMP_Text label;
    private string originalText;
    private bool changed;

    private void Awake()
    {
        interactable = GetComponent<StatefulInteractable>();
        label = GetComponentInChildren<TMP_Text>(true);
        if (label != null) originalText = label.text;
    }

    private void OnEnable() => interactable.OnClicked.AddListener(OnClick);
    private void OnDisable() => interactable.OnClicked.RemoveListener(OnClick);

    private void OnClick()
    {
        if (label == null) return;
        changed = !changed;
        label.text = changed ? newText : originalText;
    }
}
