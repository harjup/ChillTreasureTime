using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Nest : MonoBehaviour
{
    public string TextId;
    private GuiCanvas _guiCanvas;

    public GameObject ShiningBitDisplay;

    public List<GameObject> ShinyBits = new List<GameObject>();

    public void Start()
    {
        var shinyBitsTransform = transform
            .FindChild("ShinyBits");

        foreach (Transform t in shinyBitsTransform)
        {
            ShinyBits.Add(t.gameObject);
        }


        UpdateShinyDisplay();

        _guiCanvas = GuiCanvas.Instance;
    }

    public void Update()
    {
       
    }

    public void UpdateShinyDisplay()
    {
        var shinyBitsTransform = transform
          .FindChild("ShinyBits");


        var c = 0;
        foreach (Transform t in shinyBitsTransform)
        {
            t.gameObject.SetActive(false);

            if (c + 1 <= State.Instance.StoredShinyCount)
            {
                t.gameObject.SetActive(true);
            }

            c++;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            StartCoroutine(CashInAnim(player));
            Debug.Log("Enter " + other);
        }
    }

    private IEnumerator CashInAnim(Player player)
    {
        // Amount to bank
        int amountToBank = State.Instance.PlayerShinyCount;

        while (amountToBank > 0)
        {
            // Create collectable above player
            // Fly it into nest

            var bit = Instantiate(ShiningBitDisplay, player.transform.position, Quaternion.identity) as GameObject;

            DOTween.Sequence()
                .Append(bit.transform.DOMove(transform.position.SetY(transform.position.y + 5f), .5f))
                .Append(bit.transform.DOMove(transform.position, .5f))
                .OnComplete(() =>
                {
                    State.Instance.CashInShinyItem();

                    UpdateShinyDisplay();

                }).WaitForCompletion();

            amountToBank--;

            yield return new WaitForSeconds(Random.Range(0f, .2f));
        }

        // Shoot all the collectables into the nest
        yield return null;
    }

}
