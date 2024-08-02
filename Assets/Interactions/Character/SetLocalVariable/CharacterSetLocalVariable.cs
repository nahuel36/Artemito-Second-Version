using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Linq;

using UnityEditor;
using Unity.Collections.LowLevel.Unsafe;
using System;



#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterSetLocalVariable : CharacterInteraction
{
    public LocalProperty propertyToSet;
    public CustomEnumFlags<VariableType> customEnumFlags;
    public PropertyObjectType copyPropertyObjectContainer;
    public string copyPropertyType;
    public GenericProperty copyPropertyVariable;
    public enum modes { 
        setValue,
        copyOtherProperty
    }
    public modes changeMode;
    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        if (changeMode == modes.setValue)
        {
            propertyToSet.variablesContainer.SetValue("string", customEnumFlags.GetStringValue("string"));
        }//FALTA EL DEFAULT
        else 
        {
            propertyToSet.variablesContainer.SetValue("string", copyPropertyVariable.variablesContainer.GetStringValue("string"));
        }
    }


    private void UpdateVariableChoices(DropdownField propertyField) {
        propertyField.choices = new List<string>();
        if(characterType.character!=null)
        { 
            for (int i = 0; i < characterType.character.local_properties.Count; i++)
            {
                propertyField.choices.Add(characterType.character.local_properties[i].name);
            }
            
            propertyField.value = propertyToSet?.name;
        }
        else
        {
            propertyField.value = null;
        }
            
    }

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);


        VisualElement newElement = new VisualElement();

        newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/SetProperty.uxml").CloneTree());


        DropdownField propertyField = newElement.Q<DropdownField>("Property");
        
        if(characterType != null)
            UpdateVariableChoices(propertyField);

        characterType.onPropertyEditorChange += ()=>UpdateVariableChoices(propertyField);



            propertyField.RegisterValueChangedCallback((newvalue) =>
            {
                if (characterType.character != null)
                    for (int i = 0; i < characterType.character.local_properties.Count; i++)
                    {
                        if (newvalue.newValue == characterType.character.local_properties[i].name)
                            propertyToSet = characterType.character.local_properties[i];
                    }
            });
            visualElement.Add(propertyField);

            if (customEnumFlags == null)
                customEnumFlags = new CustomEnumFlags<VariableType>(0);

            
            //variable types no muestra los posibles valores de la property
            //everything solo debe cubrir las variables disponibles 

            if (propertyToSet != null && changeMode == modes.copyOtherProperty)
                EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), new CustomEnumFlags<VariableType>[] { propertyToSet.variablesContainer, copyPropertyVariable.variablesContainer });
            else if (propertyToSet != null && changeMode == modes.setValue)
                EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), new CustomEnumFlags<VariableType>[] { propertyToSet.variablesContainer});
        
            DropdownField changeModeField = newElement.Q<DropdownField>("ChangeMode");
            changeModeField.bindingPath = "changeMode";
            changeModeField.Bind(new SerializedObject(this));

            

            changeModeField.RegisterValueChangedCallback(value => updateMode(newElement));
            updateMode(newElement);


            visualElement.Add(newElement);
        



        
#endif
    }

    void ShowVisualElement(VisualElement element)
    {
        element.visible = true;

        StyleEnum<Position> pos = new StyleEnum<Position>();
        pos.value = Position.Relative;
        element.style.position = pos;
    }

    void HideVisualElement(VisualElement element)
    {
        element.visible = false;
        element.StretchToParentSize();
    }

    void updateMode(VisualElement newElement)
    {
        VisualElement copyModeVE = newElement.Q<VisualElement>("CopyMode");
        VisualElement setModeVE = newElement.Q<VisualElement>("SetMode");


        if (changeMode == modes.copyOtherProperty)
        {
            setModeVE.Q<VisualElement>("VariablesContainer").Clear();

            HideVisualElement(setModeVE);
            ShowVisualElement(copyModeVE);

            DropdownField objectTypeField = copyModeVE.Q<DropdownField>("ObjectType");
            ObjectField objectField = copyModeVE.Q<ObjectField>("ObjectField");
            DropdownField variables = copyModeVE.Q<DropdownField>("Variables");

            EnumerablesUtility.ShowDropdownField(copyPropertyType, objectTypeField, ()=>
            {
                copyPropertyType = objectTypeField.value;

                UpdateCopyPropertyObjectContainer();

                objectField.Clear();
                
                copyPropertyObjectContainer.SetPropertyEditorField(objectField);

                copyPropertyObjectContainer.onPropertyEditorChange += ()=> UpdateDropdownCopyVariables(variables);

                UpdateDropdownCopyVariables(variables);

                //hacer un contenedor con variables container para cada modo
            });
                

        }
        else
        {
            HideVisualElement(copyModeVE);
            ShowVisualElement(setModeVE);
            EnumerablesUtility.UpdateAllVariablesFields(setModeVE, customEnumFlags);
        }        
    }

    private void UpdateDropdownCopyVariables(VisualElement variablesContainer)
    {
        variablesContainer.Clear();
        DropdownField dropdown = new DropdownField();
        dropdown.value = null;
        List<LocalProperty> localProperties = copyPropertyObjectContainer.GetLocalPropertys();
        if (localProperties != null && localProperties.Count > 0)
        {

            for (int i = 0; i < localProperties.Count; i++)
            {
                dropdown.choices.Add(localProperties[i].name);
            }

            dropdown.value = copyPropertyVariable.name;

            dropdown.RegisterValueChangedCallback((value) => {
                List<LocalProperty> localProperties = copyPropertyObjectContainer.GetLocalPropertys();
                for (int i = 0; i < localProperties.Count; i++)
                {
                    if (value.newValue == localProperties[i].name)
                        copyPropertyVariable = localProperties[i];
                }
            });
        }

        variablesContainer.Add(dropdown);
    }

    private void UpdateCopyPropertyObjectContainer()
    {
        PropertyObjectType[] props = EnumerablesUtility.GetAllPropertyObjectTypes();

        for (int i = 0; i < props.Length; i++)
        {
            if (copyPropertyType == props[i].TypeName &&
                (copyPropertyObjectContainer == null || copyPropertyObjectContainer.TypeName != props[i].TypeName))
            {
                copyPropertyObjectContainer = (PropertyObjectType)props[i].Copy();
            }
        }
    }

    public override InteractionAction Copy()
    {
        CharacterSetLocalVariable action = new CharacterSetLocalVariable();
        action.characterType = (CharacterType)characterType.Copy();
        action.propertyToSet = propertyToSet;
        if(copyPropertyObjectContainer != null)
            action.copyPropertyObjectContainer = (PropertyObjectType)copyPropertyObjectContainer.Copy();
        action.changeMode = changeMode;
        action.copyPropertyType = copyPropertyType;
        action.copyPropertyVariable = copyPropertyVariable;
        action.customEnumFlags = customEnumFlags.Copy();
        return action;
    }
}
