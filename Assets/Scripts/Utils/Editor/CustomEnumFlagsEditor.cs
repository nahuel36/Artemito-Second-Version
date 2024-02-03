using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using System;

public class CustomEnumFlagsEditor<T> :Editor where T:EnumerableType
{
    private EnumFlagsField field;
    public List<string> choices = new List<string>();
    public List<int> choicesMasks = new List<int>();
    public VisualElement Show(CustomEnumFlags<T> value)
    {
        field = new EnumFlagsField((GenericEnum)value.GetIntValue());
        field.choices = choices;
        field.choicesMasks = choicesMasks;
        field.RegisterValueChangedCallback((evt) => callback(evt, value));
        return field;
    }

    private void callback(ChangeEvent<Enum> evt, CustomEnumFlags<T> value)
    {
        value.SetIntValue(Convert.ToInt32(evt.newValue));
    }
}
