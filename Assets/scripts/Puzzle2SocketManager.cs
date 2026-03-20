using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Puzzle2SocketManager : MonoBehaviour
{
    public XRSocketInteractor socket1;
    public XRSocketInteractor socket2;
    public XRSocketInteractor socket3;

    public Transform powerButton;
    public Transform door;

    public float buttonRiseAmount = 0.15f;
    public float moveSpeed = 2f;
    public float doorMoveUp = 3f;

    private bool buttonActivated = false;
    private bool doorOpened = false;
    private Vector3 buttonStartPos;

    private void Start()
    {
        buttonStartPos = powerButton.position;
    }

    private void Update()
    {
        if (!buttonActivated && AllSocketsFilled())
        {
            buttonActivated = true;
            StartCoroutine(RaiseButton());
        }
    }

    private bool AllSocketsFilled()
    {
        return socket1.hasSelection && socket2.hasSelection && socket3.hasSelection;
    }

    IEnumerator RaiseButton()
    {
        Vector3 targetPos = buttonStartPos + new Vector3(0f, buttonRiseAmount, 0f);

        while (Vector3.Distance(powerButton.position, targetPos) > 0.01f)
        {
            powerButton.position = Vector3.MoveTowards(powerButton.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        powerButton.position = targetPos;
    }

    public void PressButton()
    {
        if (!buttonActivated || doorOpened) return;

        doorOpened = true;
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        Vector3 targetPos = door.position + new Vector3(0f, doorMoveUp, 0f);

        while (Vector3.Distance(door.position, targetPos) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = targetPos;
    }
}