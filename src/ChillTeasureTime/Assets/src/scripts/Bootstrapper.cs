using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bootstrapper : MonoBehaviour
{

    private static bool _initialized;

    private GameObject ServiceRoot;

    public void Start()
    {
        if (_initialized == false)
        {
            ServiceRoot = new GameObject("ServiceRoot");
            DontDestroyOnLoad(ServiceRoot);
            GenerateServices();
            _initialized = true;
        }
    }

    // Make sure these are all monobehaviors or bad things will happen
    // These are all services that need to exist somewhere and require no configuration
    private List<Type> SimpleServices = new List<Type>
    {
        typeof(LevelLoader),
        typeof(TextLoader),
        typeof(DialogService),
        typeof(DirectionService)
    };


    private void GenerateServices()
    {
        foreach (var expectedService in SimpleServices)
        {
            ServiceRoot.AddComponent(expectedService);
        }

        var singlePrefabs = Resources.LoadAll<GameObject>("SinglePrefabs");

        foreach (var singlePrefab in singlePrefabs)
        {
            DontDestroyOnLoad(Instantiate(singlePrefab));
        }
    }
}
