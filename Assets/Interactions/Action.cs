using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System;
using UnityEngine;
using UnityEngine.UIElements;
public class InteractionAction : ScriptableObject
{
    [System.Serializable]
    public class ActionData
    {
        public List<UnityEngine.Object> unityObjects;
        public string[] strings;
        public GenericProperty[] properties;
        public CustomEnumFlags[] customEnumFlags;
    }

    public ActionData data;

    public virtual void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    { 
        
    }

    public virtual void ExecuteAction(List<InteractionProperty> properties, LeafInteraction interaction) 
    { 

    }

    public virtual InteractionAction Copy() { 
        return ScriptableObject.CreateInstance<InteractionAction>(); 
    }

    public virtual void LoadData(ActionData data)
    {
        this.data = data;
    }

    public virtual ActionData GetData()
    { 
        return data;
    }

    public Action SaveData;


}
