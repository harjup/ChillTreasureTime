using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RandomizeSprite : MonoBehaviour
{
    public List<Sprite> Sprites;

    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Sprites.AsRandom().ToList().First();
    }
}
