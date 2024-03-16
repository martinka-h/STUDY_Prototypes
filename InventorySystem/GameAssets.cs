using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameAssets : MonoBehaviour {
    private static GameAssets _Instance;

    public static GameAssets Instance
    {
        get {
            if (_Instance == null) _Instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _Instance;
        }
    }

    [Header("UI - Panel settings")]
    public PanelSettings inventorySystem;
    public VisualTreeAsset inventoryLayout;
    public VisualTreeAsset inventoryItemCard;

    [Header("Inventory Items")]
    public List<ScriptableObject> items1;
    public List<ScriptableObject> items2;
    public List<ScriptableObject> items3;
}
