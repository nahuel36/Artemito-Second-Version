using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomInteractuable : PropertiesContainer
{
    // Start is called before the first frame update
    public List<Interaction> interactions;
    public List<InventoryItemAction> inventoryInteractions;
    public int priority = 0;
}
