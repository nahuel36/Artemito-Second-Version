using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class LocalAndGlobalProperties : Editor
{
    [SerializeField]
    private VisualTreeAsset generalTree = default;
    [SerializeField]
    VisualTreeAsset variableItem;
    [SerializeField]
    VisualTreeAsset localProperty;

    public VisualElement CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = new VisualElement();

        // Instantiate UXML
        VisualElement labelFromUXML = generalTree.Instantiate();
        root.Add(labelFromUXML);

        return root;
    }
}
