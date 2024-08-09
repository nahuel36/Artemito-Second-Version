using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Linq;

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

        VisualElementsUtils.HideVisualElement(root.Q("Interaction"));
        


        InventoryInteractionsCustomEditor InventoryInteractionsCustomEditor = (InventoryInteractionsCustomEditor)CreateInstance(typeof(InventoryInteractionsCustomEditor));

        VisualElement invInteractionsVE = InventoryInteractionsCustomEditor.ShowGUI(myTarget.inventoryInteractions, myTarget, myTarget.isDuplicate);

        root.Add(invInteractionsVE);



        LocalAndGlobalProperties properties = (LocalAndGlobalProperties)CreateInstance(typeof(LocalAndGlobalProperties));

        if (myTarget.isDuplicate)
        {
            List<LocalProperty> newList = new List<LocalProperty>();
            for (int i = 0; i < myTarget.local_properties.Count; i++)
            {
                newList.Add((LocalProperty)myTarget.local_properties[i].Copy());
            }
            myTarget.local_properties = newList;
        }

        properties.CreateGUI(myTarget.local_properties, root.Q("LocalAndGlobalProperties"));
        
        return root;
    }


    
}
