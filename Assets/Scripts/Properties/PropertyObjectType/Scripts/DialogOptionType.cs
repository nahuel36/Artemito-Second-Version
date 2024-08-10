using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class DialogOptionType : PropertyObjectType
{
    public override event ChangePropertyEditorField onPropertyEditorChange;
    public Dialog dialog;
    public int subDialog;
    public int dialogOption;
    public DialogOptionType()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }

    private void OnEnable()
    {
        Index = 3;
        TypeName = "Dialog Option";
    }

    public override void SetPropertyEditorField(VisualElement element)
    {
        base.SetPropertyEditorField(element);

        ObjectField dialogField = new ObjectField();
        DropdownField subdialogField = new DropdownField();
        DropdownField optionField = new DropdownField();

        dialogField.label = "Dialog";
        dialogField.objectType = typeof(Dialog);
        dialogField.bindingPath = "dialog";
        dialogField.Bind(new SerializedObject(this));
        dialogField.RegisterValueChangedCallback((value) =>
        {
            UpdateSubdialogs(subdialogField);
        });
        element.Add(dialogField);

        subdialogField.label = "subdialog";
        subdialogField.RegisterValueChangedCallback(value =>
        {
            UpdateOptions(optionField);
        });
        element.Add(subdialogField);

        optionField.label = "option";
        optionField.RegisterValueChangedCallback(value =>
        {
            onPropertyEditorChange?.Invoke();
        });
        element.Add(optionField);

        UpdateSubdialogs(subdialogField);
        UpdateOptions(optionField);
    }

    private void UpdateSubdialogs(DropdownField subDialogsField)
    {
        subDialogsField.choices = new List<string>();
        if (dialog == null) return;
        for (int i = 0; i < dialog.subDialogs.Count; i++)
        {
            subDialogsField.choices.Add(dialog.subDialogs[i].text);
        }
    }

    private void UpdateOptions(DropdownField subDialogsField)
    {
        subDialogsField.choices = new List<string>();
        if (dialog == null || subDialog < 0) return;
        for (int i = 0; i < dialog.subDialogs[subDialog].options.Count; i++)
        {
            subDialogsField.choices.Add(dialog.subDialogs[subDialog].options[i].initialText);
        }
    }

    public override List<LocalProperty> GetLocalPropertys()
    {
        if (dialog == null || subDialog < 0 || subDialog >= dialog.subDialogs.Count
          || dialogOption < 0 || dialogOption >= dialog.subDialogs[subDialog].options.Count)
            return null;

        return dialog.subDialogs[subDialog].options[dialogOption].local_properties;
    }
    public override EnumerableType Copy()
    {
        DialogOptionType dialogOptionType = Instantiate(this);
        dialogOptionType.Index = Index;
        dialogOptionType.TypeName = TypeName;
        return dialogOptionType;
    }
}
