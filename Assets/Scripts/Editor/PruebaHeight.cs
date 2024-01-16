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
    private int draggingIndex = -1;

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
        if (draggedItem == null)
        {
            overCursorItem = null;
            return;
        }

        overCursorItem = listItem;
    }

    private void OnMouseUp(MouseUpEvent evt, VisualElement listItem, int i)
    {
        draggedItem = null;
        overCursorItem = null;
        RestoreColors();
    }

    private void OnMouseDown(MouseDownEvent evt, VisualElement listItem, int index)
    {
        if (evt.button == 0) // Botón izquierdo del ratón
        {
            /*draggingIndex = index;
            draggedItem = listItem;
            evt.StopPropagation();*/
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
            item.style.backgroundColor = color;
        }
    }


    void OnGUI()
    {
        if (draggedItem != null)
        {
            RestoreColors();

            if (Event.current.mousePosition.y < listContainer.layout.yMin 
              || Event.current.mousePosition.y > listContainer.layout.yMax)
            {
                draggedItem = null;
                overCursorItem = null;
            }
            else
            { 
                StyleColor bgcolor = new StyleColor();
                bgcolor.value = Color.black;
                draggedItem.style.backgroundColor = bgcolor;
            }
        }
        if (overCursorItem != null && Event.current.mousePosition.y > overCursorItem.layout.center.y)
        {
            StyleColor color = new StyleColor();
            color.value = Color.red;
            overCursorItem.style.borderBottomColor = color;
     
        }
        if (overCursorItem != null && Event.current.mousePosition.y <= overCursorItem.layout.center.y)
        {
            StyleColor color = new StyleColor();
            color.value = Color.red;
            overCursorItem.style.borderTopColor = color;
        }
        /*
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            Debug.Log("CLICK UP");
            listItems.Remove(draggedItem);
            listItems.Insert(0, draggedItem);
            draggedItem = null;
            listContainer.Clear();
            foreach (VisualElement item in listItems)
            {
                listContainer.Add(item);
            }
            Repaint();
        }
        */
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
        }*/
    }



    void UpdateListOrder()
    {
        // Obtener el índice del elemento de destino
        int endIndex = Mathf.Clamp(draggingIndex, 0, listItems.Count - 1);

        if (endIndex != draggingIndex)
        {
            // Mover el elemento arrastrado a la posición deseada
            listItems.Remove(draggedItem);
            listItems.Insert(endIndex, draggedItem);

            // Actualizar la visualización de la lista
            listContainer.Clear();
            foreach (VisualElement item in listItems)
            {
                listContainer.Add(item);
            }
        }
    }

}
