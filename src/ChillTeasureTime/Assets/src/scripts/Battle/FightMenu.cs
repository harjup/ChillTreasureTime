using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;

/// <summary>
/// Step 1, displays Fight Menu Items
/// </summary>
public class FightMenu : MonoBehaviour
{

    private GameObject FightMenuItemPrefab;

    public List<FightMenuItem> MenuItems = new List<FightMenuItem>();

    public FightMenuDetail FightMenuDetail;

    public Dictionary<FightMenuItem.Kind, List<PlayerFightDetailItem>> DetailItems 
        = new Dictionary<FightMenuItem.Kind, List<PlayerFightDetailItem>>
    {
        {
            FightMenuItem.Kind.Flap, new List<PlayerFightDetailItem>{
                new PlayerFightDetailItem
                {
                    Name = "Wing Flap", Description = "Execute a normal wing flap", Id = "WING", IconPath = "Battle/MenuIcon/Battle-Flap"
                }, 
                new PlayerFightDetailItem
                {
                    Name = "Super Wing Flap", Description = "It's like a wing flap but 'super'.", Id = "WING_SUPER", IconPath = "Battle/MenuIcon/Battle-Flap"
                }}
        
        },
        {
            FightMenuItem.Kind.Peck, new List<PlayerFightDetailItem>{
                new PlayerFightDetailItem
                {
                    Name = "Peck", Description = "Peck your beak hard and fast", Id = "PECK", IconPath = "Battle/MenuIcon/Battle-Peck"
                }, 
                new PlayerFightDetailItem
                {
                    Name = "Double Peck", Description = "Pick your beak twice! With meaning!.", Id = "PECK_DBL", IconPath = "Battle/MenuIcon/Battle-Peck"
                }}
        
        },
        {
            FightMenuItem.Kind.Item, new List<PlayerFightDetailItem>{
                new PlayerFightDetailItem
                {
                    Name = "Apple", Description = "Red and delicious", Id = "APPLE", IconPath = "Battle/MenuIcon/Battle-Item"
                }, 
                new PlayerFightDetailItem
                {
                    Name = "Orange", Description = "A sweet treat in its own wrapper", Id = "ORANGE", IconPath = "Battle/MenuIcon/Battle-Item"
                }, 
                new PlayerFightDetailItem
                {
                    Name = "Banana", Description = "A Kong favorite", Id = "BANANA", IconPath = "Battle/MenuIcon/Battle-Item"
                }}
        
        },
        {
            FightMenuItem.Kind.Run, new List<PlayerFightDetailItem>{
                new PlayerFightDetailItem
                {
                    Name = "Run", Description = "Run away to fight another day", Id = "RUN", IconPath = "Battle/MenuIcon/Battle-Run"
                }}
        
        }
    }; 

    public ScrubList<FightMenuItem> ScrubMenuItems; 

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

        FightMenuDetail = FindObjectOfType<FightMenuDetail>();
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
            var kind = ScrubMenuItems.Current().WhichKind;
            Debug.Log(kind);

            // Activate detail item view
            // Disable own view
            
            // ~ Inside detail item menu... ~
            //   Generate list of available detail items and select first
            //   Move arrow based on select
            // Can go back to prev menu with cancel button
            // Can proceed to target select after picking and item

            var detailItems = DetailItems[kind];

            FightMenuDetail.ShowChoices(detailItems);

            Disable();
        }
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

}