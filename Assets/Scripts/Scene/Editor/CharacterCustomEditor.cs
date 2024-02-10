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
    [SerializeField] VisualTreeAsset customListVT;

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

        InteractionCustomEditor interaction = (InteractionCustomEditor)CreateInstance(typeof(InteractionCustomEditor));

        interaction.ShowGUI(root.Q("Interaction"), myTarget.interactions, target);



        CustomListView<InventoryItemAction> inventoryInteractions = new CustomListView<InventoryItemAction>();
        inventoryInteractions.ItemsSource = myTarget.inventoryInteractions;

        VisualElement customListInventory = customListVT.CloneTree();

        inventoryInteractions.ItemContent = (index)=> {
            VisualElement elementInventoryAttemp = new VisualElement();
            VisualElement customListInventoryAttemps = customListVT.CloneTree();
            CustomListView<InteractionsAttemp> attemps = new CustomListView<InteractionsAttemp>();
            attemps.ItemsSource = myTarget.inventoryInteractions[index].attempsContainer.attemps;
            attemps.ItemContent = (index2) =>
            {
                VisualElement elementInteraction = new VisualElement();
                VisualElement customListInventoryInteraction = customListVT.CloneTree();
                CustomListView<Interaction> interactionCLV = new CustomListView<Interaction>();
                interactionCLV.ItemsSource = myTarget.inventoryInteractions[index].attempsContainer.attemps[index2].interactions;
                interactionCLV.ItemContent = (index) =>
                {
                    return new Label("show");
                };
                interactionCLV.Init(customListInventoryInteraction);
                elementInteraction.Add(customListInventoryInteraction);
                return elementInteraction;
            };
            attemps.Init(customListInventoryAttemps);
            elementInventoryAttemp.Add(customListInventoryAttemps);
            return elementInventoryAttemp;
        };

        inventoryInteractions.Init(customListInventory);

        root.Add(customListInventory);




        LocalAndGlobalProperties properties = (LocalAndGlobalProperties)CreateInstance(typeof(LocalAndGlobalProperties));

        properties.CreateGUI(myTarget.local_properties, root.Q("LocalAndGlobalProperties"));
        
        return root;
    }





    private void OnAdded(IEnumerable<int> obj)
    {
        myTarget.interactions[myTarget.interactions.Count - 1] = new Interaction();
    }

    
}
