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
    Dictionary<Interaction, SubtypeSelector> subTypeSelectors;

    CustomListView<Interaction> listCustom;

    public override VisualElement CreateInspectorGUI()
    {
        subTypeSelectors = new Dictionary<Interaction, SubtypeSelector>();

        myTarget = (RoomInteractuable)target;

        root = new VisualElement();



        listCustom = new();
        listCustom.ItemsSource = myTarget.interactions;

        Func<int, VisualElement> itemContent = (i) =>
        {
            int index = i;

            InteractionSelect select3 = new InteractionSelect();
            VisualElement visualElem = new VisualElement();
            visualElem.Add(select3.ShowAndConfigure(myTarget.interactions[index]));
            select3.OnChangeTypeEvent += (inter) => { UpdateSelector(inter, index, visualElem); };
            UpdateSelector(myTarget.interactions[index], index, visualElem);
            return visualElem;
        };

        listCustom.ItemContent = itemContent;

        listCustom.ItemHeight = itemHeight;

        //listCustom.OnChangeItem += OnChange;

        listCustom.OnAdd = OnAdded;

        listCustom.OnReorderItem += (evt, element, index) => { SaveTargetChanges(); };

        listCustom.OnRemoveItem += (element, index) => { SaveTargetChanges(); };

        root.RegisterCallback<ChangeEvent<string>>((evt) => { SaveTargetChanges(); });

        listCustom.reOrderMode = CustomListView<Interaction>.ReOrderModes.animatedDynamic;

        //cambiar colores multiplicando por alfa o sumando varios
        //falta un recuadro
        //falta seleccion multiple
        //falta que elimine lo seleccionado
        //DONE
        //probar uxml compuestos
        //falta reordenamiento en realtime y animado
        //falta que haga highlight cuando pasas el mouse
        //falta poder seleccionar

        VisualElement element = visualTree.CloneTree();

        listCustom.Init(element.Q<VisualElement>("CustomListView"));

        root.Add(element);

        LocalAndGlobalProperties properties = new LocalAndGlobalProperties();

        properties.CreateGUI(myTarget.local_properties, root.Q("LocalAndGlobalProperties"));

        return root;
    }

    private void SaveTargetChanges()
    {
        EditorUtility.SetDirty(target);
    }


    private Interaction OnAdded()
    {
        SaveTargetChanges();
        return new Interaction();
    }

    private StyleLength itemHeight(int index)
    {
        return new StyleLength(StyleKeyword.Auto);
    }

    private void OnAdded(IEnumerable<int> obj)
    {
        myTarget.interactions[myTarget.interactions.Count - 1] = new Interaction();
    }

    private void UpdateSelector(Interaction interaction, int index, VisualElement element)
    {
        if (interaction != myTarget.interactions[index]) return;


        if (!string.IsNullOrEmpty(interaction.type))
        {
            if (interaction.type == "Inventory" && !subTypeSelectors.ContainsKey(interaction))
            {
                subTypeSelectors.Add(interaction, new SelectInventory());
            }
            else if (interaction.type != "Inventory" && subTypeSelectors.ContainsKey(interaction))
            {
                ((SelectInventory)subTypeSelectors[interaction]).Remove(element);
                subTypeSelectors.Remove(interaction);
            }
        }

        if (subTypeSelectors.ContainsKey(interaction))
        {
            ((SelectInventory)subTypeSelectors[interaction]).Add(interaction, element);
        }
    }
}
