using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class GrabbableMaterialSwitcher : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private MeshRenderer meshRenderer;

    [Header("Material Settings")]
    [SerializeField] private Material grabMaterial;     // e.g. GrabMat (red)
    [SerializeField] private Material defaultMaterial;  // e.g. BlueMat or BlackMat

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(SetGrabMaterial);
        grabInteractable.selectExited.AddListener(SetDefaultMaterial);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(SetGrabMaterial);
        grabInteractable.selectExited.RemoveListener(SetDefaultMaterial);
    }

    private void SetGrabMaterial(SelectEnterEventArgs args)
    {
        meshRenderer.material = grabMaterial;
    }

    private void SetDefaultMaterial(SelectExitEventArgs args)
    {
        meshRenderer.material = defaultMaterial;
    }
}