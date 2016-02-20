using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;

public class FightMenu : MonoBehaviour
{

    private GameObject FightMenuItemPrefab;

    public List<FightMenuItem> MenuItems = new List<FightMenuItem>();

    public List<MenuItemPosition> MenuItemPositions = new List<MenuItemPosition>(); 

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
    }


    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.DORotate(transform.rotation.eulerAngles + Vector3.zero.SetZ(-30f), .25f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.DORotate(transform.rotation.eulerAngles + Vector3.zero.SetZ(30f), .25f);
        }
    }
}
