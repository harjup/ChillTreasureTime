using UnityEngine;
using System.Collections;

public class FightService : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        SceneFadeInOut.Instance.StartScene();

        var player = FindObjectOfType<Player>();
    }
}
