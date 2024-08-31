using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif
[System.Serializable]
public class CharacterInteraction : InteractionAction
{
    public CharacterType characterType;
    
    public virtual void CheckDataInitialized()
    {
        if (data == null)
            data = new ActionData();

        if (data.unityObjects == null || data.unityObjects.Length < 1)
            data.unityObjects = new UnityEngine.Object[1];
    }

    public override void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    {
#if UNITY_EDITOR
        if (characterType == null)
            characterType = CreateInstance<CharacterType>();
        
        ((CharacterType)characterType).SetPropertyEditorField(visualElement);
#endif
    }
}
