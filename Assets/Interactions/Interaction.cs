using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Interaction 
{
    public string type;
    public string subtype;
    public string subtypeObject;

    public string InventoryNameToSubtypeObject(string item)
    {
        return "inventory-" + item;
    }

    public string SubtypeObjectToInventoryName(string subtype)
    {
        return subtype.Replace("inventory-", "");
    }
}
