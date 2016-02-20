using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightMenuItem : MonoBehaviour
{
    public enum Kind
    {
        None,
        Flap,
        Peck,
        Item,
        Run
    }

    public static List<Kind> GetAllKinds()
    {
        return new List<Kind>()
        {
            Kind.Flap,
            Kind.Peck,
            Kind.Item,
            Kind.Run
        };
    }
    
    private Dictionary<Kind, string> _kingToSpriteMap = new Dictionary<Kind, string>
    {
      {Kind.Flap, "Battle/MenuIcon/Battle-Flap" },
      {Kind.Peck, "Battle/MenuIcon/Battle-Peck" },
      {Kind.Item, "Battle/MenuIcon/Battle-Peach" },
      {Kind.Run, "Battle/MenuIcon/Battle-Run" }
    };

    public Kind WhichKind;

    private SpriteRenderer _spriteRenderer;

    public void SetKind(Kind kind)
    {
        WhichKind = kind;
        var spritePath = _kingToSpriteMap[kind];

        var sprite = Resources.Load<Sprite>(spritePath);
        if (sprite == null)
        {
            Debug.LogError("Sprite not found at path " + spritePath);
        }

        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        gameObject.name = gameObject.name.Split('-')[0] + "-" + kind;

        _spriteRenderer.sprite = sprite;
    }

    public void Start()
    {
        if (WhichKind != Kind.None)
        {
            SetKind(WhichKind);
        }
    }

    public void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
