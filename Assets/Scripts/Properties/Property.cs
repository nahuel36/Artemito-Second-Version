using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericProperty
{
    public string name = "new property";
    public bool expandedInInspector;
    //[SerializeField]public Dictionary<VariableType, string> value = new Dictionary<VariableType, string>(); //no funciona porque es diccionario
    //[SerializeField]public object[] variableValue = new object[1]; // no funciona porque es objeto
    //[SerializeField] public System.IComparable[] variableValue = new System.IComparable[1]; // no funciona la clase icomparable
    //public string stringvalue; //funciona
    [SerializeField]public string[] variableValues; //funciona
    [SerializeField]public bool[] useDefaultValues;
    [SerializeField]public UnityEngine.Object[] objectValues; //funciona
}


[System.Serializable]
public class InteractionProperty : GenericProperty
{
    public CustomEnumFlags<VariableType> variableTypes;
    public InteractionObjectsType interactionType;
    public Verb verb;
    public int itemIndex;
    public int interactuableID;

    internal InteractionProperty Copy()
    {
        InteractionProperty prop = new InteractionProperty();
        prop.name = name;
        prop.expandedInInspector = expandedInInspector;
        prop.variableValues = new string[variableValues.Length];
        variableValues.CopyTo(prop.variableValues,0);
        prop.useDefaultValues = new bool[useDefaultValues.Length];
        useDefaultValues.CopyTo(prop.useDefaultValues,0);
        prop.objectValues = new UnityEngine.Object[objectValues.Length];
        objectValues.CopyTo(prop.objectValues, 0);
        prop.variableTypes = variableTypes.Copy();
        prop.interactionType = interactionType;
        prop.verb = verb;
        prop.itemIndex = itemIndex;
        //prop.interactuableID = interactuableID;

        return prop;
    }
}
    
    
[System.Serializable]
public class LocalProperty : GenericProperty
{
    public CustomEnumFlags<VariableType> variableTypes;
}

[System.Serializable]
public class GlobalProperty : GenericProperty
{
    //public int globalID = -1;
    public GlobalPropertyConfig config;
}

[System.Serializable]
public class GlobalPropertyConfig
{
    public string name;
    //public int ID = -1;
    public PropertyObjectType objectTypes;
    public VariableType variableTypes;
}

