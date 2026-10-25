using UnityEngine;
using UnityEngine.UI;
using StarterAssets; 

public class StaminaController : MonoBehaviour
{
    [Header("UI Pasek Sprintu")]
    public Image staminaBar;

    [Header("Parametry")]
    public float maxStamina = 100f;
    public float drainRate = 25f; 
    public float regenRate = 15f; 

    [Header("Kolory (Opcjonalne)")]
    public Color normalColor = Color.white;
    public Color exhaustedColor = Color.white; 

    private float currentStamina;
    private StarterAssetsInputs inputs;
    
    private bool isExhausted = false; 

    void Start()
    {
        inputs = GetComponent<StarterAssetsInputs>();
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (inputs == null) return;

        bool isMoving = inputs.move != Vector2.zero;
        if (isExhausted)
        {
            inputs.sprint = false; 
            
            currentStamina += regenRate * Time.deltaTime; 
            
            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isExhausted = false; 
            }
        }
        else 
        {
            if (inputs.sprint && isMoving) 
            {
                currentStamina -= drainRate * Time.deltaTime;

                if (currentStamina <= 0)
                {
                    currentStamina = 0;
                    isExhausted = true; 
                    inputs.sprint = false; 
                }
            }
            else 
            {
                if (currentStamina < maxStamina)
                {
                    currentStamina += regenRate * Time.deltaTime;
                    if (currentStamina > maxStamina) currentStamina = maxStamina;
                }
            }
        }

        if (staminaBar != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
            
            staminaBar.color = isExhausted ? exhaustedColor : normalColor;
        }
    }
}