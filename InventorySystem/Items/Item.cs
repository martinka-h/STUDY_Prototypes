using UnityEngine;

[CreateAssetMenu (fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    public string displayName;
    public ItemType type;
    public Sprite icon;
    public int price;
}

public enum ItemType {
    type1,
}