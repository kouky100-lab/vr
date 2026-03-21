using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class StartButton : MonoBehaviour
{
    public Transform door;
    public float moveUpDistance = 3f;
    public float moveSpeed = 2f;

    private XRSimpleInteractable interactable;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private Vector3 originalPos;

    private bool doorOpened = false;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void Start()
    {
        originalPos = transform.position;

        if (door != null)
        {
            doorClosedPos = door.position;
            doorOpenPos = door.position + new Vector3(0f, moveUpDistance, 0f);
        }
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnPressed);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnPressed);
    }

    private void OnPressed(SelectEnterEventArgs args)
    {
        if (doorOpened) return;

        doorOpened = true;

        StartCoroutine(PressAnimation());
        StartCoroutine(OpenDoor());
    }

    IEnumerator PressAnimation()
    {
        Vector3 down = originalPos - new Vector3(0f, 0.02f, 0f);
        transform.position = down;
        yield return new WaitForSeconds(0.1f);
        transform.position = originalPos;
    }

    IEnumerator OpenDoor()
    {
        while (Vector3.Distance(door.position, doorOpenPos) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, doorOpenPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = doorOpenPos;
    }
}