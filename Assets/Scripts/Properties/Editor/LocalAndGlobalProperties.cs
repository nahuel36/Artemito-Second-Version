using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class LocalAndGlobalProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset localProperty;
    CustomListView<LocalProperty> customListView;
    public VisualElement CreateGUI(List<LocalProperty> local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML

        if(customListView == null)
            customListView = new CustomListView<LocalProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties[i]);

        customListView.ItemHeight = (i) => { return new StyleLength(StyleKeyword.Auto); };

        customListView.OnAdd = () => {
            int variablesLength = VariableTypesUtility.GetAllVariableTypes().Length;
            LocalProperty localprop = new LocalProperty();
            localprop.variablesContainer = new CustomEnumFlags<VariableType>(0);
            localprop.variableValues = new string[variablesLength];
            localprop.useDefaultValues = new bool[variablesLength];
            localprop.objectValues = new UnityEngine.Object[variablesLength];
            return localprop;        
        };

        root.Q("LocalProperties").Q("LocalProperty").visible = false;
        root.Q("LocalProperties").Q("LocalProperty").StretchToParentSize();

        customListView.Init(root.Q("LocalProperties").Q("CustomListView"));

        StyleLength lenght = new StyleLength(StyleKeyword.Auto);

        root.Q("LocalProperties").Q("CustomListView").style.height = lenght;

        return root;
    }

    private VisualElement ItemContent(int index, LocalProperty property)
    {
        VisualElement element = new VisualElement();

        element.Add(localProperty.CloneTree());

        element.Q("VariableItem").visible = false;
        element.Q("VariableItem").StretchToParentSize();

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { property.name = name.newValue; });



        VariableTypesUtility.ShowEnumFlagsField(element,property.variablesContainer);
        
        foreach (var variable in VariableTypesUtility.GetAllVariableTypes())
        {
            if (property.variablesContainer.ContainsValue(variable))
            {
                VisualElement variableItemElement = variableItem.CloneTree();
                variableItemElement.Q<VisualElement>("Value").Q<Label>("Label").text = variable.TypeName;
                property.variablesContainer.SetPropertyField(variable, variableItemElement,property);
                property.variablesContainer.SetDefaultValue(variable, variableItemElement);
                element.Add(variableItemElement);
            }
        }
        

        return element;
    }
}
