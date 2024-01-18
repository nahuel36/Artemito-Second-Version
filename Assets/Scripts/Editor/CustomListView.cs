using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomListView<T> 
{
    public List<VisualElement> listItems = new List<VisualElement>();
    public VisualElement listContainer;
    private VisualElement draggedItem;
    private VisualElement overCursorItem;
    private bool isBottom = false;

    
    public IList<T> ItemsSource { get; set; }

    public Func<int, VisualElement> ItemContent;

    public Func<int, float> ItemHeight;

    public VisualElement Init()
    {
        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/InteractionSelect 1.uxml");
        VisualElement root = visualTreeAsset.CloneTree();

        ScrollView scrollView = root.Q<ScrollView>("scrollView");
        listContainer = root.Q<VisualElement>("listContainer");


        for (int i = 0; i < ItemsSource.Count; i++)
        {
            int index = i;

            VisualElement listItem = new VisualElement();
            listItem.style.height = ItemHeight(index);
                      

            listItem.Add(ItemContent(index));
            // Agregar manipuladores de eventos para la reordenación
            
            listItem.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, listItem, index));
            listItem.RegisterCallback<MouseMoveEvent>(evt => OnMouseMove(evt, listItem, index));
            listItem.RegisterCallback<MouseUpEvent>(evt => OnMouseUp(evt, listItem, index));
            listItem.RegisterCallback<ChangeEvent<string>>(evt => OnChanged(evt, listItem, index));


            listItem.style.borderBottomWidth = 5;
            listItem.style.borderTopWidth = 5;

            listItems.Add(listItem);
            listContainer.Add(listItem);
        }

        root.RegisterCallback<MouseLeaveEvent>(evt => OnMouseLeave(evt));
        root.Add(scrollView);
       // root.StretchToParentSize();

        return root;
    }

    private void OnChanged(ChangeEvent<string> evt, VisualElement listItem, int index)
    {
        listItem.style.height = ItemHeight(index);

    }

    private void OnMouseLeave(MouseLeaveEvent evt)
    {
        if (draggedItem != null || overCursorItem != null)
        { 
            draggedItem = null;
            overCursorItem = null;
            RestoreColors();
        }
    }

    private void OnMouseMove(MouseMoveEvent evt, VisualElement listItem, int index)
    {
        RestoreColors();

        if (draggedItem == null)
        {
            overCursorItem = null;
            return;
        }
        
        overCursorItem = listItem;

        float yCenter = overCursorItem.worldBound.yMin + ((overCursorItem.worldBound.yMax - overCursorItem.worldBound.yMin) / 2f);

        if (Event.current.mousePosition.y > yCenter)
        {
            StyleColor color = new StyleColor();
            color.value = Color.red;
            overCursorItem.style.borderBottomColor = color;
            isBottom = true;
        }
        if (Event.current.mousePosition.y <= yCenter)
        {
            StyleColor color = new StyleColor();
            color.value = Color.red;
            overCursorItem.style.borderTopColor = color;
            isBottom = false;
        }

        StyleColor bgcolor = new StyleColor();
        bgcolor.value = Color.black;
        draggedItem.style.backgroundColor = bgcolor;

    }

    private void OnMouseUp(MouseUpEvent evt, VisualElement listItem, int i)
    {
        if (draggedItem == null || overCursorItem == null || draggedItem == overCursorItem)
        {
            overCursorItem = null;
            draggedItem = null;
            RestoreColors();
            return;
        }

        listItems.Remove(draggedItem);

        int indexDestiny = Mathf.Clamp(listItems.IndexOf(overCursorItem) + (isBottom ? 1 : 0), 0, listItems.Count - 1);

        listItems.Insert(indexDestiny, draggedItem);
        overCursorItem = null;
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
        if (evt.button == 0) // Botón izquierdo del ratón
        {
            draggedItem = listItem;
            StyleColor bgcolor = new StyleColor();
            bgcolor.value = Color.black;
            draggedItem.style.backgroundColor = bgcolor;
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
            item.style.backgroundColor = color;
        }
    }
       
    public void OnGUI()
        {
        /*
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
        */
    }
    


}
