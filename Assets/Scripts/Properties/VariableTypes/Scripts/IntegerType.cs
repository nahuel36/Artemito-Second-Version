using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IntegerType : VariableType
{
    public IntegerType()
    {
        typeName = "integer";
        Index = 1;
    }

    private void OnEnable()
    {
        typeName = "integer";
        Index = 1;
    }

    public override void SetPropertyField(VisualElement root, GenericProperty property)
    {
        base.SetPropertyField(root, property);

        VisualElement element = root.Q("Field");

        IntegerField field = new IntegerField();

        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property, evt.newValue));

        element.Add(field);
    }

    public int GetVariableValue(GenericProperty property)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return -1;

        int integerValue = -1; 
        if(int.TryParse(property.variableValues[Index], out integerValue))
            return integerValue;
        return -1;
    }

    public void SetVariableValue(GenericProperty property, int value)
    {
        property.variableValues[Index] = value.ToString();
    }
}
