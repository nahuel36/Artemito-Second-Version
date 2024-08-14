using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
public class InventoryInteractionsCustomEditor : Editor
{
    public static void ShowGUI(VisualElement root, List<InventoryItemAction> inventoryInteractions, UnityEngine.Object myTarget, bool isDuplicate) 
    {
        VisualElement invInteractionsVE = new VisualElement();

        Label tittle = new Label("Inventory");

        tittle.style.unityFontStyleAndWeight = FontStyle.Bold;

        invInteractionsVE.Add(tittle);

        CustomListView<InventoryItemAction> listViewInvInteractions = new CustomListView<InventoryItemAction>();
        listViewInvInteractions.ItemsSource = inventoryInteractions;

        listViewInvInteractions.ItemContent = (indexInteraction) => {
            Foldout foldout = new Foldout();
            foldout.text = "interaction " + (indexInteraction + 1).ToString();
            AttempsCustomEditor attempsCustomEditor = (AttempsCustomEditor)CreateInstance(typeof(AttempsCustomEditor));
            VisualElement attempsVE = attempsCustomEditor.ShowGUI(inventoryInteractions[indexInteraction].attempsContainer, myTarget, isDuplicate);
            foldout.Add(attempsVE);
            return foldout;
        };

        listViewInvInteractions.highlightedColor = Color.black;

        listViewInvInteractions.CopyItem = (interactionOrigin) =>
        {
            return interactionOrigin.CopyItem(interactionOrigin);
        };

        listViewInvInteractions.OnAdd = () =>
        {
            InventoryItemAction newinventoryItem = new InventoryItemAction();
            newinventoryItem.attempsContainer = new AttempsContainer();
            return newinventoryItem;
        };

        listViewInvInteractions.Init(invInteractionsVE, true);

        root.Add(invInteractionsVE);
    }
}
