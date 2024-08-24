using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class InventoryInteractionsCustomEditor : Editor
{
    public static void ShowGUI(VisualElement root, List<InventoryItemInteractions> inventoryInteractions, UnityEngine.Object myTarget, bool isDuplicate) 
    {
        VisualElement invInteractionsVE = new VisualElement();

        Label tittle = new Label("Inventory RootInteractions");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        invInteractionsVE.Add(tittle);

        CustomListView<InventoryItemInteractions> listViewInvInteractions = new CustomListView<InventoryItemInteractions>();
        listViewInvInteractions.ItemsSource = inventoryInteractions;

        listViewInvInteractions.ItemContent = (indexInteraction) => {
            if (inventoryInteractions[indexInteraction].interactions != null)
            {
                Foldout foldout = new Foldout();
                foldout.text = "interaction " + (indexInteraction + 1).ToString();
                VisualElement attempsVE = AttempsCustomEditor.ShowGUI(inventoryInteractions[indexInteraction].interactions.attempsContainer, myTarget, isDuplicate);
                foldout.Add(attempsVE);
                return foldout;
            }
            else return null;
        };

        listViewInvInteractions.highlightedColor = Color.black;

        listViewInvInteractions.CopyItem = (interactionOrigin) =>
        {
            return interactionOrigin.CopyItem(interactionOrigin);
        };

        listViewInvInteractions.OnAdd = () =>
        {
            InventoryItemInteractions newinventoryItem = new InventoryItemInteractions();
            newinventoryItem.interactions = new RootInteractions();
            newinventoryItem.interactions.attempsContainer = new AttempsContainer();
            return newinventoryItem;
        };

        listViewInvInteractions.Init(invInteractionsVE, true);

        root.Add(invInteractionsVE);
    }
}
