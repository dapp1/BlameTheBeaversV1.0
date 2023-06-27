using System;
using UnityEngine;

[Serializable]
public class InventoryItemDto
{
    public InventoryItemType Type;
    public float Durability = 100;
    public bool IsNewItem = true;
    
    public void Damage(int damage)
    {
        Durability--;
        
        InventoryController.Instance.UpdateUI();
        
        if (Durability == 0)
            InventoryController.Instance.Remove(this);
    }
}

public class InventoryItemObject : MonoBehaviour
{
    public InventoryItemDto InventoryItemDto;
    
    private ClickableObject _clickable;

    private void Start()
    {
        _clickable = GetComponent<ClickableObject>();
        _clickable.ClickEvent.AddListener(OnClick);

        if (InventoryItemDto.IsNewItem)
        {
            InventoryItemDto.IsNewItem = false;
            var settings = InventoryController.Instance.GetItemSettings(InventoryItemDto.Type);
            InventoryItemDto.Durability = settings.MaxDurability;
        }
    }

    private void OnClick()
    {
        CharacterController.Instance.PickItem(this, OnPick);
    }

    private void OnPick()
    {
        InventoryController.Instance.Add(InventoryItemDto);
        Destroy(gameObject);
    }
}
