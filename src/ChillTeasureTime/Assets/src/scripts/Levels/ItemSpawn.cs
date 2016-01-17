using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawn : MonoBehaviour
{
    public string Guid;

    public static List<string> CollectedList = new List<string>();

    public GameObject Shiney;
    public CollectableType ShineyType;

    private GuiCanvas _guiCanvas;

	// Use this for initialization
	void Start ()
	{
	    _guiCanvas = GuiCanvas.Instance;

        GetComponent<MeshRenderer>().enabled = false;
        if (!CollectedList.Contains(Guid))
        {
            SpawnCollectable();
        }
	}

    private void SpawnCollectable()
    {
        var go = Instantiate(Shiney, transform.position, Quaternion.identity) as GameObject;
        var collectable = go.GetComponent<Collectable>();
        if (collectable != null)
        {
            collectable.MyType = ShineyType;
        }

        go.GetComponent<Collectable>().SetCallback(() =>
        {
            CollectedList.Add(Guid);
            if (ShineyType == CollectableType.Good && !State.Instance.
                FirstShinyCollected)
            {
                var lines = new List<Line>
                {
                    new Line("", "Wow! A shining coin! I should find a safe place to stash this.")
                };

                StartCoroutine(ShowText(lines));
                State.Instance.FirstShinyCollected = true;
            }
        });
    }

    public IEnumerator ShowText(List<Line> lines)
    {
        FindObjectOfType<Player>().DisableControl();
        yield return StartCoroutine(DialogService.Instance.DisplayLines(lines));
        FindObjectOfType<Player>().EnableControl();
    }
}