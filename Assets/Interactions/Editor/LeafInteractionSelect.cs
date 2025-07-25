using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

public class LeafInteractionSelect : Editor
{
    LeafInteraction interaction;
    VisualElement labelFromUXML;
    [SerializeField] VisualTreeAsset interactionSelect;

    public delegate void OnSelectTypesEvents(LeafInteraction interactP);
    public event OnSelectTypesEvents OnChangeTypeEvent;
    public event OnSelectTypesEvents OnChangeSubTypeEvent;

    public VisualElement ShowAndConfigure(LeafInteraction interactionP) 
    {
        VisualElement root = new VisualElement();

        // Import UXML
        labelFromUXML = interactionSelect.Instantiate();

        DropdownField typeDropdown = labelFromUXML.Q<DropdownField>("type");
        typeDropdown.choices = FileUtils.GetDirList(Application.dataPath + "/Interactions/", true);

        root.Add(labelFromUXML);

        this.interaction = interactionP;
        typeDropdown.value = interaction.type;

        SetSubtypeChoices(false, labelFromUXML);
        typeDropdown.RegisterCallback<ChangeEvent<string>>((e) => OnChangeType(interaction, e, labelFromUXML));

        DropdownField subtype = labelFromUXML.Q<DropdownField>("subtype");
        subtype.RegisterCallback<ChangeEvent<string>>(OnChangeSubtype);
        if (string.IsNullOrEmpty(interaction.type))
            subtype.visible = false;


        return root;
    }

    

    private void SetSubtypeChoices(bool resetValue, VisualElement element)
    {
        if (String.IsNullOrEmpty(interaction.type)) return;

        DropdownField subtype = element.Q<DropdownField>("subtype");
        subtype.choices = FileUtils.GetDirList(Application.dataPath + "/Interactions/" + interaction.type + "/", true);
        if (resetValue)
            subtype.value = null;
        else
            subtype.value = interaction.subtype;

    }

    private void OnChangeType(LeafInteraction interactP, ChangeEvent<string> evt, VisualElement element)
    {
        interaction.type = evt.newValue;
        SetSubtypeChoices(true, element);
        OnChangeTypeEvent?.Invoke(interactP);
        labelFromUXML.Q<DropdownField>("subtype").visible = true;
    }

    private void OnChangeSubtype(ChangeEvent<string> evt)
    {
        if (String.IsNullOrEmpty(interaction.type)) return;

        interaction.subtype = evt.newValue;

        OnChangeSubTypeEvent?.Invoke(interaction);
    }

    

}