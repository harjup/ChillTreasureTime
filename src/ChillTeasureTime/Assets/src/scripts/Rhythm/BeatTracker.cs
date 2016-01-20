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

public class PlayerBeat : Beat
{
    public bool Hit { get; set; }

    public float TimeCode { get ; set; }

    public PlayerBeat(int bar, int beat)
        : base(bar, beat)
    {
        TimeCode =  (60.0f/150f)*(((bar - 1) * 4) + beat);
        Debug.Log(TimeCode);
    }
}

public class BeatTracker : MonoBehaviour, IBeat, IBar
{
    private Beat _currentBeat;

    List<Beat> _beatSequence = new List<Beat>
    {
        new RivalBeat(4, 1),
        new RivalBeat(4, 2),
        new RivalBeat(4, 3),
        new RivalBeat(4, 4),
        new RivalBeat(5, 4),
        new PlayerStartBeat(5, 2),
        new PlayerBeat(6, 1),
        new PlayerBeat(6, 2),
        new PlayerBeat(6, 3),
        new PlayerBeat(6, 4),
        new PlayerBeat(7, 4),
        new PlayerStartBeat(7, 3),
        new RivalBeat(8, 1),
        new RivalBeat(8, 2),
        new RivalBeat(8, 3),
        new RivalBeat(8, 4),
        new RivalBeat(9, 4),
        new PlayerStartBeat(9, 3),
        new PlayerBeat(10, 1),
        new PlayerBeat(10, 2),
        new PlayerBeat(10, 3),
        new PlayerBeat(10, 4),
        new PlayerBeat(11, 4),

       
        
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
        if (_currentBeat is RivalBeat)
        {
            Debug.Log("CPU: Bar " + bar + "Beat " + localBeat);
            RhythmRival.Peck();
        }
    }

    public void OnValidInputForBeat(float time, float tolerance)
    {
        Debug.Log("Good Input");

        var actual = _beatSequence
            .Where(b => b is PlayerBeat)
            .Cast<PlayerBeat>()
            .Where(b => !b.Hit)
            .FirstOrDefault(b => Math.Abs(b.TimeCode - time) < tolerance);


        if (actual != null)
        {
            Debug.Log("Good Input2");
            RhythmBird.Peck();
            actual.Hit = true;
        }
        else
        {
            RhythmBird.Whoops();
        }
    }



    public void OnBar(int bar)
    {
    }

    public void OnInValidInputBeat()
    {
        RhythmBird.Whoops();
    }
}
