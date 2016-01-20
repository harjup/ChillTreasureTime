using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public interface IBeat
{
    void OnBeat(int bar, int beat);
}
public interface IBar
{
    void OnBar(int bar);
}


public class Beat
{
    public int BarNumber{ get; set; }
    public int BeatNumber { get; set; }

    public Beat(int bar, int beat)
    {
        BarNumber = bar;
        BeatNumber = beat;
    }
}

public class RivalBeat : Beat
{
    public RivalBeat(int bar, int beat) : base(bar, beat)
    {
    }
}

public class PlayerStartBeat : Beat
{
    public PlayerStartBeat(int bar, int beat) : base(bar, beat)
    {
    }
}

public class PlayerEndBeat : Beat
{
    public PlayerEndBeat(int bar, int beat)
        : base(bar, beat)
    {
    }
}

public class PlayerBeat : Beat
{
    public bool Hit { get; set; }

    public float TimeCode { get ; set; }

    public PlayerBeat(int bar, int beat)
        : base(bar, beat)
    {
        TimeCode =  (60.0f/150f)*(((bar - 1) * 4) + beat);
    }
}

public class FadeOutBeat : Beat
{
    public FadeOutBeat(int bar, int beat) : base(bar, beat)
    {
    }
}

public class BeatTracker : MonoBehaviour, IBeat
{
    private Beat _currentBeat;

    List<Beat> _beatSequence = new List<Beat>
    {
        new RivalBeat(8, 1),
        new RivalBeat(8, 2),
        new RivalBeat(8, 3),
        new RivalBeat(8, 4),
        new PlayerStartBeat(9, 1),
        new PlayerBeat(10, 1),
        new PlayerBeat(10, 2),
        new PlayerBeat(10, 3),
        new PlayerBeat(10, 4),
        new PlayerEndBeat(11, 2),

   
        new RivalBeat(12, 1),
        new RivalBeat(12, 2),
        new RivalBeat(12, 3),
        new RivalBeat(12, 4),
        new PlayerStartBeat(13, 1),
        new PlayerBeat(14, 1),
        new PlayerBeat(14, 2),
        new PlayerBeat(14, 3),
        new PlayerBeat(14, 4),
        new PlayerEndBeat(15, 2),

        new RivalBeat(12, 1),
        new RivalBeat(12, 2),
        new RivalBeat(12, 3),
        new RivalBeat(12, 4),
        new PlayerStartBeat(13, 1),
        new PlayerBeat(14, 1),
        new PlayerBeat(14, 2),
        new PlayerBeat(14, 3),
        new PlayerBeat(14, 4),
        new PlayerEndBeat(15, 2),


        new RivalBeat(16, 1),
        new RivalBeat(16, 3),
        new RivalBeat(17, 1),
        new RivalBeat(17, 3),
        new PlayerStartBeat(17, 1),
        new PlayerBeat(18, 1),
        new PlayerBeat(18, 3),
        new PlayerBeat(19, 1),
        new PlayerBeat(19, 3),

        new PlayerEndBeat(20, 2),
        
        new RivalBeat(21, 1),
        new RivalBeat(21, 2),
        new RivalBeat(22, 1),
        new RivalBeat(22, 2),
        new PlayerStartBeat(22, 1),
        new PlayerBeat(23, 1),
        new PlayerBeat(23, 2),
        new PlayerBeat(24, 1),
        new PlayerBeat(24, 2),
        new PlayerEndBeat(25, 2),


        new RivalBeat(26, 1),
        new RivalBeat(26, 3),
        new RivalBeat(26, 4),
        new PlayerStartBeat(27, 1),
        new PlayerBeat(28, 1),
        new PlayerBeat(28, 3),
        new PlayerBeat(28, 4),
        new PlayerEndBeat(30, 2),

        new RivalBeat(31, 1),
        new RivalBeat(31, 3),
        new RivalBeat(32, 1),
        new RivalBeat(32, 2),
        new RivalBeat(32, 3),
        new RivalBeat(32, 4),
        new PlayerStartBeat(33, 1),
        new PlayerBeat(34, 1),
        new PlayerBeat(34, 3),
        new PlayerBeat(35, 1),
        new PlayerBeat(35, 2),
        new PlayerBeat(35, 3),
        new PlayerBeat(35, 4),
        new PlayerEndBeat(36, 2),

        new RivalBeat(37, 1),
        new RivalBeat(37, 3),
        new RivalBeat(38, 1),
        new RivalBeat(38, 2),
        new RivalBeat(38, 3),
        new RivalBeat(38, 4),
        new PlayerStartBeat(39, 1),
        new PlayerBeat(40, 1),
        new PlayerBeat(40, 3),
        new PlayerBeat(41, 1),
        new PlayerBeat(41, 2),
        new PlayerBeat(41, 3),
        new PlayerBeat(41, 4),
        new PlayerEndBeat(42, 2),

        new RivalBeat(43, 1),
        new RivalBeat(43, 2),
        new RivalBeat(43, 3),
        new RivalBeat(43, 4),
        new PlayerStartBeat(44, 1),
        new PlayerBeat(45, 1),
        new PlayerBeat(45, 2),
        new PlayerBeat(45, 3),
        new PlayerBeat(45, 4),
        new PlayerEndBeat(46, 2),

        new RivalBeat(47, 1),
        new RivalBeat(47, 2),
        new RivalBeat(47, 3),
        new RivalBeat(47, 4),
        new PlayerStartBeat(48, 1),
        new PlayerBeat(49, 1),
        new PlayerBeat(49, 2),
        new PlayerBeat(49, 3),
        new PlayerBeat(49, 4),
        new PlayerEndBeat(50, 2),
        
        new FadeOutBeat(54, 1)
    };

    public RhythmRival RhythmRival;
    public RhythmBird RhythmBird;

    public void OnBeat(int bar, int beat)
    {
        if (_currentBeat is PlayerBeat)
        {
            var pb = _currentBeat as PlayerBeat;
            if (!pb.Hit)
            {
                RhythmBird.Whoops();
            }
        }

        var localBeat = beat - (bar - 1) * 4;
        _currentBeat = _beatSequence.FirstOrDefault(b => b.BarNumber == bar && b.BeatNumber == localBeat);

        var beats = _beatSequence.Where(b => b.BarNumber == bar && b.BeatNumber == localBeat);
        foreach (var b in beats)
        {
            if (b is RivalBeat)
            {
                _currentBeat = b;
                RhythmRival.Peck();
            }

            if (b is PlayerStartBeat)
            {
                CountDownService.Instance.StartCountDown();
                RhythmBird.MistakesInRound = 0;
            }

            if (b is PlayerEndBeat)
            {
                CountDownService.Instance.EndOfSet();
                RhythmBird.EndAnim();
            }

            if (b is FadeOutBeat)
            {
                StartCoroutine(EndScene());
            }
        }
    }

    IEnumerator EndScene()
    {
        yield return SceneFadeInOut.Instance.EndScene();
        FindObjectOfType<Player>().EnableControl();
        GuiCanvas.Instance.EnableOverworldUi();
        LevelLoader.Instance.LoadLevel(LevelEntrance.BeachRival);
    }

    public void OnValidInputForBeat(float time, float tolerance)
    {
        var actual = _beatSequence
            .Where(b => b is PlayerBeat)
            .Cast<PlayerBeat>()
            .Where(b => !b.Hit)
            .FirstOrDefault(b => Math.Abs(b.TimeCode - time) < tolerance);


        if (actual != null)
        {
            RhythmBird.Peck();
            actual.Hit = true;
        }
        else
        {
            RhythmBird.Whoops();
        }
    }

    public void OnInValidInputBeat()
    {
        RhythmBird.Whoops();
    }
}
