using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class InteractionAction : ScriptableObject
{
    public virtual void SetEditorField(VisualElement visualElement, LeafInteraction interaction)
    { 
        
    }

    public virtual void ExecuteAction(List<InteractionProperty> properties, LeafInteraction interaction) 
    { 

    }

    public virtual InteractionAction Copy() { 
        return ScriptableObject.CreateInstance<InteractionAction>(); 
    }
}
