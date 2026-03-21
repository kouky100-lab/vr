using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class KeypadButton : MonoBehaviour
{
    public string buttonValue;
    public bool isResetButton = false;
    public FinalPuzzleManager manager;

    private XRSimpleInteractable interactable;
    private Vector3 originalPos;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void Start()
    {
        originalPos = transform.position;
    }

    private void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }

    private void OnDisable()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnPressed);
    }

    private void OnPressed(SelectEnterEventArgs args)
    {
        StartCoroutine(PressAnimation());

        if (manager == null) return;

        if (isResetButton)
            manager.ResetCode();
        else
            manager.PressDigit(buttonValue);
    }

    private IEnumerator PressAnimation()
    {
        Vector3 downPos = originalPos - new Vector3(0f, 0.02f, 0f);
        transform.position = downPos;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
    }
}