using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
[System.Serializable]
public class PropertyObjectType : EnumerableType
{
    public delegate void ChangePropertyEditorField();
    public virtual event ChangePropertyEditorField onPropertyEditorChange;
    [System.Serializable]
    public class Data
    {
        public List<UnityEngine.Object> unityObjects;
    }

    public Data data;
    public virtual void SetPropertyEditorField(VisualElement element) { 
        
    }

    public virtual List<LocalProperty> GetLocalPropertys() 
    {
        return null;
    }
}
