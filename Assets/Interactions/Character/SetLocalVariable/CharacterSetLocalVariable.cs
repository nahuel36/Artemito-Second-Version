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
    public CustomEnumFlags<PropertyObjectType> customFlagsPO;
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



        if(character != null) 
        { 
            for (int i = 0; i < character.local_properties.Count; i++)
            {
                if (propertyToSet.name == character.local_properties[i].name)
                    propertyToSet = character.local_properties[i];
            }

            DropdownField propertyField = new DropdownField();
            propertyField.label = "Property";
            for (int i = 0; i < character.local_properties.Count; i++)
            {
                propertyField.choices.Add(character.local_properties[i].name);
            }
            propertyField.value = propertyToSet?.name;
            propertyField.RegisterValueChangedCallback((newvalue) =>
            {
                for (int i = 0; i < character.local_properties.Count; i++)
                {
                    if (newvalue.newValue == character.local_properties[i].name)
                        propertyToSet = character.local_properties[i];
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

        if (setMode == modes.copyOtherProperty)
        {
            objectTypeField.visible = true;

            StyleEnum<Position> pos = new StyleEnum<Position>();
            pos.value = Position.Relative;
            objectTypeField.style.position = pos;

            PropertyObjectType[] props = EnumerablesUtility.GetAllPropertyObjectTypes();

            objectTypeField.choices = new List<string>();
            for (int i = 0; i < props.Length; i++)
            {
                objectTypeField.choices.Add(props[i].TypeName);
            }
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
        else 
        {
            newElement.Q<VisualElement>("VariablesContainer").Clear();
        }
    }
}
