using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemType : PropertyObjectType
{
    public InventoryItemType()
    {
        Index = 2;
        TypeName = "Inventory'";
    }


    private void OnEnable()
    {
        Index = 2;
        TypeName = "Inventory'";
    }
}
