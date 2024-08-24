using System.Collections.Generic;
using System.Xml.Linq;

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

        Label tittle = new Label("LeafInteraction Properties");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        root.Add(tittle);

        if(customListView == null)
            customListView = new CustomListView<InteractionProperty>();

        customListView.ItemsSource = local_properties;

        customListView.ItemContent = (i) => ItemContent(i, local_properties[i]);

        customListView.OnAdd = () => {
            int variablesLength = EnumerablesUtility.GetAllVariableTypes().Length;
            InteractionProperty localprop = new InteractionProperty();
            localprop.variablesContainer = new CustomEnumFlags(0);
            localprop.variablesContainer.type = CustomEnumFlags.contentType.variable;
            return localprop;        
        };

        customListView.CopyItem = (propOrigin) =>
        { return (InteractionProperty)propOrigin.Copy(); };

        customListView.Init(root, true);

        return root;
    }

    

    private VisualElement ItemContent(int index, InteractionProperty property)
    {
        Foldout foldout = new Foldout();
        foldout.text = "property " + (index+1).ToString();


        VisualElement element = new VisualElement();

        //element.Add(interaction_property.CloneTree());
        element.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Properties/Editor/InteractionProperty.uxml").CloneTree());

        VisualElementsUtils.HideVisualElement(element.Q("VariableItem"));

        element.Q<TextField>("PropertyName").value = property.name;
        element.Q<TextField>("PropertyName").RegisterValueChangedCallback((name) => { property.name = name.newValue; });


        EnumerablesUtility.ShowEnumFlagsField("VariableTypes",element, property.variablesContainer, () => {
            EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer); });

        EnumerablesUtility.UpdateAllVariablesFields(element, property.variablesContainer);

        foldout.Add(element);

        return foldout;
    }
}
#endif
