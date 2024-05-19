using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif
[System.Serializable]
public class CharacterInteraction : InteractionAction
{
    public Character character;

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        ObjectField characterField = new ObjectField();
        characterField.label = "Character";
        characterField.objectType = typeof(Character);
        characterField.bindingPath = "character";
        characterField.Bind(new SerializedObject(this));
        visualElement.Add(characterField);
#endif
    }
}
