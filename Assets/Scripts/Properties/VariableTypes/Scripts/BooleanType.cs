using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanType: VariableType
{
    public BooleanType() {
        typeName = "boolean";
        Index = 0;
        isString = true;
    }

    private void OnEnable()
    {
        typeName = "boolean";
        Index = 0;
        isString = true;
    }

    public override void SetPropertyField(VisualElement root)
    {
        base.SetPropertyField(root);

        VisualElement element = root.Q("Field");

        Toggle field = new Toggle();
               
        field.value = GetVariableValue();

        field.RegisterValueChangedCallback((evt) => SetVariableValue(evt.newValue));

        element.Add(field);
    }

    public bool GetVariableValue()
    {
        if (string.IsNullOrEmpty(stringValue))
            return false;

        if (stringValue.ToLower() == "true")
            return true;
        else
            return false;
    }

    public void SetVariableValue(bool value)
    {
        stringValue = value.ToString().ToLower();
    }

    public override EnumerableType Copy()
    {
        BooleanType newEnum = new BooleanType();
        newEnum.isDefaultValue = isDefaultValue;
        return newEnum;
    }
}
