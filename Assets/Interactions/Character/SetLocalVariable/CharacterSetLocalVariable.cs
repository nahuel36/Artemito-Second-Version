using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Linq;

using UnityEditor;
using Unity.Collections.LowLevel.Unsafe;


#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterSetLocalVariable : CharacterInteraction
{
    public LocalProperty propertyToSet;
    public CustomEnumFlags<VariableType> customEnumFlags;
    public PropertyObjectType objectContainer;
    public enum modes { 
        setValue,
        copyOtherProperty,
        setDefaultMode
    }
    public modes setMode;
    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        propertyToSet.variablesContainer.SetValue("string", customEnumFlags.GetStringValue("string"));
    }

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);



        if(characterType != null && characterType.character != null) 
        { 
            for (int i = 0; i < characterType.character.local_properties.Count; i++)
            {
                if (propertyToSet.name == characterType.character.local_properties[i].name)
                    propertyToSet = characterType.character.local_properties[i];
            }

            DropdownField propertyField = new DropdownField();
            propertyField.label = "Property";
            for (int i = 0; i < characterType.character.local_properties.Count; i++)
            {
                propertyField.choices.Add(characterType.character.local_properties[i].name);
            }
            propertyField.value = propertyToSet?.name;
            propertyField.RegisterValueChangedCallback((newvalue) =>
            {
                for (int i = 0; i < characterType.character.local_properties.Count; i++)
                {
                    if (newvalue.newValue == characterType.character.local_properties[i].name)
                        propertyToSet = characterType.character.local_properties[i];
                }
            });
            visualElement.Add(propertyField);

            if (customEnumFlags == null)
                customEnumFlags = new CustomEnumFlags<VariableType>(0);

            VisualElement newElement = new VisualElement();

            newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/SetProperty.uxml").CloneTree());

            if (propertyToSet != null)
                EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), propertyToSet.variablesContainer);

            DropdownField setModeField = newElement.Q<DropdownField>("SetMode");
            setModeField.bindingPath = "setMode";
            setModeField.Bind(new SerializedObject(this));

            DropdownField objectTypeField = newElement.Q<DropdownField>("ObjectTypes");

            setModeField.RegisterValueChangedCallback(value => updateMode(newElement, objectTypeField));
            updateMode(newElement, objectTypeField);


            visualElement.Add(newElement);
        }



        
#endif
    }

    void updateMode(VisualElement newElement, DropdownField objectTypeField)
    {
        newElement.Q<VisualElement>("VariablesContainer").Clear();

        if (setMode == modes.copyOtherProperty)
        {
            objectTypeField.visible = true;

            StyleEnum<Position> pos = new StyleEnum<Position>();
            pos.value = Position.Relative;
            objectTypeField.style.position = pos;

            EnumerablesUtility.ShowDropdownField(objectTypeField, ref objectContainer);

            if(objectContainer != null)
                objectContainer.SetPropertyEditorField(newElement.Q<VisualElement>("VariablesContainer"));

        }
        else
        {
            
            objectTypeField.visible = false;
            objectTypeField.StretchToParentSize();
        }

        if (setMode == modes.setValue)
        {
            EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags);
        }
        
    }
}
