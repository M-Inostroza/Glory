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
    [SerializeField] Slider _staminaSlider;

    [SerializeField] Transform _playerTimer;
    [SerializeField] Transform _playerStats;

    [SerializeField] Image _playerActionIcon;

    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text hpText;

    [SerializeField] Transform[] _inputs;
    [SerializeField] Transform aKey;

    // Shield
    [SerializeField] Slider shieldBar;
    [SerializeField] TMP_Text shieldNumber;

    private string _slectedAction;
    private int _numberOfTries;

    Player _player;
    TargetManager _targetManager;
    defendManager _defendManager;
    AudioManager _audioManager;
    DialogueManager _dialogueManager;

    bool timerRunning = false;

    private void Awake()
    {
        _numberOfTries = 0;
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _staminaSlider.DOValue(_staminaSlider.maxValue, 1.5f);
        _targetManager = FindObjectOfType<TargetManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _player = FindObjectOfType<Player>();
        _defendManager = FindObjectOfType<defendManager>();
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
        updateShield();
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
        _staminaSlider.DOValue(_player.GetCurrentStamina(), 0.5f);
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

    public void toggleInput(int index, int inOut)
    {   // 1 = in
        if (inOut == 1)
        {
            _inputs[index].DOLocalMoveX(140, 0.7f);
        } else
        {
            _inputs[index].DOLocalMoveX(0, 0.5f);
        }
    }

    public void OnAttackButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("ATK1");
        timerRunning = true;
    }
    public void OnDefendButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("DF");
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
                _slectedAction = "DF";
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
        _numberOfTries++;
        fadeTimer(0);
        switch (action)
        {
            case "ATK1":
                tryLimit(2, 4.5f, 0);
                _player.DecrementCurrentStamina(25);
                _player.GetComponent<Animator>().Play("ATK_jump");
                _targetManager.attack();
                break;
            case "DF":
                _player.DecrementCurrentStamina(20);
                _defendManager.activateShieldMinigame();
                break;
        }
    }

    void tryLimit(int interaction, float delay, int inputIndex)
    {
        if (_numberOfTries == 1) 
        {
            toggleInput(inputIndex, 0);
            StartCoroutine(_dialogueManager.interactions(interaction, delay));
            _numberOfTries = 0;
        } 
    }

    public void fadeTimer(int inOrOut)
    {   // 0 = out - 1 = in
        float fadeTime = 0.1f;
        Image timer = _playerTimer.GetComponent<Image>();
        if (inOrOut == 0)
        {
            _playerActionIcon.DOFade(0, fadeTime);
            timer.DOFade(0, fadeTime);
        } else if (inOrOut == 1)
        {
            timer.fillAmount = 1;
            _playerActionIcon.DOFade(1, fadeTime);
            timer.DOFade(1, fadeTime);
        }
    }

    // Keys 
    public void activateA()
    {
        if (aKey.gameObject.activeInHierarchy)
        {
            aKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            aKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(() => activateA());
        }
    }

    // Shield
    void updateShield()
    {
        shieldBar.DOValue(_player.getCurrentShield(), .3f);
        shieldNumber.text = _player.getCurrentShield().ToString();
    }
}
