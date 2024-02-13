using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class SelectInventory : SubtypeSelector
{
    [SerializeField] VisualTreeAsset SelectInventoryVisualTree;
    public VisualElement VisualElements(Interaction interaction)
    {
        // Each editor window contains a root VisualElement object
        // Import UXML
        VisualElement root = SelectInventoryVisualTree.CloneTree();

        DropdownField inventoryDropdown = root.Q<DropdownField>("InventoryList");
        inventoryDropdown.choices = Resources.Load<InventoryList>("Inventory").GetListOfItems();
        if(!string.IsNullOrEmpty(interaction.subtypeObject))
            inventoryDropdown.value = interaction.SubtypeObjectToInventoryName(interaction.subtypeObject);
        inventoryDropdown.RegisterCallback<ChangeEvent<string>>((evt) => OnChangeItem(evt, interaction));

        return root;
    }

    
    private void OnChangeItem(ChangeEvent<string> evt, Interaction interaction)
    {
        interaction.subtypeObject = interaction.InventoryNameToSubtypeObject(evt.newValue);
    }
}