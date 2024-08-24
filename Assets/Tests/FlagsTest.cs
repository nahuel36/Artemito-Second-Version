using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System;

[System.Flags]
public enum EntityFlags
{ 
    one = 1 << 0,
    two = 1 << 1,
    three = 1 << 2,

}


[System.Serializable]
public class FlagsTest : MonoBehaviour
{
    public EntityFlags flags;
    public System.Enum enumerator;
    public List<string> myList = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
    public int enumeratorFlag;
    public string testName;
    public List<LeafInteraction> interactions;
    public void OnEnable()
    {
       // enumerator = CreateEnumFromArrays(myList);
    }

    public static System.Enum CreateEnumFromArrays(List<string> list, int defaultValue)
    {

        System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
        AssemblyName aName = new AssemblyName("Enum");
        AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
        ModuleBuilder mb = ab.DefineDynamicModule(aName.Name);
        EnumBuilder enumerator = mb.DefineEnum("Enum", TypeAttributes.Public, typeof(int));

        int i = 0;
        enumerator.DefineLiteral("None", i); //Here = enum{ None }

        foreach (string names in list)
        {
            enumerator.DefineLiteral(names, 1<<i);
            i++;
        }

        //Here = enum { None, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }

        System.Type finished = enumerator.CreateTypeInfo();//enumerator.CreateTypeInfo();

        return (System.Enum)System.Enum.ToObject(finished, defaultValue);
    }

   
}

