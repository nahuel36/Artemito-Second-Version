using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

[CustomEditor(typeof(Character))]
public class CharacterCustomEditor : Editor
{
    VisualElement root;
    RoomInteractuable myTarget;
    [SerializeField] VisualTreeAsset visualTree;

    //cambiar colores multiplicando por alfa o sumando varios
    //falta un recuadro
    //falta seleccion multiple
    //falta que elimine lo seleccionado
    //DONE
    //probar uxml compuestos
    //falta reordenamiento en realtime y animado
    //falta que haga highlight cuando pasas el mouse
    //falta poder seleccionar

    public override VisualElement CreateInspectorGUI()
    {
        myTarget = (RoomInteractuable)target;

        VisualElement root = visualTree.CloneTree();

        root.Q("Interaction").visible = false;
        root.Q("Interaction").StretchToParentSize();

        CustomListView<InventoryItemAction> listViewInvInteractions = new CustomListView<InventoryItemAction>();
        listViewInvInteractions.ItemsSource = myTarget.inventoryInteractions;

        listViewInvInteractions.ItemContent = (indexInteraction)=> {
            VisualElement InvAttempsVE = new VisualElement();
            CustomListView<InteractionsAttemp> listViewAttemps = new CustomListView<InteractionsAttemp>();
            listViewAttemps.ItemsSource = myTarget.inventoryInteractions[indexInteraction].attempsContainer.attemps;
            listViewAttemps.ItemContent = (indexAttemp) =>
            {
                InteractionsCustomEditor interactionCustomEditor = (InteractionsCustomEditor)CreateInstance(typeof(InteractionsCustomEditor));
                VisualElement interactionVE = new VisualElement();
                interactionCustomEditor.ShowGUI(interactionVE, myTarget.inventoryInteractions[indexInteraction].attempsContainer.attemps[indexAttemp].interactions, target, true);
                return interactionVE;
            };
            listViewAttemps.Init(InvAttempsVE, true);
            return InvAttempsVE;
        };
        VisualElement invInteractionsVE = new VisualElement();
                    
        listViewInvInteractions.Init(invInteractionsVE, true);

        root.Add(invInteractionsVE);



        LocalAndGlobalProperties properties = (LocalAndGlobalProperties)CreateInstance(typeof(LocalAndGlobalProperties));

        properties.CreateGUI(myTarget.local_properties, root.Q("LocalAndGlobalProperties"));
        
        return root;
    }





    private void OnAdded(IEnumerable<int> obj)
    {
        myTarget.interactions[myTarget.interactions.Count - 1] = new Interaction();
    }

    
}
