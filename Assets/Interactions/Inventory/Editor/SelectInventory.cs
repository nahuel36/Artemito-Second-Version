using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class SelectInventory : SubtypeSelector
{
    VisualElement root;
    bool added;
    public SelectInventory()
    {
        added = false;
        root = new VisualElement();
    }

    public VisualElement VisualElements(Interaction interaction, VisualElement element)
    {
        // Each editor window contains a root VisualElement object
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Inventory/Editor/SelectInventory.uxml");
        VisualElement labelFromUXML = visualTree.CloneTree();

        DropdownField inventoryDropdown = labelFromUXML.Q<DropdownField>("InventoryList");
        inventoryDropdown.choices = Resources.Load<InventoryList>("Inventory").GetListOfItems();
        if(!string.IsNullOrEmpty(interaction.subtypeObject))
        inventoryDropdown.value = interaction.SubtypeObjectToInventoryName(interaction.subtypeObject);
        inventoryDropdown.RegisterCallback<ChangeEvent<string>>((evt) => OnChangeItem(evt, interaction));

        root = labelFromUXML;

        return root;
    }

    public void Add(Interaction interaction, VisualElement element) {
        if (added) return;
        added = true;
        element.Add(VisualElements(interaction, element));
    }

    public void Remove(VisualElement element)
    {
        added = false;
        if(element.Contains(root))
            element.Remove(root);
    }

    private void OnChangeItem(ChangeEvent<string> evt, Interaction interaction)
    {
        interaction.subtypeObject = interaction.InventoryNameToSubtypeObject(evt.newValue);
    }
}