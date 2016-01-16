using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class State : Singleton<State>
{
    public bool IsIntro = true;
    public LevelEntrance LevelEntrance;
    public int PlayerShinyCount;
    public int StoredShinyCount;
    public readonly List<Sequence> ImportantSequences = new List<Sequence>();

    public void SetLevelEntrance(LevelEntrance levelEntrance)
    {
        LevelEntrance = levelEntrance;
    }

    public void AddToPlayerShinyCount(int count)
    {
        PlayerShinyCount += count;
    }

    public int CashInShinyItem()
    {
        PlayerShinyCount -= 1;
        StoredShinyCount += 1;
        return StoredShinyCount;
    }

    //TODO: Figure out a more generic way to determining if all our coroutines and shit are done
    public void AddToImportantSequences(Sequence seq)
    {
        ImportantSequences.Add(seq);
    }

    public bool AllImportantSequencesAreDone()
    {
        return ImportantSequences.All(i => !i.IsPlaying());
    }
}
