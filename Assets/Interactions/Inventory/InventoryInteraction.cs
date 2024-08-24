using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class InventoryInteraction : InteractionAction
{
    public InventoryItem inventoryItem;
    public override void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    {
#if UNITY_EDITOR
        DropdownField inventoryDropdown = new DropdownField();
        inventoryDropdown.choices = Resources.Load<InventoryList>("Inventory").GetListOfItemsNames();
        if (inventoryItem != null)
            inventoryDropdown.value = inventoryItem.itemName;
        inventoryDropdown.RegisterCallback<ChangeEvent<string>>((evt) => OnChangeItem(evt, interaction));
        visualElement.Add(inventoryDropdown);
#endif
    }

    private void OnChangeItem(ChangeEvent<string> evt, LeafInteraction interaction)
    {
        InventoryItem[] items = Resources.Load<InventoryList>("Inventory").items;
        for (int i = 0; i < items.Length; i++)
        {
            if (evt.newValue == items[i].itemName)
            {
                inventoryItem = items[i]; break;
            }
        }
    }
}
