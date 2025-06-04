using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NationDatabase", menuName = "Database/Nation Database")]
public class NationDatabase : ScriptableObject
{
    public List<Nations> allNations;
}
