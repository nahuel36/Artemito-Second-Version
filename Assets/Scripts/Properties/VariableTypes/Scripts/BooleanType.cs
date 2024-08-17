using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BooleanType: VariableType
{
    public BooleanType() {
        typeName = "boolean";
        Index = 0;
        data.isString = true;
    }

    private void OnEnable()
    {
        typeName = "boolean";
        Index = 0;
        data.isString = true;
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
        if (string.IsNullOrEmpty(data.stringValue))
            return false;

        if (data.stringValue.ToLower() == "true")
            return true;
        else
            return false;
    }

    public void SetVariableValue(bool value)
    {
        data.stringValue = value.ToString().ToLower();
        onChangeAVariableContentValue?.Invoke();
    }

    public override EnumerableType Copy()
    {
        BooleanType newEnum = new BooleanType();
        newEnum.typeName = typeName;
        newEnum.Index = Index;
        newEnum.data.stringValue = data.stringValue;
        newEnum.data.isDefaultValue = data.isDefaultValue;
        return newEnum;
    }
}
