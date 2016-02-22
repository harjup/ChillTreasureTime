using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;

public class FightMenu : MonoBehaviour
{

    private GameObject FightMenuItemPrefab;

    public List<FightMenuItem> MenuItems = new List<FightMenuItem>();

    public ScrubList<FightMenuItem> ScrubMenuItems; 

    public List<MenuItemPosition> MenuItemPositions = new List<MenuItemPosition>();

    public FightMenuItem CurrentMenuItem;
    // Current item
    // 

    void Start()
    {
        var menuItemKinds = FightMenuItem.GetAllKinds();

        var initialPosition = transform.position;
        var initialRadians = 0;
        var radianIncrement = Mathf.PI / 6;
        var radius = 2.5f;


        FightMenuItemPrefab = Resources.Load<GameObject>("Prefabs/Battle/MenuItem-Default");

        for (int i = 0; i < menuItemKinds.Count; i++)
        {
            var res = Instantiate(FightMenuItemPrefab);
            var menuItem = res.GetComponent<FightMenuItem>();
            menuItem.SetKind(menuItemKinds[i]);
            menuItem.transform.parent = transform;
            MenuItems.Add(menuItem);

            var x = radius*Mathf.Cos(initialRadians + (radianIncrement*i));
            var y = radius*Mathf.Sin(initialRadians + (radianIncrement*i));

            menuItem.transform.position = transform.position + new Vector3(x, y, 0f);   
        }

        ScrubMenuItems = new ScrubList<FightMenuItem>(MenuItems);

        SetCurrentMenuItem(ScrubMenuItems.Current());
    }

    public void SetCurrentMenuItem(FightMenuItem menuItem)
    {
        var pos = menuItem.transform.position - transform.position;
        var currentAngle = Mathf.Atan2(pos.y, pos.x);
        var amountToRotate = Mathf.PI/12 - currentAngle;

        transform.DORotate(transform.rotation.eulerAngles + Mathf.Rad2Deg * Vector3.zero.SetZ(amountToRotate), .25f);

        ScrubMenuItems.AllInactive().ForEach(c => c.SetInactive());
        ScrubMenuItems.Current().SetActive();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCurrentMenuItem(ScrubMenuItems.Previous());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetCurrentMenuItem(ScrubMenuItems.Next());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
          Debug.Log(ScrubMenuItems.Current().WhichKind);
        }
    }
}

public class ScrubList<T>
{
    private List<T> _items;

    private int _index;

    public ScrubList(List<T> items)
    {
        _items = items;
        _index = 0;
    }

    public List<T> All()
    {
        return _items;
    }

    public List<T> AllInactive()
    {
        return _items.Except(new List<T> {Current()}).ToList();
    } 

    public T Current()
    {
        return _items[_index];
    }

    public T Next()
    {
        var nextIndex = _index + 1;
        if (nextIndex <= _items.Count - 1)
        {
            _index = nextIndex;
        }

        return _items[_index];
    }

    public T Previous()
    {
        var nextIndex = _index - 1;
        if (nextIndex >= 0)
        {
            _index = nextIndex;
        }

        return _items[_index];
    }
}
