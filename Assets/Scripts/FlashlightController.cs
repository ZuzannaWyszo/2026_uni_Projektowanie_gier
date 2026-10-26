using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    public Light flashlightSpotlight;
    public bool isTurnedOn = true;

    void Start()
    {
        if (flashlightSpotlight != null)
        {
            flashlightSpotlight.enabled = isTurnedOn;
        }
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
        {
            ToggleLight();
        }

    }

    private void ToggleLight()
    {
        isTurnedOn = !isTurnedOn;
        if (flashlightSpotlight != null)
        {
            flashlightSpotlight.enabled = isTurnedOn;
        }
    }
}