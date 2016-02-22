using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

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

    private Vector3 _initialScale;

    public Vector3 InitialScale
    {
        get
        {
            if (_initialScale == Vector3.zero)
            {
                _initialScale = transform.localScale;
            }

            return _initialScale;
        }
    }

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

    public void SetInactive()
    {
        GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, .3f);
        transform.localScale = InitialScale * .75f;
    }

    public void SetActive()
    {
        GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        transform.localScale = InitialScale * 1.1f;
    }
}
