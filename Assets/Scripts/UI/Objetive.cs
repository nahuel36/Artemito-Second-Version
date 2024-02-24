using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Objetive : MonoBehaviour
{
    [HideInInspector]public RoomInteractuable overInteractuable;
    //[HideInInspector] public GoToScene overGoToScene;
    PNCCursor cursor;
    [SerializeField] Transform comparePoint;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.FindObjectOfType<PNCCursor>();
       
    }

    // Update is called once per frame
    void Update()
    {
        overInteractuable = null;
        //overGoToScene = null;
        RaycastHit2D[] hits = Physics2D.RaycastAll(cursor.transform.position,Vector2.zero);
        int bestpriority = -1;
        foreach(RaycastHit2D hit in hits)
        {
            if (hit.collider)
            {
                RoomInteractuable pncInteractuable = hit.collider.GetComponent<RoomInteractuable>();
                if (pncInteractuable && pncInteractuable.priority > bestpriority)
                { 
                    overInteractuable = pncInteractuable;
                    bestpriority = pncInteractuable.priority;
                }
                /*GoToScene goToScene = hit.collider.GetComponent<GoToScene>();
                if (goToScene)
                {
                    overGoToScene = goToScene;   
                }
                */
            }
        }

    }
}
