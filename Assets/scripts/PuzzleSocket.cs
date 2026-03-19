using UnityEngine;

public class PuzzleSocket : MonoBehaviour
{
    public string requiredColor;
    public Puzzle1Manager puzzleManager;
    private bool isFilled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        PuzzleCube cube = other.GetComponent<PuzzleCube>();
        if (cube != null)
        {
            puzzleManager.TryPlaceCube(this, cube);
        }
    }

    public void SetFilled(bool value)
    {
        isFilled = value;
    }

    public bool GetFilled()
    {
        return isFilled;
    }
}