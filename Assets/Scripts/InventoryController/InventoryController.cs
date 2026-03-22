using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventBusSystem;
using Pixelplacement;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] private LootBox _lootBoxPrefab;
    
    private readonly Dictionary<InventoryItemType, InventoryItemDto> _inventory = new();

    public List<InventoryItem> ItemsCatalog;

    public List<InventoryCell> Cells;
    
    private InventoryItemType _activeItemType;

    private bool canSwitchItems = true;
    
    private readonly List<InventoryItemType> _switchableItems = new()
    {
        InventoryItemType.Axe,
        InventoryItemType.Shovel
    };
    
    private Dictionary<InventoryItemType, InventoryItem> _itemsByType;
    
    private void Awake()
    {
        _inventory.Add(InventoryItemType.Hands, new InventoryItemDto{Type = InventoryItemType.Hands, Durability = Mathf.Infinity});
        _itemsByType = ItemsCatalog.ToDictionary(x => x.Type);
    }
    
    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0 && canSwitchItems)
        {
            StartCoroutine(SwitchItems(scroll > 0 ? 1 : -1));
        }
    }

    private int _currentIndex = 0;

    private IEnumerator SwitchItems(int direction)
    {
        canSwitchItems = false;

        _currentIndex += direction;

        if (_currentIndex >= _switchableItems.Count)
            _currentIndex = 0;

        if (_currentIndex < 0)
            _currentIndex = _switchableItems.Count - 1;

        var type = _switchableItems[_currentIndex];

        if (_inventory.TryGetValue(type, out var dto))
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
        if (_inventory.ContainsKey(dto.Type))
            ThrowItem(_inventory[dto.Type]);

        _inventory[dto.Type] = dto;
        SetActiveItem(dto);
        UpdateUI();
    }

    private void SetActiveItem(InventoryItemDto dto)
    {
        EventBus.Publish(new OnActiveItemChangedEvent(dto));
        _activeItemType = dto.Type;
    }
    
    public void Remove(InventoryItemDto dto)
    {
        if (!_inventory.ContainsKey(dto.Type))
            return;

        if (_activeItemType == dto.Type)
            SetActiveItem(_inventory[InventoryItemType.Hands]);

        _inventory.Remove(dto.Type);
        UpdateUI();
    }
    
    private void ThrowItem(InventoryItemDto dto)
    {
        var itemSettings = _itemsByType[dto.Type];
        
        InventoryItemObject itemObject = Instantiate(itemSettings.Prefab, Vector3.one, Quaternion.identity);
        itemObject.InventoryItemDto = dto;
        
        _inventory.Remove(dto.Type);
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
        return _itemsByType[type];
    }
    
    public void UpdateUI()
    {
        foreach (var key in _inventory.Keys)
        {
            if (key == InventoryItemType.Hands)
                continue;
            
            var itemSettings = _itemsByType[key];
            
            var item = _inventory[key];
            var cell = Cells[(int)key];
            
            cell.SetActive(item != null, itemSettings);

            if (item != null)
                cell.Select(item.Type == _activeItemType);
            
            cell.SetDurability(item != null ? 1 - (item.Durability / itemSettings.MaxDurability) : 0);
        }
    }
}
