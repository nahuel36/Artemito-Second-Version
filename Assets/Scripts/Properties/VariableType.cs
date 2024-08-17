using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class VariableType : EnumerableType
{
    public bool isDefaultValue = false;
    [SerializeField] public bool changedIngame = false;
    [SerializeField] public string stringValue;
    [SerializeField] public string stringIngameValue;
    [SerializeField] public UnityEngine.Object objectValue;
    [SerializeField] public UnityEngine.Object objectIngameValue;
    [SerializeField] public bool isString;
    public Action onChangeAVariableContentValue;
    public virtual void SetPropertyField(VisualElement element)
    {        

    }

    public virtual void SetValue<T>(T value)
    {   
        this.isDefaultValue = false;
        this.changedIngame = true;
        if(isString)
            this.stringIngameValue = value as string;
        else
            this.objectIngameValue = value as UnityEngine.Object;

        onChangeAVariableContentValue?.Invoke();
    }

    public virtual string GetStringValue()
    {
        if (changedIngame)
            return stringIngameValue;
        else
            return stringValue;
    }

    public virtual UnityEngine.Object GetObjectValue()
    {
        if (changedIngame)
            return objectIngameValue;
        else
            return objectValue;
    }

}
