using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthMeter : MonoBehaviour
{
    public Text NumberText { get; set; }

    public Image HpForeground { get; set; }

    private bool _init;
    private void Init()
    {
        if (!_init)
        {
            _init = true;

            NumberText = transform.FindChild("NumberText").GetComponent<Text>();
            HpForeground = transform.FindChild("HP-Foreground").GetComponent<Image>();
        }
    }

    public void Start()
    {
        Init();
    }

    public void SetAmount(int amount, int amountMax)
    {
        Init();

        NumberText.text = amount.ToString();
        HpForeground
            .rectTransform
            .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)amount / amountMax);
    }
}
