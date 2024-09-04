using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Xml.Linq;
using UnityEditor;
using System;
using Unity.VisualScripting;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterSetLocalVariable : CharacterInteraction
{
    public GenericProperty propertyToSet { 
        get {
            CheckDataInitialized();
            return data.properties[0]; 
        }
        set {
            CheckDataInitialized();
            data.properties[0] = value;
        }    
    }
    public CustomEnumFlags customEnumFlags{
        get {
             CheckDataInitialized();
            return data.customEnumFlags[0]; 
        }
        set {
            CheckDataInitialized();
            data.customEnumFlags[0] = value;
        }
    }

    public PropertyObjectType copyPropertyObjectContainer;
        
    public string copyPropertyObjectType
    {
        get {
            CheckDataInitialized();
            return data.strings[0];
        }
        set {
            CheckDataInitialized();
            data.strings[0] = value;
        }
    }
    public GenericProperty copyPropertyVariable
    {
        get {
            CheckDataInitialized();
            return data.properties[1];
        }
        set {
            CheckDataInitialized();
            data.properties[1] = value;
        }
    }
    public enum modes { 
        setValue,
        copyOtherProperty
    }
    public modes changeMode { 
        get {
            CheckDataInitialized();
            modes mode;
            Enum.TryParse<modes>(data.strings[1],out mode);
            return mode;
        }
        set {
            CheckDataInitialized();
            data.strings[1] = Enum.GetNames(typeof(modes))[(int)value];
        }   

    }

    public override ActionData GetData()
    {
        ActionData data = base.GetData();
        if (copyPropertyObjectContainer != null && copyPropertyObjectContainer.data != null && copyPropertyObjectContainer.data.unityObjects != null && copyPropertyObjectContainer.data.unityObjects.Count > 0)
        {
            if (data.unityObjects == null)
                data.unityObjects = new List<UnityEngine.Object>();
            if (data.unityObjects.Count < 2)
                data.unityObjects.Add(new UnityEngine.Object());
            data.unityObjects[1] = copyPropertyObjectContainer.data.unityObjects[0];
        }


        return data;
    }

    public override void LoadData(ActionData data)
    {
        base.LoadData(data);

        if (!string.IsNullOrEmpty(copyPropertyObjectType))
        {
            copyPropertyObjectContainer = (PropertyObjectType)CreateInstance(copyPropertyObjectType);
            copyPropertyObjectContainer.saveData = SaveData;
            copyPropertyObjectContainer.data = new PropertyObjectType.Data();
            copyPropertyObjectContainer.data.unityObjects = new List<UnityEngine.Object>
        {
            new UnityEngine.Object()
        };
            if (data != null && data.unityObjects != null && data.unityObjects.Count > 1)
            {
                copyPropertyObjectContainer.data.unityObjects[0] = data.unityObjects[1];
            }
        }
        
       
    }

    public override void CheckDataInitialized() 
    {
        base.CheckDataInitialized();

        if (data == null)
            data = new ActionData();
        
        if(data.properties == null || data.properties.Length<2)
            data.properties = new LocalProperty[2];

        if (data.customEnumFlags == null || data.customEnumFlags.Length < 1)
            data.customEnumFlags = new CustomEnumFlags[1];

        if (data.unityObjects.Count < 2)
            data.unityObjects.Add(new Character());

        if(data.strings == null || data.strings.Length < 2)
            data.strings = new string[2];

    }

    public override void ExecuteAction(List<InteractionProperty> properties, LeafInteraction interaction)
    {
        VariableType[] variables = EnumerablesUtility.GetAllVariableTypes();
        if (changeMode == modes.setValue)
        {
            for (int i = 0; i < variables.Length; i++)
            {
                if (customEnumFlags.ContainsValueByFields(variables[i]))
                {
                    if (variables[i].data.isString)
                        propertyToSet.variablesContainer.SetValue(variables[i].TypeName, customEnumFlags.GetStringValue(variables[i].TypeName));
                    else
                        propertyToSet.variablesContainer.SetValue(variables[i].TypeName, customEnumFlags.GetObjectValue(variables[i].TypeName));
                }
            }
        }//FALTA EL DEFAULT
        else 
        {
            for (int i = 0; i < variables.Length; i++)
            {
                if (customEnumFlags.ContainsValueByFields(variables[i]))
                {
                    if (variables[i].data.isString)
                        propertyToSet.variablesContainer.SetValue(variables[i].TypeName, copyPropertyVariable.variablesContainer.GetStringValue(variables[i].TypeName));
                    else
                        propertyToSet.variablesContainer.SetValue(variables[i].TypeName, copyPropertyVariable.variablesContainer.GetObjectValue(variables[i].TypeName));
                }
            }
        }
    }


    private void UpdateVariableNameChoices(DropdownField propertyField) {
        propertyField.choices = new List<string>();
        propertyField.value = null;
        if (((CharacterType)characterType).character!=null)
        {
            Debug.Log("finding variable " + (propertyToSet==null).ToString());
            for (int i = 0; i < ((Character)((CharacterType)characterType).character).local_properties.Count; i++)
            {
                propertyField.choices.Add(((Character)((CharacterType)characterType).character).local_properties[i].name);
                if (((Character)((CharacterType)characterType).character).local_properties[i].name == propertyToSet?.name)
                {
                    Debug.Log("founded variable");
                    propertyField.value = propertyToSet?.name;
                }
            }
        }
    }

    public override void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    {
#if UNITY_EDITOR
        base.SetEditorField(visualElement, interaction);


        VisualElement newElement = new VisualElement();

        newElement.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/ChangeProperty.uxml").CloneTree());


        DropdownField propertyField = newElement.Q<DropdownField>("Property");
        
        if(characterType != null)
            UpdateVariableNameChoices(propertyField);

        ((CharacterType)characterType).onPropertyEditorChange += () =>
            {
                UpdateVariableNameChoices(propertyField);
                UpdateVariableTypes(newElement);
            };


            propertyField.RegisterValueChangedCallback((newvalue) =>
            {
                if (((CharacterType)characterType).character != null)
                    for (int i = 0; i < ((Character)((CharacterType)characterType).character).local_properties.Count; i++)
                    {
                        if (newvalue.newValue == ((Character)((CharacterType)characterType).character).local_properties[i].name)
                        { 
                            propertyToSet = ((Character)((CharacterType)characterType).character).local_properties[i];
                            SaveData?.Invoke();
                        }
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
            //changeModeField.bindingPath = "changeMode";
            //changeModeField.Bind(new SerializedObect(this));
            changeModeField.choices = Enum.GetNames(typeof(modes)).ToList();
        changeModeField.value = Enum.GetName(typeof(modes), changeMode);

        changeModeField.RegisterValueChangedCallback(value =>
        {
            modes newmode;
            Enum.TryParse<modes>(value.newValue,out newmode);
            changeMode = newmode;
            updateMode(newElement);
        });
        
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

            EnumerablesUtility.ShowDropdownField(copyPropertyObjectType, objectTypeField, ()=>
            {
                
                copyPropertyObjectType = objectTypeField.value;

                UpdateCopyPropertyObjectContainer();


                objectField.Clear();

                if (!string.IsNullOrEmpty(copyPropertyObjectType) && copyPropertyObjectContainer != null)
                { 

                    (copyPropertyObjectContainer).SetPropertyEditorField(objectField);

                    (copyPropertyObjectContainer).onPropertyEditorChange += () =>
                    {
                        //copyPropertyVariable = null;//no funciona porque al inicio se ejecuta
                        UpdateDropdownCopyVariables(newElement, variables);
                        UpdateCopyPropertyObjectContainer();
                    };
                    
                    UpdateDropdownCopyVariables(newElement, variables);

                }

                if (!string.IsNullOrEmpty(copyPropertyObjectType))
                {
                    SaveData?.Invoke();
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
        List<LocalProperty> localProperties = (copyPropertyObjectContainer).GetLocalPropertys();
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
                    SaveData?.Invoke();
                }
            }

            if(!string.IsNullOrEmpty(localPropertyToCopy))
                dropdown.value = localPropertyToCopy;// localPropertyToCopy;

            dropdown.RegisterValueChangedCallback((value) => {
                List<LocalProperty> localProperties = ((PropertyObjectType)copyPropertyObjectContainer).GetLocalPropertys();
                for (int i = 0; i < localProperties.Count; i++)
                {
                    if (value.newValue == localProperties[i].name)
                    { 
                        copyPropertyVariable = localProperties[i];
                        UpdateVariableTypes(rootElement);
                        SaveData?.Invoke();
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
            if (copyPropertyObjectType == props[i].name &&
                (copyPropertyObjectContainer == null || (copyPropertyObjectContainer).TypeName != props[i].TypeName))
            {
                copyPropertyObjectContainer = (PropertyObjectType)props[i].Copy();
                copyPropertyObjectContainer.saveData = SaveData;
                SaveData?.Invoke();
            }
        }
    }

    public override InteractionAction Copy()
    {
        CharacterSetLocalVariable action = new CharacterSetLocalVariable();
        action.characterType = (CharacterType)characterType?.Copy();
        action.propertyToSet = propertyToSet;
        if(copyPropertyObjectContainer != null)
            action.copyPropertyObjectContainer = copyPropertyObjectContainer.Copy() as PropertyObjectType;
        action.changeMode = changeMode;
        action.copyPropertyObjectType = copyPropertyObjectType;
        action.copyPropertyVariable = copyPropertyVariable;
        action.customEnumFlags = customEnumFlags.Copy();
        return action;
    }
}
