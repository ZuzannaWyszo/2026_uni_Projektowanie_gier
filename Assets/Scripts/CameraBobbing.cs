using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CameraBobbing : MonoBehaviour
{
    public Transform cameraTarget; 
    private Vector3 initialPosition;

    public float idleAmplitude = 0.015f; 
    public float idleFrequency = 1.5f;   

    public float walkAmplitude = 0.06f;  
    public float walkFrequency = 12.0f; 

    private float timer = 0f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTarget != null)
        {
            initialPosition = cameraTarget.localPosition;
        }
    }

    void Update()
    {
        if (cameraTarget == null) return;

        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;

        if (speed > 0.1f)
        {
            timer += Time.deltaTime * walkFrequency;
            cameraTarget.localPosition = initialPosition + new Vector3(
                Mathf.Cos(timer / 2) * (walkAmplitude / 2), 
                Mathf.Sin(timer) * walkAmplitude, 
                0);
        }
        else
        {
            timer += Time.deltaTime * idleFrequency;
            cameraTarget.localPosition = initialPosition + new Vector3(
                Mathf.Cos(timer / 2) * (idleAmplitude / 2), 
                Mathf.Sin(timer) * idleAmplitude, 
                0);
        }
    }
}