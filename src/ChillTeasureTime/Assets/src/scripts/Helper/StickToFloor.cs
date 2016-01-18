using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StickToFloor : MonoBehaviour
{
    void Update()
    {
        Ray r = new Ray(GameObject.Find("Player(Clone)").transform.position, -Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.transform.GetComponent<LevelGeometry>() != null)
            {
                float targetY = hit.point.y;
                transform.position = transform.position.SetY(targetY + .1f);
            }
        }
    }
}
