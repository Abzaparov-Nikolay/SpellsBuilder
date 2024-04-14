using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = MenuNames.Item + "Buff")]
public class Buff : ScriptableObject
{
    public PlayerStat Stat;
    public float Value;

    public string GetString()
    {
        return $"{Stat} : {(Value > 0 ? '+' : '-')}{Value}%";
    }
}
