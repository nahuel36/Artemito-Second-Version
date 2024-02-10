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
    [SerializeField] VisualTreeAsset interactionVT;

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
            VisualElement customListInventoryAttemps = customListVT.CloneTree();
            CustomListView<InteractionsAttemp> attemps = new CustomListView<InteractionsAttemp>();
            attemps.ItemsSource = myTarget.inventoryInteractions[index].attempsContainer.attemps;
            attemps.ItemContent = (index2) =>
            {
                InteractionCustomEditor interactionCustomEditor = (InteractionCustomEditor)CreateInstance(typeof(InteractionCustomEditor));
                VisualElement interactionVE = interactionVT.CloneTree();
                interactionCustomEditor.ShowGUI(interactionVE, myTarget.inventoryInteractions[index].attempsContainer.attemps[index2].interactions, target);
                return interactionVE;
            };
            attemps.Init(customListInventoryAttemps);
            return customListInventoryAttemps;
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
