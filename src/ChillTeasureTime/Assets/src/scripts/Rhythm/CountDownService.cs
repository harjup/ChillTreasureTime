using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CountDownService : Singleton<CountDownService>, IBeat
{
    private int max = 4;
    public int Current;

    public void StartCountDown()
    {
        Current = max;
    }

    public void OnBeat(int bar, int beat)
    {
        Current -= 1;

        if (Current == 0)
        {
            Arrow.SetActive(true);
        }

        if (Current <= 0)
        {
            Image.SetActive(false);
            Text.gameObject.SetActive(false);
        }
        else
        {
            Image.SetActive(true);
            Text.gameObject.SetActive(true);
            Text.text = Current.ToString();
            Arrow.SetActive(false);
        }
    }

    public GameObject Image;
    public GameObject Arrow;
    public Text Text;

    public void Start()
    {
        Current = -2;
        Image = GameObject.Find("CountDownBg").gameObject;
        Text = GameObject.Find("CountDownText").GetComponent<Text>();
        Arrow = GameObject.Find("CountDownArrow");
        Arrow.SetActive(false);
    }

    public void EndOfSet()
    {
        Arrow.SetActive(false);
    }
}
