using UnityEngine;
using System.Collections;
using DG.Tweening;

public class AlternateVisibilityOnBeat : MonoBehaviour, IBeat, IBar
{

    public GameObject UpObject;
    public GameObject DownObject;

    public void OnBeat(int bar, int beat)
    {
        UpObject.SetActive(!UpObject.activeSelf);
        DownObject.SetActive(!DownObject.activeSelf);
    }

    public void OnBar(int bar)
    {
        transform.DOPunchPosition(Vector3.down * .1f, .1f);
    }
}
