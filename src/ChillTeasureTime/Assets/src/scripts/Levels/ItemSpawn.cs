using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour
{
    public string Guid;

    public static List<string> CollectedList = new List<string>();

    public GameObject Shiney;


	// Use this for initialization
	void Start ()
	{
        GetComponent<MeshRenderer>().enabled = false;
        if (!CollectedList.Contains(Guid))
        {
            SpawnCollectable();
        }
	}

    private void SpawnCollectable()
    {
        var go = Instantiate(Shiney, transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<Collectable>().SetCallback(() => {CollectedList.Add(Guid);});
    }
}