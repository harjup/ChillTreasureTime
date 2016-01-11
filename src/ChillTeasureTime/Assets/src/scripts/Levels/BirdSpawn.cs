using UnityEngine;
using System.Collections;

public class BirdSpawn : MonoBehaviour
{
    public string Id;

    public GameObject Bird;

    // Use this for initialization
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        //SpawnBird();
    }

    public void SpawnBird()
    {
        var go = Instantiate(Bird, transform.position, Quaternion.identity) as GameObject;
        
        go.GetComponent<Bird>().TextId = Id;
    }
}
