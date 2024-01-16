using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomListView : EditorWindow
{
    private List<VisualElement> listItems = new List<VisualElement>();
    private VisualElement listContainer;
    private VisualElement draggedItem;
    private VisualElement overCursorItem;
    private bool isBottom = false;

    [MenuItem("Window/Custom ListView")]
    public static void ShowExample()
    {
        CustomListView wnd = GetWindow<CustomListView>();
        wnd.titleContent = new GUIContent("Custom ListView");
    }

    void OnEnable()
    {
        VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Interactions/Editor/InteractionSelect 1.uxml");
        VisualElement root = visualTreeAsset.CloneTree();

        ScrollView scrollView = root.Q<ScrollView>("scrollView");
        listContainer = root.Q<VisualElement>("listContainer");

        // Crear elementos de la lista con alturas diferentes
        List<float> itemHeights = new List<float> { 50f, 75f, 100f, 60f };

        for (int i = 0; i < itemHeights.Count; i++)
        {
            VisualElement listItem = new VisualElement();
            listItem.style.height = itemHeights[i];
            listItem.Add(new Label("Elemento " + i));
            listItem.Add(new Label("Elemento " + i));
            listItem.Add(new Label("Elemento " + i));

            // Agregar manipuladores de eventos para la reordenación
            int index = i;
            listItem.RegisterCallback<MouseDownEvent>(evt => OnMouseDown(evt, listItem, index));
            listItem.RegisterCallback<MouseMoveEvent>(evt => OnMouseMove(evt, listItem, index));
            listItem.RegisterCallback<MouseUpEvent>(evt => OnMouseUp(evt, listItem, index));

            listItem.style.borderBottomWidth = 5;
            listItem.style.borderTopWidth = 5;

            listItems.Add(listItem);
            listContainer.Add(listItem);
        }

        root.Add(scrollView);
        root.StretchToParentSize();

        this.rootVisualElement.Add(root);
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

        if (Event.current.mousePosition.y > overCursorItem.layout.center.y)
        {
            StyleColor color = new StyleColor();
            color.value = Color.red;
            overCursorItem.style.borderBottomColor = color;
            isBottom = true;
        }
        if (Event.current.mousePosition.y <= overCursorItem.layout.center.y)
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

        int indexDestiny = Mathf.Clamp(listItems.IndexOf(overCursorItem) + (isBottom? 1 : 0), 0, listItems.Count -1);

        listItems.Remove(draggedItem);
        listItems.Insert(indexDestiny, draggedItem);
        overCursorItem = null;
        draggedItem = null;
        listContainer.Clear();
        foreach (VisualElement item in listItems)
        {
            listContainer.Add(item);
        }
        Repaint();


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
       
    void OnGUI()
        {
        if (draggedItem == null || overCursorItem == null) return;

        if (
            Input.mousePosition.y < listContainer.layout.yMin
        || Input.mousePosition.y > listContainer.layout.yMax
        || Input.mousePosition.x < listContainer.layout.xMin
        || Input.mousePosition.x > listContainer.layout.xMax)
        {
            draggedItem = null;
            overCursorItem = null;
            RestoreColors();
        }
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
