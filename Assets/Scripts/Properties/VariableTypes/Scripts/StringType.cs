using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class StringType : VariableType
{
    [SerializeField]public string value = "";

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

    public override void SetPropertyField(VisualElement root, GenericProperty property)
    {
        base.SetPropertyField(root, property);

        VisualElement element = root.Q("Field");

        TextField field = new TextField();
       
        field.value = GetVariableValue(property);

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property,evt.newValue));
        
        element.Add(field);
    }

    public string GetVariableValue(GenericProperty property)
    {
       // if (property.variableValues == null || Index >= property.variableValues.Length)
         //   return "";

        return value;
    }

    public void SetVariableValue(GenericProperty property,string value)
    {
        this.value = value;
    }

    public override EnumerableType Copy() {
        StringType newEnum = new StringType();
        newEnum.value = value;
        newEnum.isDefaultValue = isDefaultValue;
        return newEnum;
    }
}
