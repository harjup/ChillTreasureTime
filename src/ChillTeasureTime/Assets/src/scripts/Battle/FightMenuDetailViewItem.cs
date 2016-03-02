using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class FightMenuDetailViewItem : MonoBehaviour
{
    private bool _init;

    private Image _moveIcon;

    private Text _moveName;

    private Text _moveCost;


    public void Init()
    {
        _init = true;

        var imageComponents = GetComponentsInChildren<Image>();
        _moveIcon = imageComponents.First(i => i.name == "Icon");

        var textComponents = GetComponentsInChildren<Text>();
        _moveName = textComponents.First(t => t.name == "MoveName");
        _moveCost = textComponents.First(t => t.name == "MoveCost");

    }

    public void Set(PlayerFightDetailItem detail)
    {
        if (!_init)
        {
            _init = true;
            Init();
        }

        _moveName.text = detail.Name;
        _moveCost.text = detail.Cost;

        if (!string.IsNullOrEmpty(detail.IconPath))
        {
            var texture = Resources.Load<Sprite>(detail.IconPath);
            _moveIcon.sprite = texture;
        }

        // Set description on some other gui element
    }
}
