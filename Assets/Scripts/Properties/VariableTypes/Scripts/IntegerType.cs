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
        isString = true;
    }

    private void OnEnable()
    {
        typeName = "integer";
        Index = 1;
        isString = true;
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
        int integerValue = -1; 
        if(int.TryParse(stringValue, out integerValue))
            return integerValue;
        return -1;
    }

    public void SetVariableValue(GenericProperty property, int value)
    {
        stringValue = value.ToString();
    }

    public override EnumerableType Copy()
    {
        IntegerType newEnum = new IntegerType();
        newEnum.isDefaultValue = isDefaultValue;
        return newEnum;
    }
}
