using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HotbarController : MonoBehaviour
{
    [Header("UI Elements")]
    public Image[] slotBackgrounds;
    public Image[] itemIcons;

    [Header("Equipment")]
    public ItemData[] items = new ItemData[5];

    [Header("Place of holding item")]
    public Transform itemHolder; // Obiekt ItemHolder umieszczony pod kamerą

    [Header("Colors")]
    public Color unselectedColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color selectedColor = new Color(1f, 1f, 1f, 1f);

    private int selectedSlot = 0;
    private GameObject currentHeldObject; // Przechowuje aktualnie zespawnowany model 3D

    void Start()
    {
        UpdateUI();
        SelectSlot(selectedSlot);
    }

    void Update()
    {
        // Zmiana slotu rolką myszy
        if (Mouse.current != null)
        {
            float scroll = Mouse.current.scroll.ReadValue().y;
            if (scroll > 0f)
            {
                selectedSlot--;
                if (selectedSlot < 0) selectedSlot = slotBackgrounds.Length - 1;
                SelectSlot(selectedSlot);
            }
            else if (scroll < 0f)
            {
                selectedSlot++;
                if (selectedSlot >= slotBackgrounds.Length) selectedSlot = 0;
                SelectSlot(selectedSlot);
            }
        }

        // Zmiana slotu klawiszami 1-5
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame) SelectSlot(0);
            if (Keyboard.current.digit2Key.wasPressedThisFrame) SelectSlot(1);
            if (Keyboard.current.digit3Key.wasPressedThisFrame) SelectSlot(2);
            if (Keyboard.current.digit4Key.wasPressedThisFrame) SelectSlot(3);
            if (Keyboard.current.digit5Key.wasPressedThisFrame) SelectSlot(4);
        }
    }

    void SelectSlot(int index)
    {
        selectedSlot = index;

        // 1. Zmiana podświetlenia w UI
        for (int i = 0; i < slotBackgrounds.Length; i++)
        {
            if (i == selectedSlot)
                slotBackgrounds[i].color = selectedColor;
            else
                slotBackgrounds[i].color = unselectedColor;
        }

        // 2. Podmiana modelu 3D w dłoni
        UpdateHeldItem();
    }

    void UpdateHeldItem()
    {
        // Usuń obecny model z dłoni, jeśli istnieje
        if (currentHeldObject != null)
        {
            Destroy(currentHeldObject);
        }

        // Stwórz nowy model, jeśli w slocie znajduje się przedmiot z przypisanym Prefab3D
        if (selectedSlot < items.Length && items[selectedSlot] != null && items[selectedSlot].prefab3D != null)
        {
            currentHeldObject = Instantiate(items[selectedSlot].prefab3D, itemHolder);
            currentHeldObject.transform.localPosition = Vector3.zero;
            currentHeldObject.transform.localRotation = Quaternion.identity;
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (i < items.Length && items[i] != null && items[i].itemIcon != null)
            {
                itemIcons[i].sprite = items[i].itemIcon;
                itemIcons[i].enabled = true;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
            }
        }
    }
}