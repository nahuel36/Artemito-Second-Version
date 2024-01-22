using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

public class InteractionSelect 
{
    Interaction interaction;
    VisualElement labelFromUXML;
    public InteractionSelect()
    {        
    }

    public delegate void OnSelectTypesEvents(Interaction interactP);
    public event OnSelectTypesEvents OnChangeTypeEvent;

    public VisualElement BothFunctions(Interaction interactionP) 
    {
        VisualElement root = new VisualElement();

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/InteractionSelect.uxml");
        labelFromUXML = visualTree.Instantiate();

        DropdownField typeDropdown = labelFromUXML.Q<DropdownField>("type");
        typeDropdown.choices = FileUtils.GetDirList(Application.dataPath + "/Interactions/");
        typeDropdown.choices.Remove("Editor");

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

    public VisualElement VisualElements()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = new VisualElement();

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/InteractionSelect.uxml");
        labelFromUXML = visualTree.Instantiate();

        DropdownField typeDropdown = labelFromUXML.Q<DropdownField>("type");
        typeDropdown.choices = FileUtils.GetDirList(Application.dataPath + "/Interactions/");
        typeDropdown.choices.Remove("Editor");
        
        root.Add(labelFromUXML);

        return root;
    }

    public void Binding(Interaction interactionP, VisualElement element)
    {
        this.interaction = interactionP;
        DropdownField typeDropdown = element.Q<DropdownField>("type");
        typeDropdown.value = interaction.type;
        
        SetSubtypeChoices(false, element);
        typeDropdown.RegisterCallback<ChangeEvent<string>>((e) => OnChangeType(interaction, e, element));

        DropdownField subtype = element.Q<DropdownField>("subtype");
        subtype.RegisterCallback<ChangeEvent<string>>(OnChangeSubtype);

    }

    private void SetSubtypeChoices(bool resetValue, VisualElement element)
    {
        if (String.IsNullOrEmpty(interaction.type)) return;

        DropdownField subtype = element.Q<DropdownField>("subtype");
        subtype.choices = FileUtils.GetDirList(Application.dataPath + "/Interactions/" + interaction.type + "/");
        subtype.choices.Remove("Editor");
        if (resetValue)
            subtype.value = null;
        else
            subtype.value = interaction.subtype;

    }

    private void OnChangeType(Interaction interactP, ChangeEvent<string> evt, VisualElement element)
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
    }

    

}