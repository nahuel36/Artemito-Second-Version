using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StringType : VariableType
{
    public StringType()
    {
        typeName = "string";
        Index = 2;
    }

    private void OnEnable()
    {
        typeName = "string";
        Index = 2;
    }

    public override void SetPropertyField(VisualElement element, GenericProperty property)
    {
        base.SetPropertyField(element, property);

        TextField field = new TextField();
       
        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property,evt.newValue));
        
        element.Add(field);
    }

    public string GetVariableValue(GenericProperty property)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return "";

        return property.variableValues[Index];
    }

    public void SetVariableValue(GenericProperty property,string value)
    {
        property.variableValues[Index] = value;
    }
}
