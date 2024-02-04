using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalAndGlobalProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset localProperty;
    
    public VisualElement CreateGUI(LocalProperty[] local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML


        CustomListView<LocalProperty> customListView = new CustomListView<LocalProperty>();
        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties);

        customListView.ItemHeight = (i) => { return new StyleLength(StyleKeyword.Auto); };

        root.Q("LocalProperties").Q("LocalProperty").visible = false;
        root.Q("LocalProperties").Q("LocalProperty").StretchToParentSize();

        customListView.Init(root.Q("LocalProperties").Q("CustomListView"));

        StyleLength lenght = new StyleLength(StyleKeyword.Auto);

        root.Q("LocalProperties").Q("CustomListView").style.height = lenght;

        return root;
    }

    private VisualElement ItemContent(int index, GenericProperty[] property)
    {
        VisualElement element = new VisualElement();

        element.Add(localProperty.CloneTree());

        element.Q("VariableItem").visible = false;
        element.Q("VariableItem").StretchToParentSize();

        return element;
    }
}
