using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "StringType", menuName = "Pnc/PropertyVariablesType/StringType", order = 1)]
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
        return property.variableValue[Index];
    }

    public void SetVariableValue(GenericProperty property,string value)
    {
        property.variableValue[Index] = value;
    }
}
