using UnityEngine;

public class FuseSocket : MonoBehaviour
{
    public Puzzle2Manager puzzleManager;
    private bool isFilled = false;

    public Renderer socketRenderer;
    public Material correctMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (isFilled) return;

        Fuse fuse = other.GetComponent<Fuse>();
        if (fuse != null)
        {
            puzzleManager.TryPlaceFuse(this, fuse);
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