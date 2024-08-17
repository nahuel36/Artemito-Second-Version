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
    public CustomEnumFlags customEnumFlags;
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


    private void UpdateVariableNameChoices(DropdownField propertyField) {
        propertyField.choices = new List<string>();
        propertyField.value = null;
        if (characterType.character!=null)
        { 
            for (int i = 0; i < characterType.character.local_properties.Count; i++)
            {
                propertyField.choices.Add(characterType.character.local_properties[i].name);
                if(characterType.character.local_properties[i].name == propertyToSet?.name)
                    propertyField.value = propertyToSet?.name;
            }
        }
    }

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);


        VisualElement newElement = new VisualElement();

        newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/ChangeProperty.uxml").CloneTree());


        DropdownField propertyField = newElement.Q<DropdownField>("Property");
        
        if(characterType != null)
            UpdateVariableNameChoices(propertyField);

        characterType.onPropertyEditorChange += () =>
            {
                UpdateVariableNameChoices(propertyField);
                UpdateVariableTypes(newElement);
            };


            propertyField.RegisterValueChangedCallback((newvalue) =>
            {
                if (characterType.character != null)
                    for (int i = 0; i < characterType.character.local_properties.Count; i++)
                    {
                        if (newvalue.newValue == characterType.character.local_properties[i].name)
                            propertyToSet = characterType.character.local_properties[i];
                    }
                UpdateVariableTypes(newElement);
            });
            visualElement.Add(propertyField);

            if (customEnumFlags == null)
            {
                customEnumFlags = new CustomEnumFlags(0);
                customEnumFlags.type = CustomEnumFlags.contentType.variable;
            }


        UpdateVariableTypes(newElement);

            DropdownField changeModeField = newElement.Q<DropdownField>("ChangeMode");
            changeModeField.bindingPath = "changeMode";
            changeModeField.Bind(new SerializedObject(this));

            

            changeModeField.RegisterValueChangedCallback(value => updateMode(newElement));
            updateMode(newElement);


            visualElement.Add(newElement);
        



        
#endif
    }

    void UpdateVariableTypes(VisualElement newElement )
    {
        if (propertyToSet != null && changeMode == modes.copyOtherProperty && copyPropertyVariable != null)
            EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => { }, new CustomEnumFlags[] { propertyToSet.variablesContainer, copyPropertyVariable.variablesContainer });
        else if (propertyToSet != null && changeMode == modes.setValue)
            EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), new CustomEnumFlags[] { propertyToSet.variablesContainer });
        else   
            newElement.Q<EnumFlagsField>("VariableTypes").choices = new List<string>();
    }



    void updateMode(VisualElement newElement)
    {
        VisualElement copyModeVE = newElement.Q<VisualElement>("CopyMode");
        VisualElement setModeVE = newElement.Q<VisualElement>("SetMode");


        if (changeMode == modes.copyOtherProperty)
        {
            setModeVE.Q<VisualElement>("VariablesContainer").Clear();

            VisualElementsUtils.HideVisualElement(setModeVE);
            VisualElementsUtils.ShowVisualElement(copyModeVE);

            DropdownField objectTypeField = copyModeVE.Q<DropdownField>("ObjectType");
            VisualElement objectField = copyModeVE.Q<VisualElement>("ObjectField");
            DropdownField variables = copyModeVE.Q<DropdownField>("Variables");

            EnumerablesUtility.ShowDropdownField(copyPropertyType, objectTypeField, ()=>
            {
                copyPropertyType = objectTypeField.value;

                UpdateCopyPropertyObjectContainer();

                objectField.Clear();

                if (copyPropertyType != null && copyPropertyObjectContainer != null)
                { 

                    copyPropertyObjectContainer.SetPropertyEditorField(objectField);

                    copyPropertyObjectContainer.onPropertyEditorChange += () =>
                    {
                        //copyPropertyVariable = null;//no funciona porque al inicio se ejecuta
                            UpdateDropdownCopyVariables(newElement, variables);
                    };

                    UpdateDropdownCopyVariables(newElement, variables);

                }
            });
                

        }
        else
        {
            VisualElementsUtils.HideVisualElement(copyModeVE);
            VisualElementsUtils.ShowVisualElement(setModeVE);
            EnumerablesUtility.UpdateAllVariablesFields(setModeVE, customEnumFlags);
        }        
    }

    private void UpdateDropdownCopyVariables(VisualElement rootElement, VisualElement variablesContainer)
    {
        variablesContainer.Clear();
        DropdownField dropdown = new DropdownField();
        dropdown.value = null;
        dropdown.label = "Property to copy";
        List<LocalProperty> localProperties = copyPropertyObjectContainer.GetLocalPropertys();
        if (localProperties != null && localProperties.Count > 0)
        {
            string localPropertyToCopy = string.Empty;
            for (int i = 0; i < localProperties.Count; i++)
            {
                dropdown.choices.Add(localProperties[i].name);
                if (copyPropertyVariable != null && localProperties[i].name == copyPropertyVariable.name)
                {
                    localPropertyToCopy = copyPropertyVariable.name;
                    copyPropertyVariable = localProperties[i];
                }
            }

            if(!string.IsNullOrEmpty(localPropertyToCopy))
                dropdown.value = localPropertyToCopy;// localPropertyToCopy;

            dropdown.RegisterValueChangedCallback((value) => {
                List<LocalProperty> localProperties = copyPropertyObjectContainer.GetLocalPropertys();
                for (int i = 0; i < localProperties.Count; i++)
                {
                    if (value.newValue == localProperties[i].name)
                    { 
                        copyPropertyVariable = localProperties[i];
                        UpdateVariableTypes(rootElement);
                    }
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
