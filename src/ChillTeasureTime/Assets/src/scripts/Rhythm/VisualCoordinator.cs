using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using JetBrains.Annotations;

public class VisualCoordinator : MonoBehaviour
{

    [SerializeField]
    private GameObject _spritePrefab;


    [UsedImplicitly]
	void Start()
    {
        var sprites = GenerateSprites(_spritePrefab);

        ChildToElement(sprites, "BeatSpots");
	}

    private GameObject[] GenerateSprites(GameObject prefab)
    {
        return 
            new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 }
            .Select(i => new Vector3((-7.5f + (i * 2.125f)), 0f))
            .Select(v => Instantiate(prefab, v, Quaternion.identity))
            .Cast<GameObject>()
            .ToArray();
    }

    private void ChildToElement(IEnumerable<GameObject> objs, string elementName)
    {
        var target = GameObject.Find(elementName) ?? new GameObject(elementName);
        objs.ToList().ForEach(o => o.transform.SetParent(target.transform));
    }
}
