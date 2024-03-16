using UnityEngine;
using UnityEngine.UIElements;

namespace Inventory {
    public class InventoryItem {

        public VisualElement itemCard;
        Label displayName;
        public Item item;

        public InventoryItem(Item item, VisualTreeAsset template)
        {
            itemCard = template.Instantiate();

            displayName = itemCard.Q<Label>("DisplayName");
            displayName.text = item.name;

            Label itemPrice = itemCard.Q<Label>("ItemPrice");
            itemPrice.text = item.price.ToString();

            VisualElement itemImage = itemCard.Q<VisualElement>("ItemImage");
            itemImage.style.backgroundColor = Color.green;

            Button button = itemCard.Q<Button>();
            button.style.backgroundColor = Color.yellow;
            button.RegisterCallback<ClickEvent>(OnClick);

            this.item = item;
        }

        public void OnClick(ClickEvent evt)
        {
            Debug.Log(displayName.text);
        }
    }
}