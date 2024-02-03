using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(RoomInteractuable))]
public class RoomInteractuableCustomEditor : Editor
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

        InteractionSelect select = new InteractionSelect();

        Func<VisualElement> makeItem = () =>
        {
            return select.VisualElements();
        };

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            InteractionSelect select2 = new InteractionSelect();
            select2.VisualElements();
            select2.Binding(myTarget.interactions[i], e);
            int index = i;
            VisualElement element = e;
            select2.OnChangeTypeEvent += (inter) => { UpdateSelector(inter, index, element); };
            UpdateSelector(myTarget.interactions[index], index, element);
        };

        ListView listView = new ListView();
        listView.itemsSource = myTarget.interactions;
        listView.reorderable = true;
        listView.reorderMode = ListViewReorderMode.Animated;
        listView.showAddRemoveFooter = true;
        listView.makeItem = makeItem;
        listView.bindItem = bindItem;
        listView.itemsAdded += OnAdded;
        listView.fixedItemHeight = EditorGUIUtility.singleLineHeight * 4;
        listView.selectionType = SelectionType.Multiple;
        listView.RegisterCallback<ChangeEvent<string>>((evt) => SaveTargetChanges());

        root.Add(listView);


        listCustom = new();
        listCustom.ItemsSource = myTarget.interactions;

        Func<int, VisualElement> itemContent = (i) =>
        {
            int index = i;

            InteractionSelect select3 = new InteractionSelect();
            VisualElement visualElem = new VisualElement();
            visualElem.Add(select3.BothFunctions(myTarget.interactions[index]));
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

        //probar uxml compuestos
        //cambiar colores multiplicando por alfa o sumando varios
        //falta un recuadro
        //falta seleccion multiple
        //falta reordenamiento en realtime y animado
        //falta que elimine lo seleccionado
        //DONE
        //falta que haga highlight cuando pasas el mouse
        //falta poder seleccionar

        VisualElement element = visualTree.CloneTree();

        listCustom.Init(element.Q<VisualElement>("CustomListView"));

        root.Add(element);

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

    private float itemHeight(int index)
    {
        if (string.IsNullOrEmpty(myTarget.interactions[index].type))
            return EditorGUIUtility.singleLineHeight * 2;
        if (myTarget.interactions[index].type == "Inventory")
            return EditorGUIUtility.singleLineHeight * 4;
        else
            return EditorGUIUtility.singleLineHeight * 3;
    }

    private void OnAdded(IEnumerable<int> obj)
    {
        myTarget.interactions[myTarget.interactions.Count - 1] = new Interaction();
    }

    private void UpdateSelector(Interaction interaction, int index, VisualElement element )
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
