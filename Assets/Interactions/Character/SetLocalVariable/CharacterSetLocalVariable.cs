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
    public PropertyObjectType copyObjectContainer;
    //public string copyPropertyType;
    public GenericProperty copyPropertyVariable;
    public enum modes { 
        setValue,
        copyOtherProperty
    }
    public modes setMode;
    public override void ExecuteAction(List<InteractionProperty> properties, Interaction interaction)
    {
        if (setMode == modes.setValue)
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


        DropdownField propertyField = new DropdownField();
        propertyField.label = "Property";
        
        if(characterType != null)
            UpdateVariableChoices(propertyField);

        characterType.onCharacterChange += ()=>UpdateVariableChoices(propertyField);



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

            VisualElement newElement = new VisualElement();

            newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/SetProperty.uxml").CloneTree());

            //variable types no muestra los posibles valores de la property
            //everything solo debe cubrir las variables disponibles 

            if (propertyToSet != null && setMode == modes.copyOtherProperty)
                EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), new CustomEnumFlags<VariableType>[] { propertyToSet.variablesContainer, copyPropertyVariable.variablesContainer });
            else if (propertyToSet != null && setMode == modes.setValue)
                EnumerablesUtility.ShowEnumFlagsField("VariableTypes", newElement, customEnumFlags, () => EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags), new CustomEnumFlags<VariableType>[] { propertyToSet.variablesContainer});
        
            DropdownField setModeField = newElement.Q<DropdownField>("SetMode");
            setModeField.bindingPath = "setMode";
            setModeField.Bind(new SerializedObject(this));

            DropdownField objectTypeField = newElement.Q<DropdownField>("ObjectTypes");

            setModeField.RegisterValueChangedCallback(value => updateMode(newElement));
            updateMode(newElement);


            visualElement.Add(newElement);
        



        
#endif
    }

    void updateMode(VisualElement newElement)
    {
        newElement.Q<VisualElement>("VariablesContainer").Clear();

        if (setMode == modes.copyOtherProperty)
        {
            if (copyObjectContainer == null)
                copyObjectContainer = new PropertyObjectType();

            copyObjectContainer.SetGenericField(newElement.Q<VisualElement>("VariablesContainer"));

            //copyPropertyVariable = PropertyObjectType.GetGenericPropertyWithField(newElement.Q<VisualElement>("VariablesContainer"), variableOrigin);
            

            //((DialogOption)obj.value).subDialogs = new List<SubDialog>(); 

            /*
            EnumerablesUtility.ShowDropdownField(copyPropertyType, objectTypeField, ()=>
            {

                PropertyObjectType[] props = EnumerablesUtility.GetAllPropertyObjectTypes();

                copyPropertyType = objectTypeField.value;

                for (int i = 0; i < props.Length; i++)
                {
                    if (copyPropertyType == props[i].TypeName &&
                        (objectContainer == null || objectContainer.TypeName != props[i].TypeName))
                    {
                        objectContainer = (PropertyObjectType)props[i].Copy();
                    }
                }
                newElement.Q<VisualElement>("VariablesContainer").Clear();

                objectContainer.SetPropertyEditorField(newElement.Q<VisualElement>("VariablesContainer"));

                newElement.Q<VisualElement>("VariablesContainer").Q<ObjectField>().RegisterValueChangedCallback((evt) => 
                    {
                        
                        Debug.Log(evt.newValue.ToString()); 
                    }) ; 


                DropdownField dropdown = new DropdownField();
                List<LocalProperty> localProperties = objectContainer.GetLocalPropertys();
                for (int i = 0; i < localProperties.Count; i++)
                {
                    dropdown.choices.Add(localProperties[i].name);
                }
                dropdown.value = copyPropertyVariable.name;
                dropdown.RegisterValueChangedCallback((value) => {
                    List<LocalProperty> localProperties = objectContainer.GetLocalPropertys();
                    for (int i = 0; i < localProperties.Count; i++)
                    {
                        if(value.newValue == localProperties[i].name)
                            copyPropertyVariable = localProperties[i];
                    }}) ;
                newElement.Q<VisualElement>("VariablesContainer").Add(dropdown);
            });
              */

        }
        else
        {
            
            //objectTypeField.visible = false;
            //objectTypeField.StretchToParentSize();
        }

        if (setMode == modes.setValue)
        {
            EnumerablesUtility.UpdateAllVariablesFields(newElement, customEnumFlags);
        }
        
    }

    public override InteractionAction Copy()
    {
        CharacterSetLocalVariable action = new CharacterSetLocalVariable();
        action.characterType = (CharacterType)characterType.Copy();
        action.propertyToSet = propertyToSet;
        if(copyObjectContainer != null)
            action.copyObjectContainer = (PropertyObjectType)copyObjectContainer.Copy();
        action.setMode = setMode;
        //action.copyPropertyType = copyPropertyType;
        action.copyPropertyVariable = copyPropertyVariable;
        action.customEnumFlags = customEnumFlags.Copy();
        return action;
    }
}
