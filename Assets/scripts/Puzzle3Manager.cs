using System.Collections;
using UnityEngine;
using TMPro;

public class FinalPuzzleManager : MonoBehaviour
{
    [Header("Code Settings")]
    public string correctCode = "3142";
    private string currentInput = "";

    [Header("Door Settings")]
    public Transform finalDoor;
    public float doorMoveUp = 3f;
    public float moveSpeed = 2f;

    [Header("Display")]
    public TMP_Text displayText;
    public string defaultDisplay = "----";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonSound;
    public AudioClip successSound;
    public AudioClip wrongSound;
    public AudioClip doorSound;

    private bool doorOpened = false;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    private void Start()
    {
        if (finalDoor != null)
        {
            doorClosedPos = finalDoor.position;
            doorOpenPos = finalDoor.position + new Vector3(0f, doorMoveUp, 0f);
        }

        UpdateDisplay();
    }

    public void PressDigit(string digit)
    {
        if (doorOpened) return;
        if (currentInput.Length >= correctCode.Length) return;

        currentInput += digit;

        if (audioSource != null && buttonSound != null)
            audioSource.PlayOneShot(buttonSound);

        UpdateDisplay();

        if (currentInput.Length == correctCode.Length)
        {
            if (currentInput == correctCode)
            {
                if (audioSource != null && successSound != null)
                    audioSource.PlayOneShot(successSound);

                if (displayText != null)
                    displayText.text = "OPEN";

                doorOpened = true;
                StartCoroutine(OpenDoor());
            }
            else
            {
                if (audioSource != null && wrongSound != null)
                    audioSource.PlayOneShot(wrongSound);

                if (displayText != null)
                    displayText.text = "ERROR";

                StartCoroutine(ResetAfterDelay());
            }
        }
    }

    public void ResetCode()
    {
        if (doorOpened) return;

        currentInput = "";

        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);

        UpdateDisplay();
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        currentInput = "";
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (displayText == null) return;

        if (string.IsNullOrEmpty(currentInput))
            displayText.text = defaultDisplay;
        else
            displayText.text = currentInput;
    }

    private IEnumerator OpenDoor()
    {
        if (audioSource != null && doorSound != null)
            audioSource.PlayOneShot(doorSound);

        while (Vector3.Distance(finalDoor.position, doorOpenPos) > 0.01f)
        {
            finalDoor.position = Vector3.MoveTowards(finalDoor.position, doorOpenPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        finalDoor.position = doorOpenPos;
    }
}