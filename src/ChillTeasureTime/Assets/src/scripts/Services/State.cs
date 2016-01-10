using UnityEngine;

public class State : Singleton<State>
{
    public LevelEntrance LevelEntrance;
    public int ShinyCount;


    public void SetLevelEntrance(LevelEntrance levelEntrance)
    {
        LevelEntrance = levelEntrance;
    }

    public void AddToShineyCount(int count)
    {
        ShinyCount += count;
    }

}
