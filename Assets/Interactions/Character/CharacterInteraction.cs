using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.Tilemaps;


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

        if (data.unityObjects == null || data.unityObjects.Count < 1)
        { 
            data.unityObjects = new List<UnityEngine.Object>
            {
                new Character()
            };
        }
    }

    public override ActionData GetData()
    {
        ActionData data = base.GetData();
        if (characterType.data != null && characterType.data.unityObjects != null && characterType.data.unityObjects.Count > 0)
        { 
            if(data.unityObjects == null)
                data.unityObjects= new List<UnityEngine.Object>();
            if (data.unityObjects.Count < 1)
                data.unityObjects.Add(new Character());
            data.unityObjects[0] = characterType.data.unityObjects[0];
        }
            

        return data;
    }

    public override void LoadData(ActionData data)
    {
        base.LoadData(data);

        if (characterType == null)
            characterType = CreateInstance<CharacterType>();

        characterType.data = new PropertyObjectType.Data();
        characterType.data.unityObjects = data.unityObjects;
    }
    public override void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    {
#if UNITY_EDITOR
        if (characterType == null)
            characterType = CreateInstance<CharacterType>();

        characterType.saveData = SaveData;

        ((CharacterType)characterType).SetPropertyEditorField(visualElement);
#endif
    }
}
