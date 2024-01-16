using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MultiSelectionEnumField : VisualElement 
{
    private HashSet<string> selectedOptions = new HashSet<string>();
    private Label label;
    private List<string> options;

    public MultiSelectionEnumField(string label, List<string> options)
    {
        this.options = options;
        this.label = new Label(label);
        this.Add(this.label);

        var button = new Button(() => ShowMenu());
        button.text = "Select Options";
        this.Add(button);
    }

    private void ShowMenu()
    {
        var menu = new GenericMenu();

        foreach (string option in options)
        {
            var isSelected = selectedOptions.Contains(option);
            menu.AddItem(new GUIContent(option.ToString()), isSelected, () => ToggleOption(option));
        }

        menu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
    }

    private void ToggleOption(string option)
    {
        if (selectedOptions.Contains(option))
        {
            selectedOptions.Remove(option);
        }
        else
        {
            selectedOptions.Add(option);
        }

        UpdateLabel();
    }

    private void UpdateLabel()
    {
        label.text = "Selected Options: " + string.Join(", ", selectedOptions);
    }
}