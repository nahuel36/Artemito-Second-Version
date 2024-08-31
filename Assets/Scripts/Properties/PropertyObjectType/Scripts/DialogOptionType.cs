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
    public Dialog dialog { 
        get {
            CheckInitializedData();
            return data.unityObjects[0] as Dialog; } 
        set {
            CheckInitializedData();
            data.unityObjects[0] = value; } }
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

    public void CheckInitializedData()
    {
        if (data == null)
            data = new Data();
        if (data.unityObjects == null || data.unityObjects.Count < 1)
            data.unityObjects = new List<UnityEngine.Object> { new Character() };
    }

    public override void SetPropertyEditorField(VisualElement element)
    {
        base.SetPropertyEditorField(element);

        ObjectField dialogField = new ObjectField();
        DropdownField subdialogField = new DropdownField();
        DropdownField optionField = new DropdownField();

        dialogField.label = "Dialog";
        dialogField.objectType = typeof(Dialog);
        //dialogField.bindingPath = "dialog";
        //dialogField.Bind(new SerializedObject(this));
        if(dialog)
            dialogField.value = dialog;
        dialogField.RegisterValueChangedCallback((value) =>
        {
            dialog = value.newValue as Dialog;
            UpdateSubdialogs(subdialogField);
            saveData?.Invoke();
        });
        element.Add(dialogField);

        subdialogField.label = "subdialog";
        subdialogField.RegisterValueChangedCallback(value =>
        {
            UpdateOptions(optionField);

            for (int i = 0; i < dialog.subDialogs.Count; i++) 
            {
                if (dialog.subDialogs[i].text == value.newValue)
                    subDialog = i;
            }

        });
        element.Add(subdialogField);

        optionField.label = "option";
        optionField.RegisterValueChangedCallback(value =>
        {
            onPropertyEditorChange?.Invoke();
            if (dialog == null || subDialog < 0 || subDialog >= dialog.subDialogs.Count)
                return;

            for (int i = 0; i < dialog.subDialogs[subDialog].options.Count; i++)
            {
                if (dialog.subDialogs[subDialog].options[i].initialText == value.newValue)
                    dialogOption = i;
            }
        });
        element.Add(optionField);

        UpdateSubdialogs(subdialogField);
        UpdateOptions(optionField);
    }

    private void UpdateSubdialogs(DropdownField subDialogsField)
    {
        subDialogsField.choices = new List<string>();
        if (dialog == null) return;
        string subdialogvalue = null;
        for (int i = 0; i < dialog.subDialogs.Count; i++)
        {
            subDialogsField.choices.Add(dialog.subDialogs[i].text);
            if (i==subDialog)
                subdialogvalue = dialog.subDialogs[i].text;
        }
        subDialogsField.value = subdialogvalue;
    }

    private void UpdateOptions(DropdownField optionsField)
    {
        optionsField.choices = new List<string>();
        if (dialog == null || subDialog < 0) return;
        string optionvalue = null;
        for (int i = 0; i < dialog.subDialogs[subDialog].options.Count; i++)
        {
            optionsField.choices.Add(dialog.subDialogs[subDialog].options[i].initialText);
            if (i == dialogOption)
                optionvalue = dialog.subDialogs[subDialog].options[i].initialText;
        }
        optionsField.value = optionvalue;
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
        dialogOptionType.data = data;
        return dialogOptionType;
    }
}
