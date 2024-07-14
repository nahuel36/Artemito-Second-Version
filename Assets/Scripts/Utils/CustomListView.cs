using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using static UnityEditor.Progress;

public class CustomListView<T> 
{
    private ListView listView;

    public Color highlightedColor = Color.black;
    public IList<T> ItemsSource { get; set; }

    public Func<int, VisualElement> ItemContent;

    public Func<T> OnAdd;

    public Func<T, T> CopyItem;

    public delegate void ItemChangeDelegate(ChangeEvent<string> evt, VisualElement element, int index);
    public event ItemChangeDelegate OnChangeItem;
    public delegate void ItemReorderDelegate(int neworder);
    public event ItemReorderDelegate OnReorderItem;
    public delegate void ItemRemoveDelegate(VisualElement element, int index);
    public event ItemRemoveDelegate OnRemoveItem;

    private static T copiedItem;
    public void Init(VisualElement root, bool createVisualTree = false)
    {
#if UNITY_EDITOR
        if (createVisualTree)
        {
            root.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Editor/CustomListView.uxml").CloneTree());
        }
#endif

        listView = root.Q<ListView>("listView");

        listView.itemsSource = (List<T>)ItemsSource;

        listView.makeItem = () =>
        {
            VisualElement VE = new VisualElement();
            return VE;
        };

        listView.bindItem = AddAllItems;//AddAllItems;

        listView.showAddRemoveFooter = true;

        listView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;

        listView.itemIndexChanged += (int source, int destiny) => { OnReorderItem(destiny); };

        listView.showBorder = true;

        listView.style.borderRightWidth = 10;

        listView.reorderable = true;

        listView.reorderMode = ListViewReorderMode.Animated;

        listView.itemsAdded += new Action<IEnumerable<int>>((IEnumerable<int> k) =>
        {
            Add();

        });

        listView.itemsRemoved += new Action<IEnumerable<int>>((IEnumerable<int> k) =>
        {
            Remove();

        });

        Label footText = new Label("add or remove " + typeof(T).ToString()); 

        StyleEnum<TextAnchor> alignFoot = new StyleEnum<TextAnchor>();

        alignFoot.value = TextAnchor.UpperRight;

        footText.style.unityTextAlign = alignFoot;

        root.Add(footText);
    }

    private void Remove()
    {
        int index = ItemsSource.Count - 1;

//       TODO; que sea segun el seleccionado, no el ultimo

        OnRemoveItem?.Invoke(listView, index);
    }

    private void AddAllItems(VisualElement e, int index)
    {
        e.Clear();

        e.Add(ItemContent(index));
        // Agregar manipuladores de eventos para la reordenación

        e.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, listView, index));
        e.RegisterCallback<ChangeEvent<string>>(evt => OnChanged(evt, listView, index));   
    }

    public void Add()
    {
        if (OnAdd == null)
        { 
            Debug.LogError("You must define OnAdd in your custom list view");
            return;
        }
        ItemsSource[ItemsSource.Count-1] = OnAdd.Invoke();
        
    }

    private void OnChanged(ChangeEvent<string> evt, VisualElement listItem, int index)
    {
        OnChangeItem?.Invoke(evt, listItem, index);
    }

    
  

    private void OnMouseDown(MouseDownEvent evt, VisualElement listItem, int index)
    {
#if UNITY_EDITOR
        if (evt.button == 1 && CopyItem != null)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Copy " + typeof(T).ToString()), false, () =>
            {
                copiedItem = ItemsSource[index];
            });
            if (copiedItem != null)
            {
                genericMenu.AddItem(new GUIContent("Paste " + typeof(T).ToString()), false, () => 
                {
                    ItemsSource[index] = CopyItem(copiedItem);

                    listView.Rebuild();
                });
            }
            genericMenu.AddItem(new GUIContent("Cancel"), false, () =>
            {
            });
            genericMenu.DropDown(new Rect(Event.current.mousePosition, Vector2.zero));
            evt.StopPropagation();
        }
#endif
    }

    /*
     public void OnGUI()
     {

     if (draggedItem != null)
         {
             // Muestra el ícono del cursor de reordenación
             EditorGUIUtility.AddCursorRect(new Rect(0, 0, position.width, position.height), MouseCursor.Pan);

             // Mover el elemento arrastrado
             Vector2 mousePosition = Event.current.mousePosition;
             draggedItem.style.position = Position.Absolute;
             draggedItem.style.left = mousePosition.x - draggedItem.resolvedStyle.width * 0.5f;
             draggedItem.style.top = mousePosition.y - draggedItem.resolvedStyle.height * 0.5f;

             if (Event.current.type == EventType.MouseUp)
             {
                 // Finalizar el arrastre y reordenar
                 draggingIndex = -1;
                 draggedItem.style.position = Position.Relative;
                 UpdateListOrder();
                 draggedItem = null;
                 Repaint();
             }
         }
     
    }
    */


                    }
