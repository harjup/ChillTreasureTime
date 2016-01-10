using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class State : Singleton<State>
{
    public LevelEntrance LevelEntrance;
    public int ShinyCount;
    public readonly List<Sequence> ImportantSequences = new List<Sequence>();

    public void SetLevelEntrance(LevelEntrance levelEntrance)
    {
        LevelEntrance = levelEntrance;
    }

    public void AddToShineyCount(int count)
    {
        ShinyCount += count;
    }

    public void AddToImportantSequences(Sequence seq)
    {
        ImportantSequences.Add(seq);
    }

    public bool AllImportantSequencesAreDone()
    {
        return ImportantSequences.All(i => !i.IsPlaying());
    }
}
