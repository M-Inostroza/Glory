using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Tutorial_UI : MonoBehaviour
{
    [SerializeField] Sprite[] iconSprites;

    [SerializeField] Transform _playerStamina;
    [SerializeField] Transform _playerTimer;
    [SerializeField] Transform _playerStats;

    [SerializeField] Image _playerActionIcon;

    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text hpText;

    [SerializeField] Transform[] _inputs;

    private string _slectedAction;

    Player _player;
    TargetManager _targetManager;
    AudioManager _audioManager;

    bool timerRunning = false;

    private void Awake()
    {
        _targetManager = FindObjectOfType<TargetManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _player = FindObjectOfType<Player>();
    }
    private void Start()
    {
        showUI();
    }
    private void Update()
    {
        reduceTimer();
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

    public void showInput(int index)
    {
        _inputs[index].DOLocalMoveX(140, 0.7f);
    }

    public void OnAttackButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("ATK1");
        timerRunning = true;
    }

    void reduceTimer()
    {
        Image timer = _playerTimer.GetComponent<Image>();
        if (timerRunning)
        {
            timer.fillAmount -= Time.deltaTime / 2;
            if (timer.fillAmount <= 0)
            {
                executeAction(_slectedAction);
                timerRunning = false;
            }
        }
    }

    public void selectIcon(string icon)
    {
        switch (icon)
        {
            case "ATK1":
                _slectedAction = "ATK1";
                _playerActionIcon.sprite = iconSprites[1];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "ATK2":
                _playerActionIcon.sprite = iconSprites[6];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "DF":
                _playerActionIcon.sprite = iconSprites[2];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "DG":
                _playerActionIcon.sprite = iconSprites[3];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "FC":
                _playerActionIcon.sprite = iconSprites[4];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "RST":
                _playerActionIcon.sprite = iconSprites[5];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "Default":
                _playerActionIcon.sprite = iconSprites[0];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
        }
    }

    void executeAction(string action)
    {
        switch (action)
        {
            case "ATK1":
                _player.GetComponent<Animator>().Play("ATK_jump");
                _targetManager.attack();
                break;
        }
    }
}
