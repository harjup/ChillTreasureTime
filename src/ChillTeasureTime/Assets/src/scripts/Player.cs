using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public void StartLevelTransition(Transform walkTarget)
    {
        // Walk to target
        Debug.Log(string.Format("Walk to {0}", walkTarget.position));
    }
}
