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
        if (value.type == CustomEnumFlags.contentType.variable)
        {
            List<int> indexs = new List<int>();
            for (int i = 0; i < field.choices.Count; i++)
            {
                if ((Convert.ToInt32((GenericEnum)evt.newValue) & (1 << i)) != 0)
                    indexs.Add(i);
            }
            int finalValue = 0;
            VariableType[] variables = EnumerablesUtility.GetAllVariableTypes();
            for (int i = 0; i < indexs.Count; i++)
            {
                for (int j = 0; j < variables.Length; j++)
                {
                    if (field.choices[indexs[i]] == variables[j].TypeName)
                    {
                        finalValue = finalValue | (1 << variables[j].Index);
                    }
                }
            }
            value.SetIntValue(finalValue);
        }
        else
        { 
            value.SetIntValue(Convert.ToInt32((GenericEnum)evt.newValue));
        }
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
