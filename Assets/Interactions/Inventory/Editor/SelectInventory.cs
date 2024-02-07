using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

public class SelectInventory : SubtypeSelector
{
    bool added;
    [SerializeField] VisualTreeAsset SelectInventoryVisualTree;
    VisualElement root;
    public void OnEnable()
    {
        added = false;
    }

    public VisualElement VisualElements(Interaction interaction, VisualElement element)
    {
        // Each editor window contains a root VisualElement object
        // Import UXML
        root = SelectInventoryVisualTree.CloneTree();

        DropdownField inventoryDropdown = root.Q<DropdownField>("InventoryList");
        inventoryDropdown.choices = Resources.Load<InventoryList>("Inventory").GetListOfItems();
        if(!string.IsNullOrEmpty(interaction.subtypeObject))
        inventoryDropdown.value = interaction.SubtypeObjectToInventoryName(interaction.subtypeObject);
        inventoryDropdown.RegisterCallback<ChangeEvent<string>>((evt) => OnChangeItem(evt, interaction));

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