using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomListView<T> 
{
    private List<VisualElement> listItems = new List<VisualElement>();
    private VisualElement listContainer;
    private VisualElement draggedItem;
    private float draggedItemPosY;
    private VisualElement overCursorOnReorderItem;
    private bool isBottom = false;
    private VisualElement highlightedItem = null;
    private VisualElement selectedItem;

    public enum ReOrderModes { 
        withBordersStatic, 
        animatedDynamic
    }

    public ReOrderModes reOrderMode = ReOrderModes.animatedDynamic;

    public IList<T> ItemsSource { get; set; }

    public Func<int, VisualElement> ItemContent;

    public Func<int, float> ItemHeight;

    public Func<T> OnAdd;

    public delegate void ItemChangeDelegate(ChangeEvent<string> evt, VisualElement element, int index);
    public event ItemChangeDelegate OnChangeItem;
    public delegate void ItemReorderDelegate(MouseUpEvent evt, VisualElement element, int index);
    public event ItemReorderDelegate OnReorderItem;
    public delegate void ItemRemoveDelegate(VisualElement element, int index);
    public event ItemRemoveDelegate OnRemoveItem;


    public VisualElement Init()
    {
        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/CustomListView.uxml");
        VisualElement root = visualTreeAsset.CloneTree();

        ScrollView scrollView = root.Q<ScrollView>("scrollView");
        listContainer = root.Q<VisualElement>("listContainer");


        for (int i = 0; i < ItemsSource.Count; i++)
        {
            AddNewItem(i);
        }

        Button buttonAdd = root.Q<Button>("add");
        buttonAdd.clicked += Add;

        Button buttonRemove = root.Q<Button>("remove");
        buttonRemove.clicked += Remove;

        Button animTest = root.Q<Button>("anim");
        animTest.clicked += () => { MoveVertical(true,0); };

        root.RegisterCallback<MouseLeaveEvent>(evt => OnMouseLeave(evt));
        root.Add(scrollView);
       // root.StretchToParentSize();

        return root;
    }

    private async Task MoveVertical(bool directionIsDown, int index)
    {
        int i = 0;
        while (i < 10)
        {
            StyleTranslate translate = new StyleTranslate();
            translate.value = new Translate(0, directionIsDown?1:-1 * 10 * i);
            listItems[index].style.translate = translate;
            i++;

            await Task.Delay(75);
        }
    }



    private void Remove()
    {
        int index = ItemsSource.Count - 1;

        ItemsSource.RemoveAt(index);
        VisualElement listItem = listItems[index];
        listContainer.Remove(listItem);
        listItems.RemoveAt(index);

        OnRemoveItem?.Invoke(listItem, index);
    }

    private void AddNewItem(int i)
    {
        int index = i;

        VisualElement listItem = new VisualElement();
        listItem.style.height = ItemHeight(index);

        listItem.Add(ItemContent(index));
        // Agregar manipuladores de eventos para la reordenación

        listItem.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, listItem, index), TrickleDown.TrickleDown);
        listItem.RegisterCallback<MouseMoveEvent>(evt => OnMouseMove(evt, listItem, index));
        listItem.RegisterCallback<MouseUpEvent>(evt => OnMouseUp(evt, listItem, index));
        listItem.RegisterCallback<ChangeEvent<string>>(evt => OnChanged(evt, listItem, index));
        listItem.RegisterCallback<MouseEnterEvent>(evt => OnMouseEnterItem(evt, listItem, index));

        listItem.style.borderBottomWidth = 5;
        listItem.style.borderTopWidth = 5;

        listItems.Add(listItem);
        listContainer.Add(listItem);
    }

    private void OnMouseEnterItem(MouseEnterEvent evt, VisualElement listItem, int index)
    {
        highlightedItem = listItem;
    }

    public void Add()
    {
        ItemsSource.Add(OnAdd.Invoke());
        AddNewItem(ItemsSource.Count - 1);
    }

    private void OnChanged(ChangeEvent<string> evt, VisualElement listItem, int index)
    {
        listItem.style.height = ItemHeight(index);
        OnChangeItem?.Invoke(evt, listItem, index);
    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        if (draggedItem != null) draggedItem = null;
        if (overCursorOnReorderItem != null) overCursorOnReorderItem = null;
        if (highlightedItem != null) highlightedItem = null;
        if (selectedItem != null) selectedItem = null;

        RestoreColors();
    }

    private void OnMouseMove(MouseMoveEvent evt, VisualElement listItem, int index)
    {
        RestoreColors();

        if (draggedItem == null)
        {
            overCursorOnReorderItem = null;
            return;
        }
        
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

        if (reOrderMode == ReOrderModes.animatedDynamic)
        { 
            StyleTranslate translate = new StyleTranslate();
            draggedItemPosY += evt.mouseDelta.y;
            translate.value = new Translate(0, draggedItemPosY);
            draggedItem.style.translate = translate;
        }
    }

    private void OnMouseUp(MouseUpEvent evt, VisualElement listItem, int i)
    {
        if (draggedItem == listItem)
        {
            selectedItem = listItem;
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

        OnReorderItem?.Invoke(evt, listItem, i);
    }

    private void OnMouseDown(MouseDownEvent evt, VisualElement listItem, int index)
    {
        if (evt.button == 0) // Botón izquierdo del ratón
        {
            draggedItem = listItem;
            evt.StopPropagation();
        }
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
                colorHg.value = Color.black;
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
