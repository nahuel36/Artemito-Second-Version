using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif

public class CharacterType : PropertyObjectType
{

    public Character character;
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

    public override void SetPropertyEditorField(VisualElement element)
    {
        base.SetPropertyEditorField(element);

        ObjectField characterField = new ObjectField();
        characterField.label = "Character";
        characterField.objectType = typeof(Character);
        characterField.bindingPath = "character";
        characterField.Bind(new SerializedObject(this));
        element.Add(characterField);


    }

    public override EnumerableType Copy()
    {
        CharacterType characterType = new CharacterType();
        characterType.Index = Index;
        characterType.TypeName = TypeName;
        characterType.character = character;
        return characterType;
    }

    public override List<LocalProperty> GetLocalPropertys()
    {
        if(character!=null && character.local_properties!=null)
            return character.local_properties;

        return null;
    }
}
