using UnityEngine;

public abstract class BasePuzzle : MonoBehaviour
{
    [Header("Stan Zagadki")]
    public bool isSolved = false;

    //override
    protected virtual void OnPuzzleSolved()
    {
        if (isSolved) return;
        isSolved = true;

        Debug.Log("🎉 Zagadka rozwiązana! (Domyślna akcja bazowa)");
    }
}