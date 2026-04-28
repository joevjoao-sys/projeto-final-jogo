using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{
    [Header("UI Slots")]
    public Image[] slots;
    public Color normalColor = Color.gray;
    public Color selectedColor = Color.white;

    private int currentSlotIndex = 0;

    void Start()
    {
        UpdateHotbarVisuals();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SelectSlot(0); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
    }

    void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length) return; // Proteção extra
        currentSlotIndex = index;
        UpdateHotbarVisuals();
    }

    void UpdateHotbarVisuals()
    {
        // Corrigido para .Length com L maiúsculo
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == currentSlotIndex)
            {
                slots[i].color = selectedColor;
                slots[i].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            else
            {
                slots[i].color = normalColor;
                slots[i].transform.localScale = Vector3.one;
            }
        }
    }
}
