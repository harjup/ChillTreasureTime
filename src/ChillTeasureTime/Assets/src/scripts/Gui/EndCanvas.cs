using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class EndCanvas : MonoBehaviour
{
    void Start()
    {
         List<GuiDirection> GuiDirections = new List<GuiDirection>
        {
            new CrawlText("TopText"),
            new CrawlText("TreasureCollected"),
            new RevealAndShake("TreasureCollectedCount", State.Instance.StoredShinyCount.ToString()),
            new Reveal("TreasureWorth", (State.Instance.StoredShinyCount * 3.14f).ToString()),
            new CrawlText("TrashCount"),
            new RevealAndShake("TrashCollectedCount", State.Instance.PlayerShinyCount[CollectableType.Worthless].ToString()),
            new Reveal("TrashWorth"),
            new CrawlText("TimePlayed"),
            new RevealAndShake("TimePlayedCount", (State.Instance.TimeElapsed / 60d).ToString("0.00")),
            new Reveal("TimePlayedMinutes"),
            new CrawlText("Rank"),
            new RevealAndShake("RankValue"),
            new RevealAndShake("FIN")
        };

        foreach (var guiDirection in GuiDirections)
        {
            var target = transform.FindChild(guiDirection.Name).gameObject;
            guiDirection.GameObject = target;
            guiDirection.Text = target.GetComponent<Text>().text;
            guiDirection.GameObject.SetActive(false);
        }

        StartCoroutine(StartEndSequence(GuiDirections));  
    }

    IEnumerator StartEndSequence(List<GuiDirection> guiDirections)
    {
        foreach (var guiDirection in guiDirections)
        {
            var target = guiDirection.GameObject;

            if (guiDirection is CrawlText)
            {
                target.SetActive(true);
                var text = target.GetComponent<Text>();
                target.GetComponent<Text>().text = "";

                while (text.text.Length < guiDirection.Text.Length)
                {
                    text.text = guiDirection.Text.Substring(0, text.text.Length + 1);
                    yield return new WaitForSeconds(.1f);
                }
            }

            if (guiDirection is RevealAndShake)
            {
                target.SetActive(true);
                target.transform.DOShakePosition(.25f, 10, 20);

                var shake = guiDirection as RevealAndShake;
                if (shake.SetValue != null)
                {
                    target.GetComponent<Text>().text = shake.SetValue;
                }

                if (shake.Name == "RankValue")
                {
                    var textUi = target.GetComponent<Text>();
                    var timeInSeconds = (State.Instance.TimeElapsed / 60d);
                    var treasures = State.Instance.StoredShinyCount;
                    var trash = State.Instance.PlayerShinyCount[CollectableType.Worthless];

                    textUi.text = "Cool Dude";

                   
                    if (treasures >= 15)
                    {
                        textUi.text = "Collector";
                    }

                    if (trash >= 10)
                    {
                        textUi.text = "Best Janitor";
                    }

                    if (treasures >= 30)
                    {
                        textUi.text = "Hoarder";
                    }

                    if (timeInSeconds < 2)
                    {
                        textUi.text = "Speed Runner";
                    }

                    if (timeInSeconds > 30)
                    {
                        textUi.text = "Away From Keyboard";
                    }
                }

                yield return new WaitForSeconds(1f);
            }

            if (guiDirection is Reveal)
            {
                target.SetActive(true);
                var reveal = guiDirection as Reveal;
                if (reveal.SetValue != null)
                {
                   target.GetComponent<Text>().text = String.Format(target.GetComponent<Text>().text, reveal.SetValue);
                }
  

                yield return new WaitForSeconds(1f);
            }

        }
    }
}

public class GuiDirection
{
    public string Name { get; set; }

    public string Text { get; set; }

    public GameObject GameObject { get; set; }

    public GuiDirection(string name)
    {
        Name = name;
    }
}

public class Delay : GuiDirection
{
    public float Seconds { get; set; }

    public Delay(float seconds, string name = "") : base(name)
    {
        Seconds = seconds;
    }
}

public class CrawlText : GuiDirection
{
    public CrawlText(string name) : base(name)
    {
    }
}

public class RevealAndShake : GuiDirection
{
    public string SetValue { get; private set; }

    public RevealAndShake(string name, string setValue = null) : base(name)
    {
        SetValue = setValue;
    }
}

public class Reveal : GuiDirection
{
    public string SetValue { get; private set; }

    public Reveal(string name, string setValue = null)
        : base(name)
    {
        SetValue = setValue;
    }
}