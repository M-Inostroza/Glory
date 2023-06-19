using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterManager : MonoBehaviour
{
    public GameObject shieldImage;
    public GameObject counterBullet;

    float rotationSpeed = 150;

    private void Start()
    {
        counterBullet.transform.DOLocalMoveX(-17, 6);
    }

    private void Update()
    {
        rotateOnKey();
        rotateOnStart();
    }

    void rotateOnKey()
    {
        if (Input.GetKey(KeyCode.X))
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            shieldImage.transform.Rotate(0f, 0f, rotationAmount);
        }
    }

    void rotateOnStart()
    {
        float rotationAmount = 50 * Time.deltaTime;
        shieldImage.transform.Rotate(0f, 0f, rotationAmount);
    }
}
