using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RotateLoop : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        transform
            .DOLocalRotate(transform.eulerAngles + new Vector3(0f, 360f, 0f), 4f, RotateMode.FastBeyond360).SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Incremental);
    }
}
