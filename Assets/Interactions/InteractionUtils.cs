using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;
public enum InteractionObjectsType
{
    inventoryIninventory,
    inventoryInObject,
    objectInObject,
    verbInObject,
    verbInInventory,
    dialogOption,
    sceneEvent,
    finishedTimer,
    unhandledEvent,

}
public class InteractionUtils : MonoBehaviour
{


    public async static Task RunAttempsInteraction(AttempsContainer attempsContainer, InteractionObjectsType interactionType, Verb verb, InventoryItem[] items, RoomInteractuable[] sceneInteractuables = null, bool runInBackground = false)
    {
        bool runUnhandledEvents = false;
        if (attempsContainer.attemps.Count == 0)
            runUnhandledEvents = true;
        else
        {
            int index;

            if (attempsContainer.attempsIteration == AttempsContainer.Iteration.inOrderAndRepeatLastOne)
                index = Mathf.Clamp(attempsContainer.executedTimes, 0, attempsContainer.attemps.Count - 1);
            else if (attempsContainer.attempsIteration == AttempsContainer.Iteration.inOrderAndRestartAgain)
                index = attempsContainer.executedTimes % attempsContainer.attemps.Count;
            else
                index = Random.Range(0, attempsContainer.attemps.Count);

            if (attempsContainer.attemps[index].interactions.Count == 0)
            {
                runUnhandledEvents = true;
            }
            else
            {
                int i = 0;
                while (i < attempsContainer.attemps[index].interactions.Count)
                {
                    InitializeInteractionCommand command = new InitializeInteractionCommand();
                    command.Queue(attempsContainer.attemps[index].interactions[i], runInBackground);

                    while (command.action == null)
                    {
                        await Task.Yield();
                    }

                    InteractionProperty argument;
                    if (attempsContainer.attemps[index].interactions[i].interaction_properties.Count <= 0)
                    {
                        attempsContainer.attemps[index].interactions[i].interaction_properties = new List<InteractionProperty>();
                        argument = new InteractionProperty();
                        attempsContainer.attemps[index].interactions[i].interaction_properties.Add(argument);
                    }
                    else
                    {
                        argument = attempsContainer.attemps[index].interactions[i].interaction_properties[0];
                    }
                    argument.interactionType = interactionType;
                    argument.verb = verb;
                    if (items != null && items.Length > 0)
                        argument.itemIndex = items[0].specialIndex;
                    if (sceneInteractuables != null && sceneInteractuables.Length > 0)
                        argument.interactuableID = sceneInteractuables[0].GetInstanceID();

                    attempsContainer.attemps[index].interactions[i].interaction_properties[0] = argument;

                    if (interactionType == InteractionObjectsType.inventoryIninventory)
                    {
                        InteractionProperty argument2 = new InteractionProperty();
                        argument2.interactionType = interactionType;
                        argument2.verb = verb;
                        argument2.itemIndex = items[1].specialIndex;

                        if (attempsContainer.attemps[index].interactions[i].interaction_properties.Count <= 1)
                        {
                            attempsContainer.attemps[index].interactions[i].interaction_properties.Add(argument2);
                        }
                        else
                        {
                            attempsContainer.attemps[index].interactions[i].interaction_properties[1] = argument2;
                        }
                    }
                    else if (interactionType == InteractionObjectsType.objectInObject)
                    {
                        InteractionProperty argument2 = new InteractionProperty();
                        argument2.interactionType = interactionType;
                        argument2.verb = verb;
                        argument2.interactuableID = sceneInteractuables[1].GetInstanceID();

                        if (attempsContainer.attemps[index].interactions[i].interaction_properties.Count <= 1)
                        {
                            attempsContainer.attemps[index].interactions[i].interaction_properties.Add(argument2);
                        }
                        else
                        {
                            attempsContainer.attemps[index].interactions[i].interaction_properties[1] = argument2;
                        }
                    }

                    command.action.Invoke(attempsContainer.attemps[index].interactions[i].interaction_properties);

                    i++;

                    if (i < attempsContainer.attemps[index].interactions.Count)
                    {
                        i = CheckConditionals(i, attempsContainer.attemps[index].interactions[i - 1]);
                        if (i == -1)
                        {
                            break;
                        }
                    }
                }
            }
        }

        if (runUnhandledEvents && interactionType != InteractionObjectsType.unhandledEvent)
        {
            RunUnhandledEvents(interactionType, verb, items, sceneInteractuables, runInBackground);
        }

        attempsContainer.executedTimes++;
    }

    public static void RunUnhandledEvents(InteractionObjectsType interactionType, Verb verb, InventoryItem[] item, RoomInteractuable[] sceneInteractuable = null, bool runInBackground = false)
    {

    }

    private static int CheckConditionals(int actualindex, Interaction interaction)
    {
        return actualindex;
    }

    public static UnityEvent<List<InteractionProperty>> InitializeInteraction(Interaction interaction)
    {
        UnityEvent<List<InteractionProperty>> action = new UnityEvent<List<InteractionProperty>>();

        action.AddListener((properties) => interaction.action.ExecuteAction(properties, interaction));

        return action;
    }
}
