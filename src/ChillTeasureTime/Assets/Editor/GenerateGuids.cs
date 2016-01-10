using System;
using UnityEditor;
using UnityEngine;

public class GenerateGuids : ScriptableWizard
{
    // Utility that gives all of our ItemSpawns a unique GUID so we can track them across multiple scene loads
    [MenuItem("Custom/Generate Guids")]
    static void Generate()
    {
        var objList = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (var obj in objList)
        {
            ItemSpawn guidObj = ((GameObject)obj).GetComponent<ItemSpawn>();
            if (guidObj != null)
            {
                guidObj.Guid = GuidToBase64(Guid.NewGuid());
            }
        }
    }

    public static string GuidToBase64(Guid guid)
    {
        return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");
    }
}