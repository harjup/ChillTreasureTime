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
            var itemSpawn = ((GameObject)obj).GetComponent<ItemSpawn>();
            if (itemSpawn != null)
            {
                itemSpawn.Guid = GuidToBase64(Guid.NewGuid());
                EditorUtility.SetDirty(itemSpawn);
            }

            var canBlow = ((GameObject)obj).GetComponent<CanBlow>();
            if (canBlow != null)
            {
                canBlow.Guid = GuidToBase64(Guid.NewGuid());
                EditorUtility.SetDirty(canBlow);
            }

            var sandPile = ((GameObject)obj).GetComponent<SandPile>();
            if (sandPile != null)
            {
                sandPile.Guid = GuidToBase64(Guid.NewGuid());
                EditorUtility.SetDirty(sandPile);
            }
        }
    }

    public static string GuidToBase64(Guid guid)
    {
        return Convert.ToBase64String(guid.ToByteArray()).Replace("/", "-").Replace("+", "_").Replace("=", "");
    }
}