using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Verb
{
    public string name = "";
    public bool isLikeGive;
    public bool isLikeUse;
    public int index;
}

public class Interactions
{
    public AttempsContainer attempsContainer = new AttempsContainer();
}


[System.Serializable]
public class VerbInteractions
{
    public Verb verb = new Verb();
    //public bool use = true;
    public Interactions interactions;
}


[System.Serializable]
public class InventoryItemInteractions
{
   // public int specialIndex = -1;
    public Verb verb;
    // public RoomInteractuable sceneObject;
    public Interactions interactions; 
    public InventoryItemInteractions CopyItem(InventoryItemInteractions interactionOrigin)
    {
        InventoryItemInteractions interactionDestiny = new InventoryItemInteractions();
        interactionDestiny.interactions.attempsContainer = new AttempsContainer();
        interactionDestiny.interactions.attempsContainer.attempsIteration = interactionOrigin.interactions.attempsContainer.attempsIteration;
        interactionDestiny.interactions.attempsContainer.executedTimes = interactionOrigin.interactions.attempsContainer.executedTimes;
        interactionDestiny.interactions.attempsContainer.expandedInInspector = interactionOrigin.interactions.attempsContainer.expandedInInspector;
        interactionDestiny.interactions.attempsContainer.attemps = new List<InteractionsAttemp>();
        for (int i = 0; i < interactionOrigin.interactions.attempsContainer.attemps.Count; i++)
        {
            interactionDestiny.interactions.attempsContainer.attemps.Add(interactionOrigin.interactions.attempsContainer.attemps[i].CopyItem(interactionOrigin.interactions.attempsContainer.attemps[i]));
        }
        //interactionDestiny.sceneObject = interactionOrigin.sceneObject;
        interactionDestiny.verb = interactionOrigin.verb;
        //interactionDestiny.specialIndex = interactionOrigin.specialIndex;
        return interactionDestiny;
    }
}

[System.Serializable]
public class AttempsContainer
{
    public enum Iteration
    {
        inOrderAndRepeatLastOne,
        inOrderAndRestartAgain,
        random
    }
    public Iteration attempsIteration;
    public List<InteractionsAttemp> attemps = new List<InteractionsAttemp>();
    public bool expandedInInspector;
    public int executedTimes = 0;
}

[System.Serializable]
public class InteractionsAttemp
{
    public List<Interaction> interactions = new List<Interaction>();
    public bool expandedInInspector;

    public InteractionsAttemp CopyItem(InteractionsAttemp attempOrigin)
    {
        InteractionsAttemp attempDestiny = new InteractionsAttemp();
        attempDestiny.interactions = new List<Interaction>();
        for (int i = 0; i < attempOrigin.interactions.Count; i++)
        {
            attempDestiny.interactions.Add(attempOrigin.interactions[i].CopyItem(attempOrigin.interactions[i]));
        }
        attempDestiny.expandedInInspector = attempOrigin.expandedInInspector;
        return attempDestiny;
    }
}
