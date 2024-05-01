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
        isString = true;
    }

    private void OnEnable()
    {
        typeName = "string";
        Index = 2;
        isString = true;
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

        return stringValue;
    }

    public void SetVariableValue(string value)
    {
        this.stringValue = value;
    }

    public override EnumerableType Copy() {
        StringType newEnum = new StringType();
        newEnum.stringValue = stringValue;
        newEnum.objectValue = objectValue;
        newEnum.isDefaultValue = isDefaultValue;
        return newEnum;
    }
}
