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

[System.Serializable]
public class VerbInteractions
{
    public Verb verb = new Verb();
    public bool use = true;
    public AttempsContainer attempsContainer = new AttempsContainer();
}


[System.Serializable]
public class InventoryItemAction
{
    public int specialIndex = -1;
    public Verb verb;
    public RoomInteractuable sceneObject;
    public AttempsContainer attempsContainer;

    public InventoryItemAction CopyItem(InventoryItemAction interactionOrigin)
    {
        InventoryItemAction interactionDestiny = new InventoryItemAction();
        interactionDestiny.attempsContainer = new AttempsContainer();
        interactionDestiny.attempsContainer.attempsIteration = interactionOrigin.attempsContainer.attempsIteration;
        interactionDestiny.attempsContainer.executedTimes = interactionOrigin.attempsContainer.executedTimes;
        interactionDestiny.attempsContainer.expandedInInspector = interactionOrigin.attempsContainer.expandedInInspector;
        interactionDestiny.attempsContainer.attemps = new List<InteractionsAttemp>();
        for (int i = 0; i < interactionOrigin.attempsContainer.attemps.Count; i++)
        {
            interactionDestiny.attempsContainer.attemps.Add(interactionOrigin.attempsContainer.attemps[i].CopyItem(interactionOrigin.attempsContainer.attemps[i]));
        }
        interactionDestiny.sceneObject = interactionOrigin.sceneObject;
        interactionDestiny.verb = interactionOrigin.verb;
        interactionDestiny.specialIndex = interactionOrigin.specialIndex;
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
