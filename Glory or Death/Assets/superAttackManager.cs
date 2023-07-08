using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class superAttackManager : MonoBehaviour
{
    [SerializeField] Transform[] swordSpawners;
    [SerializeField] GameObject swordProyectilePrefab, parentCanvas;

    [SerializeField] Transform heartTarget, shield;

    timeManager timeManager;

    public float bulletSpeed;
    public int swordNumber;
    public float rotationSpeed;

    private void Update()
    {
        rotateSpawners();
        rotateOnKey();
    }
    private void Start()
    {
        timeManager = FindObjectOfType<timeManager>();
        StartCoroutine(SpawnSwordsWithDelay(0.5f));
        timeManager.executeSlowMotion(5, 0.6f);
    }

    IEnumerator SpawnSwordsWithDelay(float delay)
    {
         
        for (int i = 0; i < swordNumber; i++)
        {
            var randomNumber = Random.Range(0, swordSpawners.Length);
            Transform randomSpawner = swordSpawners[randomNumber];
            var bullet = Instantiate(swordProyectilePrefab, new Vector3(randomSpawner.transform.localPosition.x, randomSpawner.transform.localPosition.y, 0), Quaternion.identity, parentCanvas.transform);
            PointSpriteTowards(heartTarget.localPosition, bullet.transform);
            bullet.transform.DOMove(new Vector3(transform.position.x, transform.position.y, 0), 2);

            yield return new WaitForSeconds(delay);
        }
    }
    void rotateSpawners()
    {
        transform.DOLocalRotate(new Vector3(0, 0, 180), 5).SetEase(Ease.Linear);
    }

    void PointSpriteTowards(Vector3 targetPosition, Transform bullet)
    {
        Vector3 direction = targetPosition - bullet.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }

    void rotateOnKey()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            shield.transform.Rotate(0f, 0f, -rotationAmount);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            shield.transform.Rotate(0f, 0f, rotationAmount);
        }
    }
}
