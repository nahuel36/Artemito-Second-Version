using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class DialogOption
{
    public string initialText;
    public string currentText;
    public int index;
    public DialogRootInteraction interactions;
    public int subDialogDestinyIndex;
    public enum state{ 
        enabled = 0, 
        disabled = 1, 
    }
    public state initialState = state.enabled;
    public enum current_state
    {
        enabled = 0,
        disabled = 1,
        disabled_forever = 2
    }
    public current_state currentState;

    public bool say = true;

    public List<LocalProperty> local_properties = new List<LocalProperty>();
    public List<GlobalProperty> global_properties = new List<GlobalProperty>();

    public List<LocalProperty> current_local_properties = new List<LocalProperty>();
    public List<GlobalProperty> current_global_properties = new List<GlobalProperty>();


    //public LocalProperty[] LocalProperties { get { return local_properties; } set { local_properties = value; } }
    //public GlobalProperty[] GlobalProperties { get { return global_properties; } set { global_properties = value;  } }
}

[System.Serializable]
public class SubDialog
{
    public string text;
    public int index;
    public List<DialogOption> options;
    public Rect nodeRect;
    public bool expandedInInspector;
    public int optionSpecialIndex;
}


[CreateAssetMenu(fileName = "New Dialog", menuName = "Pnc/Dialog", order = 1)]
public class Dialog : ScriptableObject
{
    public List<SubDialog> subDialogs;
    public int subDialogIndex;
    public int initial_entryDialogIndex;
    public int current_entryDialogIndex;
    public Rect enterNodeRect;
    public Rect exitNodeRect;
    public int instanceID = 0;
    public bool isDuplicate = false;
    
    public void OnEnable() {
        if (instanceID != GetInstanceID() && instanceID != 0)
        {
            isDuplicate = true;
        }

    }

       

    public void ChangeSubDialogRect(int index, Rect rect)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                subDialogs[i].nodeRect = rect;
            }
        }
    }

    public void ChangeEntryRect(Rect rect)
    {
        enterNodeRect = rect;
    }

    public void ChangeExitRect(Rect rect)
    {
        exitNodeRect = rect;
    }


    public void ChangeText(int index, string text)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                subDialogs[i].text = text;
            }
        }
    }

    public string GetText(int index)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                return subDialogs[i].text;
            }
        }
        return "";
    }

    public void Remove(int index)
    {
        SubDialog founded = null;
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                founded = subDialogs[i];
            }
        }
        if(founded != null)
            subDialogs.Remove(founded);
    }

    public void ChangeDestiny(int index, int destiny, int option)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            for (int j = 0; j < subDialogs[i].options.Count; j++)
            {
                if (subDialogs[i].index == index && subDialogs[i].options[j].index == option)
                {
                    subDialogs[i].options[j].subDialogDestinyIndex = destiny;
                }
            }
        }
    }

    public void ChangeEntry(int index)
    {
        initial_entryDialogIndex = index;
    }

    public int GetOptionsCuantity(int index)
    {
        if(subDialogs != null)
            for (int i = 0; i < subDialogs.Count; i++)
            {
                if (subDialogs[i].options != null && subDialogs[i].index == index)
                {
                    return subDialogs[i].options.Count;
                }
            }
        return 0;
    }

    public int GetOptionSpecialIndex(int index, int optionindex)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                return subDialogs[i].options[optionindex].index;
            }
        }
        return 0;
    }

    public SubDialog GetSubDialogByIndex(int index)
    {
        for (int i = 0; i < subDialogs.Count; i++)
        {
            if (subDialogs[i].index == index)
            {
                return subDialogs[i];
            }
        }
        return null;
    }

    
}
