using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ScriptableObject> consumiblesInventory = new List<ScriptableObject>();
    public List<ScriptableObject> armorsInventory = new List<ScriptableObject>();
    public List<ScriptableObject> weaponsInventory = new List<ScriptableObject>();
    public List<ScriptableObject> materialsInventory = new List<ScriptableObject>();
    // Start is called before the first frame update
    void Awake()
    {
        LoadInventoryData();
    }

    public void AddConsumible(ScriptableObject newConsumable)
    {
        consumiblesInventory.Add(newConsumable);
    }
    public void RemoveConsumible(ScriptableObject consumible)
    {
        consumiblesInventory.Remove(consumible);
    }
    public void AddArmor(ScriptableObject newArmor)
    {
        armorsInventory.Add(newArmor);
    }
    public void RemoveArmor(ScriptableObject armor)
    {
        armorsInventory.Remove(armor);
    }
    public void Addweapon(ScriptableObject newWeapon)
    {
        weaponsInventory.Add(newWeapon);
    }
    public void RemoveWeapon(ScriptableObject weapon)
    {
        weaponsInventory.Remove(weapon);
    }
    public void AddMaterial(ScriptableObject newMaterial)
    {
        materialsInventory.Add(newMaterial);
    }
    public void RemoveMaterial(ScriptableObject material)
    {
        materialsInventory.Remove(material);
    }
    
    private void LoadInventoryData()
    {

    }
}
