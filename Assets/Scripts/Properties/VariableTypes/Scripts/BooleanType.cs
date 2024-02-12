using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanType: VariableType
{
    public BooleanType() {
        typeName = "boolean";
        Index = 0;
    }

    private void OnEnable()
    {
        typeName = "boolean";
        Index = 0;
    }

    public override void SetPropertyField(VisualElement root, GenericProperty property)
    {
        base.SetPropertyField(root, property);

        VisualElement element = root.Q("Field");

        Toggle field = new Toggle();
               
        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property, evt.newValue));

        element.Add(field);
    }

    public bool GetVariableValue(GenericProperty property)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return false;

        if (string.IsNullOrEmpty(property.variableValues[Index]))
            return false;

        if (property.variableValues[Index].ToLower() == "true")
            return true;
        else
            return false;
    }

    public void SetVariableValue(GenericProperty property, bool value)
    {
        property.variableValues[Index] = value.ToString().ToLower();
    }
}
