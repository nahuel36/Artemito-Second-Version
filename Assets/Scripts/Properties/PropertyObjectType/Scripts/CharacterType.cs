using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

[System.Serializable]
public class CharacterType : PropertyObjectType
{
    [System.Serializable]
    public class Data
    {
        public Character[] unityObjects;
    }

    public Data data;


    public override event ChangePropertyEditorField onPropertyEditorChange;
    public Character character
    {
        get {
            CheckInitializedData();
            return (Character)data.unityObjects[0];
        }
        set {
            CheckInitializedData();
            data.unityObjects[0] = value;
        }
    }
        
    public CharacterType()
    {
        
        Index = 0;
        TypeName = "Character";
    }

    private void OnEnable()
    {
        Index = 0;
        TypeName = "Character";
    }

    public void CheckInitializedData() 
    {
        if(data == null)
            data = new Data();
        if (data.unityObjects == null || data.unityObjects.Length<1)
            data.unityObjects = new Character[1];
    }

    public override void SetPropertyEditorField(VisualElement element)
    {
        CheckInitializedData();

        base.SetPropertyEditorField(element);

        ObjectField characterField = new ObjectField();
        characterField.label = "Character";
        characterField.objectType = typeof(Character);
        if(character != null)
            characterField.value = character;
        //characterField.bindingPath = "character";
        //characterField.Bind(new SerializedObject(this));
        characterField.RegisterValueChangedCallback((value) => {
            if (character != null && ((Character)character).gameObject != value.newValue)
            { 
                onPropertyEditorChange?.Invoke();
                
            }
            character = value.newValue as Character;
            saveData?.Invoke();
        });
        element.Add(characterField);
        

    }

    public override EnumerableType Copy()
    {
        CharacterType characterType = Instantiate(this);
        characterType.Index = Index;
        characterType.TypeName = TypeName;
        characterType.character = character;
        return characterType;
    }

    public override List<LocalProperty> GetLocalPropertys()
    {
        if(character!=null && ((Character)character).local_properties!=null)
            return ((Character)character).local_properties;

        return null;
    }
}
