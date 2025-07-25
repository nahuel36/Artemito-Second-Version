using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif
using UnityEngine.UIElements;
using System;

[System.Serializable]
public class CustomEnumFlagsEditor
{
#if UNITY_EDITOR

    private EnumFlagsField field;
    public List<string> choices = new List<string>();
    public List<int> choicesMasks = new List<int>();
    public void Show(string enumFieldName, CustomEnumFlags value, VisualElement element, Action OnChange = null)
    {
        field = element.Q<EnumFlagsField>(enumFieldName);
        field.choices = choices;
        field.choicesMasks = choicesMasks;
        field.value = (GenericEnum)value.GetIntValue();
        value.Enumfield = field;
        field.RegisterValueChangedCallback((evt) => callback(evt, value));
        value.OnValueChange += () => {
            OnChange?.Invoke(); };
    }

    private void callback(ChangeEvent<Enum> evt, CustomEnumFlags value)
    {
        value.SetIntValue(Convert.ToInt32((GenericEnum)evt.newValue));
    }

    public void SetChoices(Func<int, string> func, int Lenght)
    {
        choices.Clear();

        for (int i = 0; i < Lenght; i++)
        {
            if(!string.IsNullOrEmpty(func(i)))
                choices.Add(func(i));
        }
    }


#endif
}
