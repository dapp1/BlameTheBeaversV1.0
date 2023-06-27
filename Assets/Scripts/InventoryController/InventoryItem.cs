using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Create Inventory Item", order = 1)]
[Serializable]
public class InventoryItem : ScriptableObject
{
    public Sprite UIIcon;
    public Sprite UIIconUnavailable;
    public InventoryItemObject Prefab;
    public InventoryItemType Type;
    public int Price = 100;
    public int MaxDurability = 10;
}
