using UnityEngine;
using System.Collections;

public class RemoveRendererOnStart : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject.GetComponent<MeshFilter>());
    }
}
