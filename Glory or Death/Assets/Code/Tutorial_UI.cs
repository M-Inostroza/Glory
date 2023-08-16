using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tutorial_UI : MonoBehaviour
{
    [SerializeField] Transform _playerStamina;
    [SerializeField] Transform _playerTimer;
    [SerializeField] Transform _playerStats;
    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text hpText;

    [SerializeField] Transform[] _inputs;

    Player _player;
    private void Awake()
    {
        _player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        showUI();
    }
    private void Update()
    {
        textHP();
        refillStamina();
    }
    void showUI()
    {
        float move_in_speed = 0.3f;

        _playerStamina.DOLocalMoveX(_playerStamina.localPosition.x + 200, move_in_speed).SetEase(Ease.InOutSine);
        _playerStats.DOLocalMoveX(_playerStats.localPosition.x + 350, move_in_speed).SetEase(Ease.InOutSine);
        _playerTimer.DOLocalMoveY(_playerTimer.localPosition.y - 160, move_in_speed);
        //_inputManager.transform.DOLocalMoveX(-435, move_in_speed).SetEase(Ease.InOutSine);
    }
    void refillStamina()
    {
        staminaText.text = ((int)_player.GetCurrentStamina()).ToString() + " / " + ((int)_player.GetMaxStamina()).ToString();
        if (_player.GetCurrentStamina() < _player.GetMaxStamina())
        {
            _player.IncrementCurrentStamina(0.5f * Time.deltaTime);  //Mejorable
        }
    }
    void textHP()
    {
        hpText.text = _player.GetCurrentHP().ToString() + " / " + _player.GetMaxHP().ToString();
    }
}
