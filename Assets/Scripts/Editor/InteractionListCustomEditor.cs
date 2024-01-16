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
    InteractionList interactionList;
    [SerializeField] VisualTreeAsset visualTree;
    Dictionary<Interaction, VisualElement> subTypeSelectors;


    public override VisualElement CreateInspectorGUI()
    {
        subTypeSelectors = new Dictionary<Interaction, VisualElement>();

        interactionList = (InteractionList)target;

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
            select2.Binding(interactionList.interactions[i], e);
            int index = i;
            VisualElement element = e;
            select2.OnChangeTypeEvent += (inter) => { UpdateSelector(inter, index, element); };
            UpdateSelector(interactionList.interactions[index], index, element);
        };
        
        ListView listView = new ListView();
        listView.itemsSource = interactionList.interactions;
        listView.reorderable = true;
        listView.reorderMode = ListViewReorderMode.Animated;
        listView.showAddRemoveFooter = true;
        listView.makeItem = makeItem;
        listView.bindItem = bindItem;
        listView.itemsAdded += OnAdded;
        //listView.fixedItemHeight = EditorGUIUtility.singleLineHeight * 4;
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

        

        return root;
    }

    private void OnAdded(IEnumerable<int> obj)
    {
        interactionList.interactions[interactionList.interactions.Count - 1] = new Interaction();
    }

    private void OnChange(ChangeEvent<string> evt)
    {
        EditorUtility.SetDirty(target);
    }

    private void UpdateSelector(Interaction interaction, int index, VisualElement element)
    {
        if (interaction != interactionList.interactions[index]) return;

        if (!string.IsNullOrEmpty(interaction.type))
        {
            if (interaction.type == "Inventory" && !subTypeSelectors.ContainsKey(interaction))
            {
                SelectInventory selInv = new SelectInventory();
                subTypeSelectors.Add(interaction, selInv.VisualElements(interaction, element));

            }
            else if (interaction.type != "Inventory" && subTypeSelectors.ContainsKey(interaction))
            {
                if (element.Contains(subTypeSelectors[interaction]))
                    element.Remove(subTypeSelectors[interaction]);
                subTypeSelectors.Remove(interaction);
            }
        }

        if (subTypeSelectors.ContainsKey(interaction))
        {
            if (!element.Contains(subTypeSelectors[interaction]))
                element.Add(subTypeSelectors[interaction]);
        }

    }

}
