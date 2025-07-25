using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class RoomInteractuable : PropertiesContainerInScene
{
    // Start is called before the first frame update
    public List<LeafInteraction> interactions;
    public List<InventoryItemRootInteractions> inventoryInteractions;
    public int priority = 0;
    public int instanceID = 0;
    public bool isDuplicate = false;
    public void RunInteraction()
    {
        InteractionUtils.RunAttempsInteraction(inventoryInteractions[0].interactions.attempsContainer, InteractionObjectsType.verbInObject, null, null, new RoomInteractuable[] { this });
    }

    public new void Awake()
    {
        base.Awake();
        if (instanceID != GetInstanceID())
        {
            if (instanceID == 0)
                instanceID = GetInstanceID();
            else
            { 
                isDuplicate = true;
                instanceID = GetInstanceID();
            }
        }
        else
            isDuplicate = false;
    }

    public void Update()
    {
        if (instanceID == GetInstanceID())
        {
            isDuplicate = false;
        }
    }
}
