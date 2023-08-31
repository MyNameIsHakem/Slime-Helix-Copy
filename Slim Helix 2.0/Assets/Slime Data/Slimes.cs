using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Slime" , menuName = "Slime")]
public class Slimes : ScriptableObject
{
    public SlimeType Types;   

    public Material Mat;

    public GameObject DeathEffect;

    public int Lives;

    public bool Attack;
}
