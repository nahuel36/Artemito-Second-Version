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
}
