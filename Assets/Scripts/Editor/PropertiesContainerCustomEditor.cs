using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;
using System;

[CustomEditor(typeof(PropertiesContainer))]
public class PropertiesContainerCustomEditor : Editor
{
    [SerializeField] VisualTreeAsset generalTree;

    public override VisualElement CreateInspectorGUI()
    {
        /*VisualElement root = new VisualElement();

        root.Add(generalTree.CloneTree());

        LocalAndGlobalProperties properties = new LocalAndGlobalProperties();

        properties.CreateGUI(((PropertiesContainer)target).local_properties, root.Q("LocalAndGlobalProperties"));

        return root;*/
        return EnumFlagsTest();
    }

    public VisualElement EnumFlagsTest()
    {
        VisualElement root = new VisualElement();

        //((PropertiesContainer)target).local_properties[0].variableTypes = new CustomEnumFlags<VariableType>(VariableTypesUtility.GetAllVariableTypes(), 0);

        VisualElement element = VariableTypesUtility.ShowEnumFlagsField(((PropertiesContainer)target).local_properties[0].variableTypes);

        element.RegisterCallback<ChangeEvent<string>>( (evt) => SaveTargetChanges());

        root.Add(element);

        return root;
    }

    private void SaveTargetChanges()
    {
        Debug.Log(((PropertiesContainer)target).local_properties[0].variableTypes.GetIntValue());
        EditorUtility.SetDirty(target);
    }
}
