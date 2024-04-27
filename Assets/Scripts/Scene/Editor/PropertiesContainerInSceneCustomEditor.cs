using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.UIElements;
using System;

[CustomEditor(typeof(PropertiesContainerInScene))]
public class PropertiesContainerInSceneCustomEditor : Editor
{
    [SerializeField] VisualTreeAsset generalTree;

    public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        root.Add(generalTree.CloneTree());

        LocalAndGlobalProperties properties = (LocalAndGlobalProperties)CreateInstance(typeof(LocalAndGlobalProperties));

        properties.CreateGUI(((PropertiesContainerInScene)target).local_properties, root.Q("LocalAndGlobalProperties"));

        root.RegisterCallback<ChangeEvent<string>>((evt) => SaveTargetChanges());

        return root;
    }

    private void SaveTargetChanges()
    {
        EditorUtility.SetDirty(target);
    }
}
