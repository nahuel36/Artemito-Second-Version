using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(InteractionList))]
public class InteractionListCustomEditor : Editor
{
    VisualElement root;
    InteractionList myTarget;
    [SerializeField] VisualTreeAsset visualTree;
    Dictionary<Interaction, SubtypeSelector> subTypeSelectors;


    CustomListView<Interaction> listCustom;

    public override VisualElement CreateInspectorGUI()
    {
        subTypeSelectors = new Dictionary<Interaction, SubtypeSelector>();

        myTarget = (InteractionList)target;

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
        listView.RegisterCallback<ChangeEvent<string>>(OnChange);
        /*
        IEnumerator<VisualElement> enumerator = ((IEnumerable<VisualElement>)listView.Children()).GetEnumerator();
        enumerator.Reset();
        do
        {
            Debug.Log(enumerator.Current);

            //enumerator.Current.style.height = EditorGUIUtility.singleLineHeight * 7;
        } while (enumerator.MoveNext());
        */
        //listView.selectionChanged += objects => Debug.Log($"Selected: {string.Join(", ", objects)}");

        root.Add(listView);

        ScrollView scroll = new ScrollView();
        scroll.Add(new Label("PRUEBA1"));
        scroll.Add(new Label("PRUEBA2"));
        scroll.Add(new Label("PRUEBA3"));
        scroll.Add(new Label("PRUEBA4"));
        scroll.Add(new Label("PRUEBA5"));
        scroll.style.height = EditorGUIUtility.singleLineHeight * 2;
        root.Add(scroll);


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

        root.Add(listCustom.Init());

        return root;
    }

    private float itemHeight(int index)
    {
        if (myTarget.interactions[index].type == "Inventory")
            return EditorGUIUtility.singleLineHeight * 5;
        else
            return EditorGUIUtility.singleLineHeight * 3;
    }

    private void OnAdded(IEnumerable<int> obj)
    {
        myTarget.interactions[myTarget.interactions.Count - 1] = new Interaction();
    }

    private void OnChange(ChangeEvent<string> evt)
    {
        EditorUtility.SetDirty(target);
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
