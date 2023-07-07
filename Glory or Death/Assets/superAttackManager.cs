using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class superAttackManager : MonoBehaviour
{
    [SerializeField] Transform[] swordSpawners;
    [SerializeField] GameObject swordProyectilePrefab;

    [SerializeField] Transform heartTarget;

    public float bulletSpeed;

    private void Update()
    {
        rotateSpawners();
        spawnSwords();
    }
    
    void spawnSwords()
    {
        foreach (Transform spawner in swordSpawners)
        {
            var bullet = Instantiate(swordProyectilePrefab, spawner.transform);
        }
    }
    void rotateSpawners()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 180), 5).SetEase(Ease.Linear);
    }
}
