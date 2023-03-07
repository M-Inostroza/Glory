using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumables", menuName = "ScripteableObjects/Consumables")]
public class SO_Consumables : ScriptableObject
{
    public int consumableID;
    public int consumableName;
    public enum consumableType {Potion, Boost}
    public int valor;
    public int price;
    public Sprite consumableSprite;
}
