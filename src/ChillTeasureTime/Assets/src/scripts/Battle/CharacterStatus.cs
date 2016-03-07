using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour
{
    public int Hp { get; set; }
    public int HpMax { get; set; }
    public int Attack { get; set; }
    
    public int Defense { get; set; }

    public void Start()
    {
        Hp = 2;
        HpMax = 1;
        Attack = 1;
        Defense = 1;
    }
}
