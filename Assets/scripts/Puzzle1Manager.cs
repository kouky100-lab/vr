using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;



public class Puzzle1Manager : MonoBehaviour
{
    public Transform door;
    public float moveUpDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    public AudioSource audioSource;
    public AudioClip placeSound;
    public AudioClip successSound;
    public AudioClip doorSound;

    private void Start()
    {
        if (door != null)
        {
            doorClosedPos = door.position;
            doorOpenPos = door.position + new Vector3(0f, moveUpDistance, 0f);
        }
    }

    public void TryPlaceCube(PuzzleSocket socket, PuzzleCube cube)
    {
        if (socket.GetFilled()) return;

        if (cube.cubeColor == socket.requiredColor)
        {
            XRGrabInteractable grab = cube.GetComponent<XRGrabInteractable>();
            if (grab != null)
            {
                grab.enabled = false;
            }

            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            cube.transform.position = socket.transform.position;
            cube.transform.rotation = socket.transform.rotation;

            socket.SetFilled(true);

            if (audioSource != null && placeSound != null)
            {
                audioSource.PlayOneShot(placeSound);
            }

            CheckPuzzleComplete();
        }
        else
        {
            Debug.Log("Wrong cube for this socket.");
        }
    }

    void CheckPuzzleComplete()
    {
        PuzzleSocket[] sockets = FindObjectsByType<PuzzleSocket>(FindObjectsSortMode.None);

        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        foreach (PuzzleSocket socket in sockets)
        {
            if (!socket.GetFilled())
            {
                return;
            }
        }

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