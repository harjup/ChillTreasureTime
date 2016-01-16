using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class State : Singleton<State>
{
    public bool IsIntro = true;
    public bool FirstShinyCollected = false;
    public LevelEntrance LevelEntrance;

    public Dictionary<CollectableType, int> PlayerShinyCount = new Dictionary<CollectableType, int>
    {
        {CollectableType.Good, 0},
        {CollectableType.Worthless, 0}
    };

    public int StoredShinyCount;
    public readonly List<Sequence> ImportantSequences = new List<Sequence>();

    public void SetLevelEntrance(LevelEntrance levelEntrance)
    {
        LevelEntrance = levelEntrance;
    }

    public void AddToPlayerItemCount(int count, CollectableType type)
    {
        PlayerShinyCount[type] += count;
    }

    public int CashInShinyItem()
    {
        PlayerShinyCount[CollectableType.Good] -= 1;
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
