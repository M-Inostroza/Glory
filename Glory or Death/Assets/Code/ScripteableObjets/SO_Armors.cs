using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armors", menuName = "ScripteableObjects/Armors")]
public class SO_Armors : ScriptableObject
{
    public int armorID;
    public string armorName;
    public enum armorType {Head, Chest, Leg, Boots}
    public int damageReduction;
    public int price;
    public Sprite armor;
}
