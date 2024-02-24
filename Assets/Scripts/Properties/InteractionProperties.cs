using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
public class InteractionProperties : Editor
{
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset interaction_property;
    CustomListView<InteractionProperty> customListView;
    public VisualElement CreateGUI(List<InteractionProperty> local_properties, VisualElement root)
    {
        // Each editor window contains a root VisualElement object
        // Instantiate UXML


        if(customListView == null)
            customListView = new CustomListView<InteractionProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties[i]);

        customListView.ItemHeight = (i) => { return new StyleLength(StyleKeyword.Auto); };

        customListView.OnAdd = () => {
            int variablesLength = VariableTypesUtility.GetAllVariableTypes().Length;
            InteractionProperty localprop = new InteractionProperty();
            localprop.variableTypes = new CustomEnumFlags<VariableType>(0);
            localprop.variableValues = new string[variablesLength];
            localprop.useDefaultValues = new bool[variablesLength];
            localprop.objectValues = new UnityEngine.Object[variablesLength];
            return localprop;        
        };

        customListView.Init(root, true);

        return root;
    }

    private VisualElement ItemContent(int index, InteractionProperty property)
    {
        VisualElement element = new VisualElement();

        //element.Add(interaction_property.CloneTree());
        element.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Properties/Editor/InteractionProperty.uxml").CloneTree());

        element.Q("VariableItem").visible = false;
        element.Q("VariableItem").StretchToParentSize();

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { property.name = name.newValue; });


        VariableTypesUtility.ShowEnumFlagsField(element, property.variableTypes);

        foreach (var variable in VariableTypesUtility.GetAllVariableTypes())
        {
            if (property.variableTypes.ContainsValue(variable))
            {
                VisualElement variableItemElement = variableItem.CloneTree();
                variableItemElement.Q<VisualElement>("Value").Q<Label>("Label").text = variable.TypeName;
                variable.SetPropertyField(variableItemElement,property);
                element.Add(variableItemElement);
            }
        }



        return element;
    }
}
#endif
