using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class InteractionAction : ScriptableObject
{
    public virtual void SetEditorField(VisualElement visualElement, Interaction interaction)
    { 
        
    }

    public virtual void ExecuteAction(List<InteractionProperty> properties, Interaction interaction) 
    { 

    }
}
