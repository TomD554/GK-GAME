using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Team", menuName = "Team")]
public class Teams : ScriptableObject
{

    public int ID;

    public string Name;

    public float Rating;

    public int HomeKitID;
    public int AwayKitID;

    public string Nation;

    public Sprite Logo;

    public Texture HomeKit;   // Used for now
    public Texture AwayKit;


}
