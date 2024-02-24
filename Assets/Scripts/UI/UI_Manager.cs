using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    Objetive obj;
    UI_Text text;
    // Start is called before the first frame update
    void Start()
    {
        obj = FindAnyObjectByType<Objetive>();
        text = FindAnyObjectByType<UI_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (obj.overInteractuable != null)
        {
            text.text.text = "obj";
            if (Input.GetMouseButtonDown(0))
                obj.overInteractuable.RunInteraction();
        }
        else
            text.text.text = "";
    }
}
