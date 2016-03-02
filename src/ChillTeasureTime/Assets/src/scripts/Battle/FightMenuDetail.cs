using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class FightMenuDetail : MonoBehaviour
{
    private GameObject _canvas;
    private GameObject _backdrop;

    private GameObject _moveDetailGameObject;

    public ScrubList<GameObject> ScrubMenuItems;


    private FightMenu _fightMenu;

    // Use this for initialization
    void Start()
    {
        _fightMenu = FindObjectOfType<FightMenu>();

        _canvas = transform.GetComponentInChildren<Canvas>().gameObject;
        _backdrop = GameObject.Find("Backdrop").gameObject;

        _moveDetailGameObject = Resources.Load<GameObject>("Prefabs/Battle/MoveDetailChoice");

        ShowChoices(new List<PlayerFightDetailItem> { new PlayerFightDetailItem
        {
            Name = "First", Cost = "1", IconPath = "Battle/MenuIcon/Battle-Flap"
        }, new PlayerFightDetailItem
        {
            Name = "Second", Cost = "1", IconPath = "Battle/MenuIcon/Battle-Peach"
        }, new PlayerFightDetailItem
        {
            Name = "Third", Cost = "1", IconPath = "Battle/MenuIcon/Battle-Peck"
        } });

        Disable();
    }

    public void ShowChoices(List<PlayerFightDetailItem> detailItems)
    {
        gameObject.SetActive(true);
        

        List<GameObject> detailGameObjects = new List<GameObject>();

        FindObjectsOfType<FightMenuDetailViewItem>().ToList().ForEach(m => Destroy(m.gameObject));

        for (int i = 0; i < detailItems.Count; i++)
        {
            var playerFightDetailItem = detailItems[i];

            var result = Instantiate(_moveDetailGameObject);
            result.name = playerFightDetailItem.Name;
            result.transform.SetParent(_backdrop.transform, false);

            result.transform.localPosition = result.transform.position.SetY(-76f/4 - (i * 96f)).SetX(-1.5f);

            result.GetComponent<FightMenuDetailViewItem>().Set(playerFightDetailItem);

            detailGameObjects.Add(result);
        } 

        ScrubMenuItems = new ScrubList<GameObject>(detailGameObjects);

    }

    public void Disable()
    {
        gameObject.SetActive(false);
    } 

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetCurrentMenuItem(ScrubMenuItems.Previous());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SetCurrentMenuItem(ScrubMenuItems.Next());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var pick = ScrubMenuItems.Current();
            Debug.Log(pick.name);
            Disable();
            _fightMenu.Enable();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Back to main menu");
            Disable();
            _fightMenu.Enable();
        }

    }

    public void SetCurrentMenuItem(GameObject menuItem)
    {
        menuItem.transform.localScale = Vector3.one * 110f;
        ScrubMenuItems.AllInactive().ForEach(s => s.transform.localScale = Vector3.one * 100);

        //var pos = menuItem.transform.position - transform.position;
        //var currentAngle = Mathf.Atan2(pos.y, pos.x);
        //var amountToRotate = Mathf.PI / 12 - currentAngle;

        //transform.DORotate(transform.rotation.eulerAngles + Mathf.Rad2Deg * Vector3.zero.SetZ(amountToRotate), .25f);

        //ScrubMenuItems.AllInactive().ForEach(c => c.SetInactive());
        //ScrubMenuItems.Current().SetActive();
    }
}
