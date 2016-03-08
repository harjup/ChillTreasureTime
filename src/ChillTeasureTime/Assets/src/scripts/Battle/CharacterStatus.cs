using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour
{
    public int Hp { get; set; }
    public int HpMax { get; set; }
    public int Attack { get; set; }
    
    public int Defense { get; set; }


    public HealthMeter HealthMeter {get; set; }

    public void Start()
    {
        Hp = 2;
        HpMax = 2;
        Attack = 1;
        Defense = 1;

        var prefab = Resources.Load<GameObject>("Prefabs/Battle/HealthBar");

        var go = Instantiate(prefab);
        go.transform.position = transform.position.SetY(transform.position.y + 1f);

        HealthMeter = go.GetComponent<HealthMeter>();
        HealthMeter.SetAmount(Hp, HpMax);
    }

    public void AddDamage(int amount)
    {
        Hp -= amount;
        if (Hp < 0)
        {
            Hp = 0;
        }

        HealthMeter.SetAmount(Hp, HpMax);
    }


}
