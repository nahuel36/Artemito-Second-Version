using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEditor;
using UnityEngine.UIElements;
using System;

[System.Serializable]
public class CustomEnumFlagsEditor<T> where T:EnumerableType
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
        value.SetIntValue(Convert.ToInt32((GenericEnum)evt.newValue));
    }

    public void SetChoices(Func<int, string> func, int Lenght)
    {
        choices.Clear();

        for (int i = 0; i < Lenght; i++)
        {
            choices.Add(func(i));
        }
    }

    public void SetChoicesMasksByChoicesInOrder()
    {
        choicesMasks.Clear();

        for (int i = 0; i < choices.Count; i++)
        {
            choicesMasks.Add(1<<i);
        }
    }
}
