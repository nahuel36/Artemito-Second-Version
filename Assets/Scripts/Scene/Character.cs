using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : RoomInteractuable
{
    public IMessageTalker messageTalker;

    private void Start()
    {
        messageTalker = new LucasArtText(this.transform, new TextTimeCalculator());
    }
    

}
