using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UIElements;
using System;

public class VariableTypesUtility 
{

    public static VariableType[] GetAllVariableTypes()
    {
#if UNITY_EDITOR
        List<string> files = FileUtils.GetFilesList(Application.dataPath + "/Scripts/Properties/VariableTypes/");
        int varIndex = 0;
        List<VariableType> list = new List<VariableType>();
        for (int i = 0; i < files.Count; i++)
        {
            VariableType var = AssetDatabase.LoadAssetAtPath<VariableType>("Assets/Scripts/Properties/VariableTypes/" + files[i]);
            if (var != null)
            {
                list.Add(var);
                varIndex++;
            }
        }
        list.Sort((x, y) => x.Index.CompareTo(y.Index));
        return list.ToArray();
#else
        return null;
#endif
    }

    public static void ShowEnumFlagsField(VisualElement element, CustomEnumFlags<VariableType> variableTypes, Action onChangeAction = null)
    {
        CustomEnumFlagsEditor<VariableType> customFlagsEditor = new CustomEnumFlagsEditor<VariableType>();

        VariableType[] list = GetAllVariableTypes();

        customFlagsEditor.SetChoices((i) => { return list[i].TypeName; }, list.Length);
        CustomEnumFlags<VariableType>.SetChoicesMasksByChoicesInOrder(customFlagsEditor.choicesMasks, customFlagsEditor.choices);

        customFlagsEditor.Show(variableTypes,element, false, onChangeAction);
    }
}
