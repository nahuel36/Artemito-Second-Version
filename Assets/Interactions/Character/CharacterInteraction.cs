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
    public CharacterType characterType;

    public override void SetEditorField(VisualElement visualElement, Interaction interaction)
    {
#if UNITY_EDITOR
        if(characterType == null)
            characterType = CreateInstance<CharacterType>();

        characterType.SetPropertyEditorField(visualElement);
#endif
    }
}
