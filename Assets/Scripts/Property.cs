using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericProperty
{
    public string name = "new property";
    public int integer = 0;
    public bool boolean = false;
    public string String = "";
    public bool integerDefault = true;
    public bool booleanDefault = true;
    public bool stringDefault = true;
    public bool expandedInInspector;

}
[System.Serializable]
public class LocalProperty : GenericProperty
{
    public VariableType variableTypes = new VariableType();
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

