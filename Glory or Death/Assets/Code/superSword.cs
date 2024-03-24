using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AssetKits.ParticleImage;
using UnityEngine.UI;

public class superSword : MonoBehaviour
{
    SoundPlayer soundPlayer;
    SuperCounterManager SuperCounterManager;
    cameraManager CameraManager;

    GameObject _hitEffect;

    private void Start()
    {
        SuperCounterManager = FindObjectOfType<SuperCounterManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
        CameraManager = FindObjectOfType<cameraManager>();
        _hitEffect = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "shield")
        {
            _hitEffect.SetActive(true);
            CameraManager.PlayBloom(1, 0.4f);
            soundPlayer.shield_metal();
            gameObject.transform.DOKill();
            transform.GetComponent<Image>().DOFade(0, 0.1f);
        }
        else if (collision.name == "Heart")
        {
            CameraManager.PlayBloom(2, 0.3f);
            SuperCounterManager.fillSword();
            soundPlayer.stabSounds();
            gameObject.transform.DOKill();
            Destroy(gameObject);
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
