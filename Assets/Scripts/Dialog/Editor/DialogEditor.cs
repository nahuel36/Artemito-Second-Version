using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    public override VisualElement CreateInspectorGUI()
    {
        Dialog thisDialog = ((Dialog)target);

        // Each editor window contains a root VisualElement object
        VisualElement root = new VisualElement();

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        labelFromUXML.Q<Label>("Tittle").text = "Dialog " + thisDialog.name;

        CustomListView<SubDialog> subDialogsList = new()
        {
            ItemsSource = thisDialog.subDialogs,

            ItemContent = (subdialogIndex) =>
            {
                VisualElement subdialogVE = new VisualElement();

                TextField subDialogTittleField = new TextField();

                subDialogTittleField.label = ("subdialog " + (subdialogIndex + 1).ToString());

                subDialogTittleField.value = thisDialog.subDialogs[subdialogIndex].text;

                subDialogTittleField.RegisterValueChangedCallback((value) => 
                    thisDialog.subDialogs[subdialogIndex].text = value.newValue);

                subdialogVE.Add(subDialogTittleField);

                CustomListView<DialogOption> optionsList = new()
                {
                    ItemsSource = thisDialog.subDialogs[subdialogIndex].options,

                    ItemContent = (optionIndex) =>
                    {
                        VisualElement optionVE = new VisualElement();

                        TextField optionTittleField = new TextField();

                        optionTittleField.label = ("option " + (optionIndex + 1).ToString());

                        optionTittleField.value = thisDialog.subDialogs[subdialogIndex].options[optionIndex].initialText;

                        optionTittleField.RegisterValueChangedCallback((value) => 
                            thisDialog.subDialogs[subdialogIndex].options[optionIndex].initialText = value.newValue);

                        optionVE.Add(optionTittleField);

                        return optionVE;
                    },

                    OnAdd = () => 
                    {
                        DialogOption newDialogOption = new DialogOption();
                        newDialogOption.initialText = "new option";
                        return newDialogOption;
                    }


                };

                optionsList.Init(subdialogVE, true);


                return subdialogVE;
            },

            OnAdd = () =>
            {
                SubDialog newSubDialog = new SubDialog();
                newSubDialog.text = "new subdialog";
                return newSubDialog;
            }
        };

        subDialogsList.Init(labelFromUXML.Q<VisualElement>("SubDialogs"), true);

        return root;
    }

}
