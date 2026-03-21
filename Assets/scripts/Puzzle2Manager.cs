using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Puzzle2Manager : MonoBehaviour
{
    public Transform powerButton;
    public Transform door;

    public float buttonRiseAmount = 0.2f;
    public float moveSpeed = 2f;
    public float doorMoveUp = 3f;

    public int totalFusesNeeded = 3;
    private int insertedFuses = 0;

    private Vector3 buttonStartPos;
    private Vector3 buttonUpPos;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    private bool buttonActivated = false;
    private bool doorOpened = false;

    public AudioSource audioSource;
    public AudioClip placeSound;
    public AudioClip successSound;
    public AudioClip buttonRiseSound;
    public AudioClip doorSound;

    private void Start()
    {
        if (powerButton != null)
        {
            buttonStartPos = powerButton.position;
            buttonUpPos = powerButton.position + new Vector3(0f, buttonRiseAmount, 0f);
        }

        if (door != null)
        {
            doorClosedPos = door.position;
            doorOpenPos = door.position + new Vector3(0f, doorMoveUp, 0f);
        }
    }

    public void TryPlaceFuse(FuseSocket socket, Fuse fuse)
    {
        if (socket.GetFilled()) return;

        XRGrabInteractable grab = fuse.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.enabled = false;
        }

        Rigidbody rb = fuse.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        fuse.transform.position = socket.transform.position;
        fuse.transform.rotation = socket.transform.rotation;

        socket.SetFilled(true);
        insertedFuses++;

        if (audioSource != null && placeSound != null)
        {
            audioSource.PlayOneShot(placeSound);
        }

        CheckPuzzleComplete();
    }

    void CheckPuzzleComplete()
    {
        if (insertedFuses < totalFusesNeeded) return;

        if (!buttonActivated)
        {
            buttonActivated = true;

            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }

            StartCoroutine(RaiseButton());
        }
    }

    IEnumerator RaiseButton()
    {
        if (audioSource != null && buttonRiseSound != null)
        {
            audioSource.PlayOneShot(buttonRiseSound);
        }

        while (Vector3.Distance(powerButton.position, buttonUpPos) > 0.01f)
        {
            powerButton.position = Vector3.MoveTowards(powerButton.position, buttonUpPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        powerButton.position = buttonUpPos;
    }

    public void PressButton()
    {
        if (!buttonActivated || doorOpened) return;

        doorOpened = true;
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        while (Vector3.Distance(door.position, doorOpenPos) > 0.01f)
        {
            door.position = Vector3.MoveTowards(door.position, doorOpenPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        door.position = doorOpenPos;
    }
}