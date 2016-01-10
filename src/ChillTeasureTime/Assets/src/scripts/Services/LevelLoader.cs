﻿using System;
using UnityEngine;

public enum LevelEntrance
{
    Unknown,
    Start,
    BeachLeft,
    BeachRight,
    Lighthouse,
    VilliageLeft
}

public class LevelLoader : Singleton<LevelLoader>
{
    public void LoadLevel(LevelEntrance levelEntrance)
    {
        State.Instance.LevelEntrance = levelEntrance;

        switch (levelEntrance)
        {
            case LevelEntrance.BeachLeft:
            case LevelEntrance.BeachRight:
                Application.LoadLevel("Main");
                break;
            case LevelEntrance.Lighthouse:
                Application.LoadLevel("Lighthouse");
                break;
            case LevelEntrance.VilliageLeft:
                Application.LoadLevel("Villiage");
                break;
            default:
                Debug.LogError("Level entrance type " + levelEntrance + " not supported");
                break;
        }
    }
}