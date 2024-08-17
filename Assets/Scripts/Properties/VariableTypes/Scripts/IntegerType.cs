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
        if(int.TryParse(stringValue, out integerValue))
            return integerValue;
        return -1;
    }

    public void SetVariableValue(int value)
    {
        stringValue = value.ToString();

        onChangeAVariableContentValue?.Invoke();
    }

    public override EnumerableType Copy()
    {
        IntegerType newEnum = new IntegerType();
        newEnum.isDefaultValue = isDefaultValue;
        newEnum.typeName = typeName;
        newEnum.Index = Index;
        newEnum.isString = isString;
        newEnum.stringValue = stringValue;
        return newEnum;
    }
}
