using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeamDatabase", menuName = "Database/Team Database")]
public class TeamDatabase : ScriptableObject
{
    public List<Teams> allTeams;
}

