using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMember : MonoBehaviour
{
    [SerializeField] private Team team;

    public Team Team { get { return team; } }

    public bool isHostileTo(Team other) => team.isHostileTo(other);
}
