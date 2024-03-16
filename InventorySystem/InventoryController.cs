using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace Inventory {
    public class InventoryController : MonoBehaviour {

        //remember to assign your Panel Settings in the Game Assets prefab.
        private VisualTreeAsset inventoryCardTemplate;

        private VisualElement activeTab;
        private VisualElement homeTab;
        private VisualElement tab1;
        private VisualElement tab2;
        private VisualElement tab3;

        private void Start()
        {
            GameObject inventory = new GameObject("UI - Inventory", typeof(UIDocument));
            inventoryCardTemplate = GameAssets.Instance.inventoryItemCard;

            UIDocument uiDocument = inventory.GetComponent<UIDocument>();
            uiDocument.panelSettings = GameAssets.Instance.inventorySystem;
            uiDocument.visualTreeAsset = GameAssets.Instance.inventoryLayout;

            // assign functions to buttons

            Button homeBtn = uiDocument.rootVisualElement.Q<Button>("HomeBtn");
            homeBtn.RegisterCallback<ClickEvent>(GoToHome);

            Button tab1Btn = uiDocument.rootVisualElement.Q<Button>("Tab1Btn");
            tab1Btn.RegisterCallback<ClickEvent>(GoToTab1);

            Button tab2Btn = uiDocument.rootVisualElement.Q<Button>("Tab2Btn");
            tab2Btn.RegisterCallback<ClickEvent>(GoToTab2);

            Button tab3Btn = uiDocument.rootVisualElement.Q<Button>("Tab3Btn");
            tab3Btn.RegisterCallback<ClickEvent>(GoToTab3);

            // define the tabs

            homeTab = uiDocument.rootVisualElement.Q("Tab0");
            tab1 = uiDocument.rootVisualElement.Q("Tab1");
            tab2 = uiDocument.rootVisualElement.Q("Tab2");
            tab3 = uiDocument.rootVisualElement.Q("Tab3");

            // populate the tabs with items of the right category

            foreach (Item item in GameAssets.Instance.items1) {
                InventoryItem newItem = new InventoryItem(item, inventoryCardTemplate);
                tab1.Add(newItem.itemCard);
            }

            foreach (Item item in GameAssets.Instance.items2) {
                InventoryItem newItem = new InventoryItem(item, inventoryCardTemplate);
                tab2.Add(newItem.itemCard);
            }

            foreach (Item item in GameAssets.Instance.items3) {
                InventoryItem newItem = new InventoryItem(item, inventoryCardTemplate);
                tab3.Add(newItem.itemCard);
            }


            // define the active tab

            homeTab.style.display = DisplayStyle.Flex;
            tab1.style.display = DisplayStyle.None;
            tab2.style.display = DisplayStyle.None;
            tab3.style.display = DisplayStyle.None;

            activeTab = homeTab;
        }

        #region On Click Methods
        public void GoToHome(ClickEvent evt)
        {
            activeTab.style.display = DisplayStyle.None;
            homeTab.style.display = DisplayStyle.Flex;
            activeTab = homeTab;
        }

        private void GoToTab1(ClickEvent evt)
        {
            activeTab.style.display = DisplayStyle.None;
            tab1.style.display = DisplayStyle.Flex;
            activeTab = tab1;
        }

        private void GoToTab2(ClickEvent evt)
        {
            activeTab.style.display = DisplayStyle.None;
            tab2.style.display = DisplayStyle.Flex;
            activeTab = tab2;
        }

        private void GoToTab3(ClickEvent evt)
        {
            activeTab.style.display = DisplayStyle.None;
            tab3.style.display = DisplayStyle.Flex;
            activeTab = tab3;
        }
        #endregion
    }
}