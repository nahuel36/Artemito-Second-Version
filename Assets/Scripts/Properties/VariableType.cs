using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class VariableType : EnumerableType
{
    [System.Serializable]
    public class VariableData {
        [SerializeField] public string stringValue;
        [SerializeField] public UnityEngine.Object objectValue;
        [SerializeField] public bool isString;
        public bool isDefaultValue = false;
        public string variableType = "";
    }

    public VariableData data = new VariableData();

    
    [SerializeField] public bool changedIngame = false;

    [SerializeField] public string stringIngameValue;
    
    [SerializeField] public UnityEngine.Object objectIngameValue;
    
    public Action saveData;
    public Action onChange;

    public virtual void SetPropertyField(VisualElement element)
    {        

    }

    public virtual void SetValue<T>(T value)
    {   
        this.data.isDefaultValue = false;
        this.changedIngame = true;
        if(data.isString)
            this.stringIngameValue = value as string;
        else
            this.objectIngameValue = value as UnityEngine.Object;

        onChange?.Invoke();
        saveData?.Invoke();
    }

    public virtual string GetStringValue()
    {
        if (changedIngame)
            return stringIngameValue;
        else
            return data.stringValue;
    }

    public virtual UnityEngine.Object GetObjectValue()
    {
        if (changedIngame)
            return objectIngameValue;
        else
            return data.objectValue;
    }

}
