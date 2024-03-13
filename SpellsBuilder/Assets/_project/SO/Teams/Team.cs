using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(Team))]
public class Team : ScriptableObject
{
    public bool isHostileTo(Team other) => this != other;
}
