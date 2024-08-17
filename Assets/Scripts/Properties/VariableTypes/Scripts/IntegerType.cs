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
        data.isString = true;
    }

    private void OnEnable()
    {
        typeName = "integer";
        Index = 1;
        data.isString = true;
    }

    public override void SetPropertyField(VisualElement root)
    {
        base.SetPropertyField(root);

        VisualElement element = root.Q("Field");

        IntegerField field = new IntegerField();

        field.value = GetVariableValue();

        field.RegisterValueChangedCallback((evt) => SetVariableValue(evt.newValue));

        element.Add(field);
    }

    public int GetVariableValue()
    {
        int integerValue = -1; 
        if(int.TryParse(data.stringValue, out integerValue))
            return integerValue;
        return -1;
    }

    public void SetVariableValue(int value)
    {
        data.stringValue = value.ToString();

        onChangeAVariableContentValue?.Invoke();
    }

    public override EnumerableType Copy()
    {
        IntegerType newEnum = new IntegerType();
        newEnum.data.isDefaultValue = data.isDefaultValue;
        newEnum.typeName = typeName;
        newEnum.Index = Index;
        newEnum.data.isString = data.isString;
        newEnum.data.stringValue = data.stringValue;
        return newEnum;
    }
}
