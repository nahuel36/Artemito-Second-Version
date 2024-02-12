using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using System;
public class EnumWithFlagsType : VariableType
{
    public EnumWithFlagsType()
    {
        typeName = "enum with flags";
        Index = 3;
    }

    private void OnEnable()
    {
        typeName = "enum with flags";
        Index = 3;
    }

    public override void SetPropertyField(VisualElement element, GenericProperty property)
    {
        base.SetPropertyField(element, property);
#if UNITY_EDITOR
        EnumFlagsField field = new EnumFlagsField((GenericEnum)0);

        field.value = (GenericEnum)GetVariableValue(property);

        field.choices = new List<string> () { "choice1", "choice2", "choice3" };

        field.choicesMasks = new List<int>() { 1 << 0, 1 << 1, 1 << 2 };

        field.RegisterValueChangedCallback((evt) => SetVariableValue(property,evt.newValue));
        
        element.Add(field);
#endif
    }

    public System.Enum GetVariableValue(GenericProperty property)
    {
        if (property.variableValues == null || Index >= property.variableValues.Length)
            return (GenericEnum)0;

        int integerValue = 0;
        if (int.TryParse(property.variableValues[Index], out integerValue))
            return (GenericEnum)integerValue;
        return (GenericEnum)0;
    }

    public void SetVariableValue(GenericProperty property,System.Enum value)
    {
        property.variableValues[Index] = (Convert.ToInt32((GenericEnum)value)).ToString();
    }
}
