using UnityEngine;

public class korytarz_segment : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider_size();
    }

    void collider_size()
    {
        Collider col = GetComponent<Collider>();
        if (!col)
        {
            Debug.Log($"{gameObject.name} nie ma przyznaczonego box collider'a.\n Nalezy przydzielic collider aby obliczenia byly odpowiednie.");
        }
    
    }
}
