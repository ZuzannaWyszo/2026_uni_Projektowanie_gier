using UnityEngine;

public class PipePiece : MonoBehaviour
{
    public bool top;
    public bool right;
    public bool bottom;
    public bool left;

    public int currentRotation { get; private set; } = 0;

    public void RotatePipe()
    {
        transform.Rotate(0, 0, -90f);
        currentRotation = (currentRotation + 90) % 360;

        bool temp = top;
        top = left;
        left = bottom;
        bottom = right;
        right = temp;
    }

    public void SetRotation(int angle)
    {
        currentRotation = 0;
        transform.localEulerAngles = Vector3.zero;

        int steps = (angle / 90) % 4;
        for (int i = 0; i < steps; i++)
        {
            RotatePipe();
        }
    }

    public bool HasConnectionInDirection(int direction)
    {
        switch (direction)
        {
            case 0: return top;
            case 1: return right;
            case 2: return bottom;
            case 3: return left;
            default: return false;
        }
    }
}