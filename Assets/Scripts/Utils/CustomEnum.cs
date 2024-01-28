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

public enum GenericEnum
{
    value1 = 1 << 0,
    value2 = 1 << 1,
    value3 = 1 << 2,
    value4 = 1 << 3,
    value5 = 1 << 4,
    value6 = 1 << 5,
    value7 = 1 << 6,
    value8 = 1 << 7,
    value9 = 1 << 8,
    value10 = 1 << 9,
    value11 = 1 << 10,
    value12 = 1 << 11,
    value13 = 1 << 12,
    value14 = 1 << 13,
    value15 = 1 << 14,
    value16 = 1 << 15,
    value17 = 1 << 16,
    value18 = 1 << 17,
    value19 = 1 << 18,
    value20 = 1 << 19,
    value21 = 1 << 20,
    value22 = 1 << 21,
    value23 = 1 << 22,
    value24 = 1 << 23,
    value25 = 1 << 24,
    value26 = 1 << 25,
    value27 = 1 << 26,
    value28 = 1 << 27,
    value29 = 1 << 28,
    value30 = 1 << 29
    value31 = 1 << 30,
    value32 = 1 << 31,
    value33 = 1 << 32,
    value34 = 1 << 33,
    value35 = 1 << 34,
    value36 = 1 << 35,
    value37 = 1 << 36,
    value38 = 1 << 37,
    value39 = 1 << 38,
    value40 = 1 << 39,
    value41 = 1 << 40,
    value42 = 1 << 41,
    value43 = 1 << 42,
    value44 = 1 << 43,
    value45 = 1 << 44,
    value46 = 1 << 45,
    value47 = 1 << 46,
    value48 = 1 << 47,
    value49 = 1 << 48,
    value50 = 1 << 49,
}