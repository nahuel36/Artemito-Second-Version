using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Dialog))]
public class DialogEditor : Editor
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField]
    private VisualTreeAsset m_OptionVisualTreeAsset = default;
    public override VisualElement CreateInspectorGUI()
    {
        Dialog thisDialog = ((Dialog)target);

        VisualElement root = new VisualElement();

        for (int i = 0; i < thisDialog.subDialogs.Count; i++)
        {
            for (int j = 0; j < thisDialog.subDialogs[i].options.Count; j++)
            {
                for (int k = 0; k < thisDialog.subDialogs[i].options[j].local_properties.Count; k++)
                {
                    if (thisDialog.isDuplicate)
                    {
                        //thisDialog.subDialogs[i].options[j].local_properties[k].variablesContainer = thisDialog.subDialogs[i].options[j].local_properties[k].variablesContainer.Copy();
                    }
                }
            }
        }
        thisDialog.isDuplicate = false;


        // Each editor window contains a root VisualElement object



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
                        VisualElement optionVE = m_OptionVisualTreeAsset.Instantiate();

                        TextField optionTittleField = optionVE.Q<TextField>("OptionTittle");

                        optionTittleField.label = ("option " + (optionIndex + 1).ToString());

                        optionTittleField.value = thisDialog.subDialogs[subdialogIndex].options[optionIndex].initialText;

                        optionTittleField.RegisterValueChangedCallback((value) =>
                            thisDialog.subDialogs[subdialogIndex].options[optionIndex].initialText = value.newValue);


                        LocalAndGlobalProperties.CreateGUI(thisDialog.subDialogs[subdialogIndex].options[optionIndex].local_properties, optionVE.Q("LocalAndGlobalProperties"), 
                        () =>
                        {
                            serializedObject.ApplyModifiedProperties();
                            EditorUtility.SetDirty(target);
                        }
                        );

                        VisualElement interVE = RootInteractionCustomEditor.ShowGUI(thisDialog.subDialogs[subdialogIndex].options[optionIndex].interactions.interactions, thisDialog, thisDialog.isDuplicate);
                        optionVE.Add(interVE);

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
