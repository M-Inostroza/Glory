using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapons", menuName ="ScripteableObjects/Weapons")]
public class SO_Weapons : ScriptableObject
{
    public int weaponID;
    public string weaponName;
    public int weaponLevel;
    public string weaponType;
    public int weaponDamage;
    public int price;
    public Sprite weaponSprite;
}
