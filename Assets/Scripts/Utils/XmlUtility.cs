using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class XmlUtility 
{
    public static string ConvertStringToUseInXml(string value)
    {
        return value.Replace(" ", "-");
        
    }

}
