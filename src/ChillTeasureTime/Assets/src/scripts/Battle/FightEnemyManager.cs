using System;
using UnityEngine;
using System.Collections.Generic;

//TODO: Rename to relate to player attack selection and not what the enemy behaviors are.
public class FightEnemyManager : MonoBehaviour
{
    private GameObject _spawnStart;
    private GameObject _selectionArrow;
    public List<GameObject> Enemies;

    public ScrubList<GameObject> ActiveEnemies;

    private FightMenuDetail _fightMenuDetail;

    private FightWorkflow _fightWorkflow;

    public void Start()
    {
        _fightMenuDetail = FindObjectOfType<FightMenuDetail>();
        _fightWorkflow = FindObjectOfType<FightWorkflow>();
        _spawnStart = GameObject.Find("EnemySpawnStart").gameObject;

        var prefab = Resources.Load<GameObject>("Prefabs/Battle/SelectionArrow");
        _selectionArrow = Instantiate(prefab);
        _selectionArrow.name = "Enemy-Select-Arrow";

        Enemies = new List<GameObject>();

        var crabEnemy = new EnemyType
        {
            Id = "Crab",
            Name = "Crab",
            Defense = 0,
            Health = 2,
            PrefabPath = "Prefabs/Battle/CrabPinch"
        };

        var birdEnemy = new EnemyType
        {
            Id = "RedBird",
            Name = "RedBird",
            Defense = 0,
            Health = 2,
            PrefabPath = "Prefabs/Battle/RedBird"
        };

        var enemiesToSpawn = new List<EnemyType>();
        enemiesToSpawn.Add(crabEnemy);
        enemiesToSpawn.Add(crabEnemy);
        enemiesToSpawn.Add(birdEnemy);

        InitEnemies(_spawnStart.transform.position, enemiesToSpawn);

        DisableEnemySelect();
    }

    public void InitEnemies(Vector3 initialPosition, List<EnemyType> enemies)
    {
        var inc = 2;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            var prefab = Resources.Load<GameObject>(enemy.PrefabPath);
            var res = Instantiate(prefab) as GameObject;
            var x = initialPosition.x + (i*inc);
            res.name = enemy.Name + i.ToString("D2");
            res.transform.position = initialPosition.SetX(x);
            res.transform.parent = transform;

            Enemies.Add(res);
        }

        ActiveEnemies = new ScrubList<GameObject>(Enemies);
    }

    public void EnableEnemySelect()
    {
        _selectionArrow.SetActive(true);
        SetCurrentMenuItem(ActiveEnemies.First());
    }

    public void DisableEnemySelect()
    {
        _selectionArrow.SetActive(false);
    }


    public void Update()
    {
        if (_selectionArrow.activeSelf)
        {
            ArrowInput();
        }

    }

    public void ArrowInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetCurrentMenuItem(ActiveEnemies.Previous());
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetCurrentMenuItem(ActiveEnemies.Next());
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            var go = ActiveEnemies.Current();
            Debug.Log(go.name);
            DisableEnemySelect();
            _fightWorkflow.InitAttackSequence(go, "");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Back to main menu");
            DisableEnemySelect();
            _fightMenuDetail.Disable();
        }
    }

    public void SetCurrentMenuItem(GameObject enemy)
    {
        var targetY = enemy.transform.position.y;

        _selectionArrow.transform.position = enemy.transform.position.SetY(targetY + 1f);
    }

}
