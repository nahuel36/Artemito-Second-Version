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

public class OldCustomListView<T> 
{
    private List<VisualElement> listItems = new List<VisualElement>();
    private List<VisualElement> listItemsInitial = new List<VisualElement>();
    private VisualElement listContainer;
    private VisualElement draggedItem;
    private float draggedItemPosY;
    private float firstItemPositionY;
    private VisualElement overCursorOnReorderItem;
    private bool isBottom = false;
    private VisualElement highlightedItem = null;
    private VisualElement selectedItem;
    private bool moving_all;
    public Color highlightedColor = Color.black;
    public enum ReOrderModes { 
        withBordersStatic, 
        animatedDynamic
    }

    public ReOrderModes reOrderMode = ReOrderModes.animatedDynamic;

    public IList<T> ItemsSource { get; set; }

    public Func<int, VisualElement> ItemContent;

    public Func<int, StyleLength> ItemHeight;

    public Func<T> OnAdd;

    public Func<T, T> CopyItem;

    public delegate void ItemChangeDelegate(ChangeEvent<string> evt, VisualElement element, int index);
    public event ItemChangeDelegate OnChangeItem;
    public delegate void ItemReorderDelegate(List<int> neworder);
    public event ItemReorderDelegate OnReorderItem;
    public delegate void ItemRemoveDelegate(VisualElement element, int index);
    public event ItemRemoveDelegate OnRemoveItem;

    public static EventCallback<ClickEvent> OnClickAdd;
    public static EventCallback<ClickEvent> OnClickRemove;

    private static T copiedItem;
    public void Init(VisualElement root, bool createVisualTree = false)
    {
#if UNITY_EDITOR
        if (createVisualTree)
        { 
            root.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Utils/Editor/CustomListView.uxml").CloneTree());
        }
#endif
        

        moving_all = false;

        ScrollView scrollView = root.Q<ScrollView>("scrollView");
        listContainer = root.Q<VisualElement>("listContainer");

        listContainer.Clear();

        if (ItemsSource != null)
            for (int i = 0; i < ItemsSource.Count; i++)
            {
                AddNewItem(i);
            }

        UpdateInitialItems();

        Button buttonAdd = root.Query<Button>("add").Last();
        //buttonAdd.clickable = new Clickable(() => { });
        if (OnClickAdd != null)
            buttonAdd.UnregisterCallback(OnClickAdd);
        OnClickAdd = (evt) => {Add(); };
        buttonAdd.RegisterCallback<ClickEvent>(OnClickAdd);

        Button buttonRemove = root.Query<Button>("remove").Last();
        //buttonRemove.clickable = new Clickable(() => { });
        if (OnClickRemove != null)
            buttonRemove.UnregisterCallback(OnClickRemove);
        OnClickRemove = (evt) => { Remove(); };
        buttonRemove.RegisterCallback<ClickEvent>(OnClickRemove);

        root.RegisterCallback<MouseLeaveEvent>(evt => OnMouseLeave(evt));
        //root.Add(scrollView);
        // root.StretchToParentSize();

        listContainer.style.borderLeftWidth = 3;
        listContainer.style.borderRightWidth = 3;
        listContainer.style.borderTopWidth = 3;
        listContainer.style.borderBottomWidth = 3;
        listContainer.style.borderBottomColor = Color.gray;
        listContainer.style.borderLeftColor = Color.gray;
        listContainer.style.borderTopColor = Color.gray;
        listContainer.style.borderRightColor = Color.gray;

    }

    private void UpdateInitialItems() 
    {
        listItemsInitial = new List<VisualElement>();
        for (int i = 0; i < listItems.Count; i++)
        {
            listItemsInitial.Add(listItems[i]);
        }
    }

    private async Task MoveVertical(bool directionIsDown, int index, float finalPosY)
    {
        float i = 0;
        while ((listItems[index].worldBound.yMin < finalPosY && directionIsDown)
            || (listItems[index].worldBound.yMin > finalPosY && !directionIsDown))
        {

            int multiplier = directionIsDown ? 1 : -1;
            Vector2 pos = listItems[index].transform.position;
            pos.y += multiplier * Easing.InOutQuad(i) * 10;
            listItems[index].transform.position = pos;
            i += 0.025f;
                      

            await Task.Delay(1);
        }
    }

    private async Task SendReordered()
    {
        await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));

        bool reordered = false;
        List<int> newOrder = new List<int>();
        for (int i = 0; i < listItemsInitial.Count; i++)
        {
            newOrder.Add(i);
            for (int j = 0; j < listItems.Count; j++)
            {
                if (listItemsInitial[i].Equals(listItems[j])&& i !=j)
                {
                    reordered = true;
                    newOrder[i] = j;
                }
            }
        }
        if (reordered)
        {
            OnReorderItem?.Invoke(newOrder);
            UpdateInitialItems();
        }
    }

    private void Remove()
    {
        int index = ItemsSource.Count - 1;

        ItemsSource.RemoveAt(index);
        VisualElement listItem = listItems[index];
        listContainer.Remove(listItem);
        listItems.RemoveAt(index);

        UpdateInitialItems();

        OnRemoveItem?.Invoke(listItem, index);
    }

    private void AddNewItem(int i)
    {
        int index = i;

        VisualElement listItem = new VisualElement();

        listItem.Add(ItemContent(index));
        // Agregar manipuladores de eventos para la reordenaci�n

        listItem.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, listItem, index));
        listItem.RegisterCallback<MouseMoveEvent>(evt => OnMouseMove(evt, listItem, index));
        listItem.RegisterCallback<MouseUpEvent>(evt => OnMouseUp(evt, listItem, index));
        listItem.RegisterCallback<ChangeEvent<string>>(evt => OnChanged(evt, listItem, index));
        listItem.RegisterCallback<MouseEnterEvent>(evt => OnMouseEnterItem(evt, listItem, index));

        if (reOrderMode == ReOrderModes.withBordersStatic)
        { 
            listItem.style.borderBottomWidth = 5;
            listItem.style.borderTopWidth = 5;
        }

        if(ItemHeight != null)
            listItem.style.height = ItemHeight(index);
        else
            listItem.style.height = new StyleLength(StyleKeyword.Auto);

        listItem.style.borderTopWidth = 10;

        listItem.style.borderBottomWidth = 10;

        listItem.style.borderLeftWidth = 10;

        listItem.style.borderRightWidth = 10;

        listItems.Add(listItem);
        listContainer.Add(listItem);
    }

    private void OnMouseEnterItem(MouseEnterEvent evt, VisualElement listItem, int index)
    {
        highlightedItem = listItem;
    }

    public void Add()
    {
        if (OnAdd == null)
        { 
            Debug.LogError("You must define OnAdd in your custom list view");
            return;
        } 
        ItemsSource.Add(OnAdd.Invoke());
        AddNewItem(ItemsSource.Count - 1);
        UpdateInitialItems();
    }

    private void OnChanged(ChangeEvent<string> evt, VisualElement listItem, int index)
    {
        if (ItemHeight != null)
            listItem.style.height = ItemHeight(index);
        else
            listItem.style.height = new StyleLength(StyleKeyword.Auto);

        UpdateInitialItems();
        
        OnChangeItem?.Invoke(evt, listItem, index);
    }

    private async Task OnMouseLeave(MouseLeaveEvent evt)
    {
        SendReordered();
        if (reOrderMode == ReOrderModes.animatedDynamic && draggedItem != null)
        {
            while (moving_all) await Task.Delay(100);

            draggedItemPosY = 0;

            draggedItem = null;
            listContainer.Clear();
            foreach (VisualElement item in listItems)
            {
                StyleTranslate translate = new StyleTranslate();
                translate.value = new Translate(0, 0);
                item.style.translate = translate;
                listContainer.Add(item);
            }
            return;
        }

        if (reOrderMode == ReOrderModes.withBordersStatic && draggedItem != null) draggedItem = null;
        if (overCursorOnReorderItem != null) overCursorOnReorderItem = null;
        if (highlightedItem != null) highlightedItem = null;
        //if (selectedItem != null) selectedItem = null;

        RestoreColors();
    }

    private async Task OnMouseMove(MouseMoveEvent evt, VisualElement listItem, int index)
    {
        RestoreColors();

        if (draggedItem == null)
        {
            overCursorOnReorderItem = null;
            return;
        }

        if (reOrderMode == ReOrderModes.withBordersStatic)
        { 
            overCursorOnReorderItem = listItem;

            float yCenter = overCursorOnReorderItem.worldBound.yMin + ((overCursorOnReorderItem.worldBound.yMax - overCursorOnReorderItem.worldBound.yMin) / 2f);

            if (Event.current.mousePosition.y > yCenter)
            {
                StyleColor color = new StyleColor();
                color.value = Color.red;
                overCursorOnReorderItem.style.borderBottomColor = color;
                isBottom = true;
            }
            if (Event.current.mousePosition.y <= yCenter)
            {
                StyleColor color = new StyleColor();
                color.value = Color.red;
                overCursorOnReorderItem.style.borderTopColor = color;
                isBottom = false;
            }
        }
        if (reOrderMode == ReOrderModes.animatedDynamic)
        {
            StyleTranslate translate = new StyleTranslate();
            draggedItemPosY += evt.mouseDelta.y;
            translate.value = new Translate(0, draggedItemPosY);
            draggedItem.style.translate = translate;

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000f));

            foreach (var item in listItems)
            {
                bool draggingUp = draggedItem.worldBound.yMin <= item.worldBound.center.y && listItems.IndexOf(draggedItem) == listItems.IndexOf(item) + 1;
                bool draggingDown = draggedItem.worldBound.yMax >= item.worldBound.center.y && listItems.IndexOf(draggedItem) == listItems.IndexOf(item) - 1;
                if ((draggingUp || draggingDown) && draggingDown != draggingUp)
                { 
                    T backup = ItemsSource[listItems.IndexOf(draggedItem)];

                    ItemsSource.RemoveAt(listItems.IndexOf(draggedItem));
                    listItems.Remove(draggedItem);

                    int indexDestiny = Mathf.Clamp(listItems.IndexOf(item) + (draggingDown?1:0), 0, ItemsSource.Count);

                    ItemsSource.Insert(indexDestiny, backup);
                    listItems.Insert(indexDestiny, draggedItem);

                }
            }

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));

            float yPosToMove = firstItemPositionY;
            float finalYPos = firstItemPositionY;
            for (int i = 0; i < listItems.Count; i++)
            {
                if (ItemHeight(i).keyword == StyleKeyword.Auto)
                {
                    if (listItems[i].style.height.keyword == StyleKeyword.Auto)
                    {
                        finalYPos += listItems[i].contentRect.yMax;
                    }
                    else
                        finalYPos += listItems[i].style.height.value.value;
                }
                else
                    finalYPos += ItemHeight(i).value.value;
            }

            for (int i = 0; i < listItems.Count; i++)
            {
                if (listItems[i] != draggedItem)
                {
                    MoveVertical(listItems[i].worldBound.yMin < yPosToMove, i, Mathf.Clamp(yPosToMove, firstItemPositionY, finalYPos));
                }
                yPosToMove += listItems[i].contentRect.yMax + listItems[i].style.borderBottomWidth.value;
            }
        }
    }

    private async Task OnMouseUp(MouseUpEvent evt, VisualElement listItem, int i)
    {
        SendReordered();
        if (reOrderMode == ReOrderModes.animatedDynamic && draggedItem != null)
        {
            while (moving_all) await Task.Delay(100);

            draggedItemPosY = 0;

            draggedItem = null;
            listContainer.Clear();
            foreach (VisualElement item in listItems)
            {
                StyleTranslate translate = new StyleTranslate();
                translate.value = new Translate(0, 0);
                item.style.translate = translate;
                listContainer.Add(item);
            }
            return;
        }

        if (draggedItem == listItem)
        {
            selectedItem = listItem;
            draggedItem = null;
            RestoreColors();
            return;
        }

        if (draggedItem == null || overCursorOnReorderItem == null)
        {
            overCursorOnReorderItem = null;
            draggedItem = null;
            RestoreColors();
            return;
        }

        T backup = ItemsSource[listItems.IndexOf(draggedItem)];

        ItemsSource.RemoveAt(listItems.IndexOf(draggedItem));
        listItems.Remove(draggedItem);

        int indexDestiny = Mathf.Clamp(listItems.IndexOf(overCursorOnReorderItem) + (isBottom ? 1 : 0), 0, ItemsSource.Count);

        ItemsSource.Insert(indexDestiny, backup);
        listItems.Insert(indexDestiny, draggedItem);
        overCursorOnReorderItem = null;
        draggedItem = null;
        listContainer.Clear();
        foreach (VisualElement item in listItems)
        {
            listContainer.Add(item);
        }

        RestoreColors();

    }

    private void OnMouseDown(MouseDownEvent evt, VisualElement listItem, int index)
    {
        if (evt.button == 0) // Bot�n izquierdo del rat�n
        {
            draggedItem = listItem;
            firstItemPositionY = listItems[0].worldBound.yMin;
            evt.StopPropagation();
        }
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

                    listContainer.Clear();

                    if (ItemsSource != null)
                        for (int i = 0; i < ItemsSource.Count; i++)
                        {
                            AddNewItem(i);
                        }
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

    void RestoreColors() 
    {
        foreach (var item in listItems)
        {
            StyleColor color = new StyleColor();
            color.value = Color.clear;
            item.style.borderBottomColor = color;
            item.style.borderTopColor = color;
            if (item != highlightedItem && item != draggedItem)
                item.style.backgroundColor = color;
            if(item == highlightedItem)
            { 
                StyleColor colorHg = new StyleColor();
                colorHg.value = highlightedColor;
                item.style.backgroundColor = colorHg;
            }
            if (item == draggedItem)
            {
                StyleColor colorDg = new StyleColor();
                colorDg.value = Color.blue;
                item.style.backgroundColor = colorDg;
            }
            if (item == selectedItem)
            {
                StyleColor colorSl = new StyleColor();
                colorSl.value = Color.red;
                item.style.backgroundColor = colorSl;
            }

        }
    }
    /*
     public void OnGUI()
     {

     if (draggedItem != null)
         {
             // Muestra el �cono del cursor de reordenaci�n
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
