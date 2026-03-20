using UnityEngine;

public class PuzzleSocket : MonoBehaviour
{
    public string requiredColor;
    public Puzzle1Manager puzzleManager;
    private bool isFilled = false;
    public Renderer socketRenderer;
    public Material correctMaterial;

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

        if (isFilled && socketRenderer != null && correctMaterial != null)
        {
            socketRenderer.material = correctMaterial;
        }
    }

    public bool GetFilled()
    {
        return isFilled;
    }
}