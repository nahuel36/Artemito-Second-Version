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
    //erializeField]public string[] variableValues; //funciona
    //[SerializeField]public bool[] useDefaultValues;
    //[SerializeField]public UnityEngine.Object[] objectValues; //funciona
    public CustomEnumFlags<VariableType> variablesContainer;

    public virtual GenericProperty Copy()
    { 
        GenericProperty prop = new GenericProperty(); 
        prop.name = name;
        prop.expandedInInspector = expandedInInspector;
        prop.variablesContainer = variablesContainer.Copy();
        return prop;
    }
}


[System.Serializable]
public class InteractionProperty : GenericProperty
{
    public InteractionObjectsType interactionType;
    public Verb verb;
    public int itemIndex;
    public int interactuableID;

    public override GenericProperty Copy()
    {
        InteractionProperty prop = new InteractionProperty();
        prop.name = name;
        prop.expandedInInspector = expandedInInspector;
        prop.variablesContainer = variablesContainer.Copy();
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
    

    public override GenericProperty Copy()
    {
        LocalProperty prop = new LocalProperty();
        prop.name = name;
        prop.expandedInInspector = expandedInInspector;
        prop.variablesContainer = variablesContainer.Copy();

        return prop;
    }
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
    public int objectTypes;
    public int variableTypes;
}

