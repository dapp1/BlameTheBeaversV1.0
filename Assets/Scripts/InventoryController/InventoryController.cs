using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pixelplacement;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] private LootBox _lootBoxPrefab;
    
    private Dictionary<InventoryItemType, InventoryItemDto> _inventory =
        new Dictionary<InventoryItemType, InventoryItemDto>
        {
            {InventoryItemType.Hands, new InventoryItemDto{Type = InventoryItemType.Hands, Durability = Mathf.Infinity}},
            {InventoryItemType.Axe, null},
            {InventoryItemType.Shovel, null}
        };

    public List<InventoryItem> ItemsCatalog;

    public List<InventoryCell> Cells;
    
    private InventoryItemType _activeItemType;

    private bool canSwitchItems = true;
    
    private void Update()
    {
        var wheelDelta = Input.GetAxis("Mouse ScrollWheel");
        if (wheelDelta != 0 && canSwitchItems)
            StartCoroutine(SwitchItems(wheelDelta > 0 ? 1 : -1));
    }

    private IEnumerator SwitchItems(int direction)
    {
        canSwitchItems = false;
        
        var nextItemIndex = (int)_activeItemType + direction;
        
        if (nextItemIndex > 2)
            nextItemIndex = 1;
        
        if (nextItemIndex < 1)
            nextItemIndex = 2;

        var dto = _inventory[(InventoryItemType) nextItemIndex];
        if (dto != null)
            SetActiveItem(dto);
        
        UpdateUI();
        
        yield return new WaitForSeconds(0.2f);

        canSwitchItems = true;
    }
    
    public void Start()
    {
        UpdateUI();
    }
    
    public void Add(InventoryItemDto dto)
    {
        var itemSettings = ItemsCatalog.First(x => x.Type == dto.Type);

        if (_inventory[dto.Type] != null)
        {
            ThrowItem(_inventory[dto.Type]);
        }

        _inventory[dto.Type] = dto;

        SetActiveItem(dto);
        
        UpdateUI();
    }

    private void SetActiveItem(InventoryItemDto dto)
    {
        CharacterController.Instance.SetActiveItem(dto);
        _activeItemType = dto.Type;
    }
    

    public void Remove(InventoryItemDto dto)
    {
        var inventoryItemDto = _inventory[dto.Type];

        if (inventoryItemDto != null)
        {
            if (inventoryItemDto.Type == _activeItemType)
            {
                 SetActiveItem(_inventory[InventoryItemType.Hands]);
                 _inventory[dto.Type] = null;
            }
        }
        
        UpdateUI();
    }
    
    private void ThrowItem(InventoryItemDto dto)
    {
        var itemSettings = ItemsCatalog.First(x => x.Type == dto.Type);
        
        InventoryItemObject itemObject = Instantiate(itemSettings.Prefab, 
            CharacterController.Instance.transform.position, Quaternion.identity);

        itemObject.InventoryItemDto = dto;
        
        _inventory[dto.Type] = null;
        
        UpdateUI();
    }

    public bool BuyItem(InventoryItemType type)
    {
        var settings = GetItemSettings(type);
        return CoinsAndScoreController.Instance.ChangeCoinsValue(-settings.Price);
    }
    
    public void DeliveryItem(InventoryItemType type)
    {
        var settings = GetItemSettings(type);
        var lootBoxInstance = Instantiate(_lootBoxPrefab, new Vector3(Random.Range(-10, 10f), 10), Quaternion.identity);
        lootBoxInstance.ItemPrefab = settings.Prefab;
    }

    public InventoryItem GetItemSettings(InventoryItemType type)
    {
        return ItemsCatalog.First(x => x.Type == type);;
    }
    
    public void UpdateUI()
    {
        foreach (var key in _inventory.Keys)
        {
            if (key == InventoryItemType.Hands)
                continue;
            
            var itemSettings = ItemsCatalog.First(x => x.Type == key);
            
            var item = _inventory[key];
            var cell = Cells[(int)key];
            
            cell.SetActive(item != null, itemSettings);

            if (item != null)
                cell.Select(item.Type == _activeItemType);
            
            cell.SetDurability(item != null ? 1 - (item.Durability / itemSettings.MaxDurability) : 0);
        }
    }
}
