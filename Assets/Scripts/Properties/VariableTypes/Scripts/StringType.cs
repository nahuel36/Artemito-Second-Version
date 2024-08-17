using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class StringType : VariableType
{
    public StringType()
    {
        typeName = "string";
        Index = 2;
        data.isString = true;
    }

    private void OnEnable()
    {
        typeName = "string";
        Index = 2;
        data.isString = true;
    }

    public override void SetPropertyField(VisualElement root)
    {
        base.SetPropertyField(root);

        VisualElement element = root.Q("Field");

        TextField field = new TextField();
       
        field.value = GetVariableValue();

        field.RegisterValueChangedCallback((evt) => SetVariableValue(evt.newValue));
        
        element.Add(field);
    }

    public string GetVariableValue()
    {
       // if (property.variableValues == null || Index >= property.variableValues.Length)
         //   return "";

        return data.stringValue;
    }

    public void SetVariableValue(string value)
    {
        this.data.stringValue = value;

        onChange?.Invoke();
        saveData?.Invoke();
    }

    public override EnumerableType Copy() {
        StringType newEnum = Object.Instantiate(this);
        newEnum.data.stringValue = data.stringValue;
        newEnum.data.objectValue = data.objectValue;
        newEnum.changedIngame = changedIngame;
        newEnum.data.isDefaultValue = data.isDefaultValue;
        return newEnum;
    }
}
