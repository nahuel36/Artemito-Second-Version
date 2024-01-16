using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class SelectInventory : SubtypeSelector
{

    public SelectInventory()
    {
    }

    public VisualElement VisualElements(Interaction interaction, VisualElement element)
    {
        VisualElement root = new VisualElement();
        // Each editor window contains a root VisualElement object
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Inventory/Editor/SelectInventory.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();

        DropdownField inventoryDropdown = labelFromUXML.Q<DropdownField>("InventoryList");
        inventoryDropdown.choices = Resources.Load<InventoryList>("Inventory").GetListOfItems();
        if(!string.IsNullOrEmpty(interaction.subtypeObject))
        inventoryDropdown.value = interaction.SubtypeObjectToInventoryName(interaction.subtypeObject);
        inventoryDropdown.RegisterCallback<ChangeEvent<string>>((evt) => OnChangeItem(evt, interaction));

        root.Add(labelFromUXML);



        return root;
    }

    private void OnChangeItem(ChangeEvent<string> evt, Interaction interaction)
    {
        interaction.subtypeObject = interaction.InventoryNameToSubtypeObject(evt.newValue);
    }
}