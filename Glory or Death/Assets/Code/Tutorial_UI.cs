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
    [SerializeField] Transform _staminaAlarm;
    [SerializeField] Slider _staminaSlider;

    [SerializeField] Transform _playerTimer;
    [SerializeField] Transform _playerStats;

    [SerializeField] Image _playerActionIcon;

    [SerializeField] TMP_Text staminaText;
    [SerializeField] TMP_Text hpText;

    [SerializeField] Transform[] _inputs;
    [SerializeField] Transform aKey;
    [SerializeField] Transform sKey;
    [SerializeField] Transform xKey;
    [SerializeField] Transform leftKey;
    [SerializeField] Transform rightKey;

    // Shield
    [SerializeField] Slider shieldBar;
    [SerializeField] TMP_Text shieldNumber;

    static GameObject _overlay;
    static GameObject _cursorContainer;

    private string _slectedAction;
    private int _numberOfTries;

    Player _player;
    TargetManager _targetManager;
    defendManager _defendManager;
    AudioManager _audioManager;
    DialogueManager _dialogueManager;
    [SerializeField] GameObject _dodgeManager;
    [SerializeField] GameObject _focusManager;
    [SerializeField] GameObject _restManager;
    [SerializeField] GameObject _counterManager;
    [SerializeField] GameObject _superCounterManager;
    [SerializeField] GameObject _superAttackManager;
    [SerializeField] GameObject _dirtManager;


    public bool timerRunning = false;
    public static bool _hasPlayedTutorial = false;
    public static bool _canClick = true;

    private void Awake()
    {
        _numberOfTries = 0;
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _staminaSlider.DOValue(_staminaSlider.maxValue, 1.5f);
        _targetManager = FindObjectOfType<TargetManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _player = FindObjectOfType<Player>();
        _defendManager = FindObjectOfType<defendManager>();

        _overlay = transform.GetChild(3).gameObject;
        _cursorContainer = transform.GetChild(10).gameObject;
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
    public void OnDodgeButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("DG");
        timerRunning = true;
    }

    public void OnFocusButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("FC");
        timerRunning = true;
    }
    public void OnRestButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("RST");
        timerRunning = true;
    }
    public void OnSuperATKButton()
    {
        _audioManager.Play("UI_select");
        selectIcon("ATK2");
        timerRunning = true;
    }

    // Enemy
    public void OnCounterButton()
    {
        if (_canClick)
        {
            if (_hasPlayedTutorial)
                showAllInput(0);
            _audioManager.Play("UI_select");
            _counterManager.SetActive(true);
            fadeTimer(0);
            if (!_hasPlayedTutorial)
                tryLimit(7, 4, 6, 2);
        }
    }

    public void OnSuperCounterButton()
    {
        if (_canClick)
        {
            if (_hasPlayedTutorial)
                showAllInput(0);
            _audioManager.Play("UI_select");
            _superCounterManager.SetActive(true);
            fadeTimer(0);
            if (!_hasPlayedTutorial)
            {
                tryLimit(8, 8, 7, 2);
            }
        }
    }

    public void OnDirtButton()
    {
        if (_canClick)
        {
            _canClick = false;
            if (_hasPlayedTutorial)
                showAllInput(0);
            _audioManager.Play("UI_select");
            _dirtManager.SetActive(true);
            fadeTimer(0);
            if (!_hasPlayedTutorial)
                tryLimit(10, 4, 8, 3);
        }
    }


    void reduceTimer()
    {
        Image timer = _playerTimer.GetComponent<Image>();
        if (timerRunning)
        {
            _canClick = false;
            timer.fillAmount -= Time.deltaTime / 2;
            if (timer.fillAmount <= 0)
            {
                executeAction(_slectedAction);
                timerRunning = false;
                _canClick = true;
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
                _slectedAction = "ATK2";
                _playerActionIcon.sprite = iconSprites[6];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "DF":
                _slectedAction = "DF";
                _playerActionIcon.sprite = iconSprites[2];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "DG":
                _slectedAction = "DG";
                _playerActionIcon.sprite = iconSprites[3];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "FC":
                _slectedAction = "FC";
                _playerActionIcon.sprite = iconSprites[4];
                timeManager.animateIcon(_playerActionIcon.transform);
                break;
            case "RST":
                _slectedAction = "RST";
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
        Image timer = _playerTimer.GetComponent<Image>();
        void showAlarm()
        {
            alarmStamina();
            timer.fillAmount = 1;
            selectIcon("Default");
        }
        switch (action)
        {
            case "ATK1":
                if (_hasPlayedTutorial)
                {
                    if (_player.GetCurrentStamina() > 15)
                    {
                        fadeTimer(0);
                        _player.DecrementCurrentStamina(15);
                        attack();
                    } else
                    {
                        showAlarm();
                    }
                } else
                {
                    fadeTimer(0);
                    _player.DecrementCurrentStamina(15);
                    attack();
                    tryLimit(2, 4, 0, 3);
                }
                void attack() {
                    StartCoroutine(timeManager.slowMotion(1.8f, 0.5f));
                    _player.GetComponent<Animator>().Play("ATK_jump");
                    _targetManager.attack();
                }
                break;
            case "ATK2":
                if (_hasPlayedTutorial)
                {
                    if (_player.GetCurrentStamina() > 60)
                    {
                        fadeTimer(0);
                        _player.DecrementCurrentStamina(60);
                        _superAttackManager.SetActive(true);
                    } else
                    {
                        showAlarm();
                    }
                } else
                {
                    fadeTimer(0);
                    tryLimit(9, 4, 5, 2);
                    _player.DecrementCurrentStamina(60);
                    _superAttackManager.SetActive(true);
                }
                break;
            case "DF":
                if (_hasPlayedTutorial)
                {
                    if (_player.GetCurrentStamina() > 10)
                    {
                        fadeTimer(0);
                        _player.DecrementCurrentStamina(10);
                        _defendManager.activateShieldMinigame();
                    }
                    else
                    {
                        showAlarm();
                    }
                }
                else
                {
                    fadeTimer(0);
                    tryLimit(3, 4, 1, 2);
                    _player.DecrementCurrentStamina(10);
                    _defendManager.activateShieldMinigame();
                }
                break;
            case "DG":
                if (_hasPlayedTutorial)
                {
                    if (_player.GetCurrentStamina() > 15)
                    {
                        fadeTimer(0);
                        _player.DecrementCurrentStamina(15);
                        _dodgeManager.SetActive(true);
                    }
                    else
                    {
                        showAlarm();
                    }
                }
                else
                {
                    fadeTimer(0);
                    tryLimit(4, 4, 2, 2);
                    _player.DecrementCurrentStamina(15);
                    _dodgeManager.SetActive(true);
                }
                break;
            case "FC":
                if (_hasPlayedTutorial)
                {
                    if (_player.GetCurrentStamina() > 15)
                    {
                        fadeTimer(0);
                        _player.DecrementCurrentStamina(15);
                        _focusManager.SetActive(true);
                    }
                    else
                    {
                        showAlarm();
                    }
                }
                else
                {
                    fadeTimer(0);
                    tryLimit(5, 7, 3, 2);
                    _player.DecrementCurrentStamina(15);
                    _focusManager.SetActive(true);
                }
                break;
            case "RST":
                fadeTimer(0);
                if (!_hasPlayedTutorial)
                    tryLimit(6, 5, 4, 2);
                _restManager.SetActive(true);
                break;
        }
    }

    void tryLimit(int interaction, float delay, int outInputIndex, int tries)
    {
        _numberOfTries++;
        if (_numberOfTries == tries)
        {
            toggleInput(outInputIndex, 0);
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
            if (_hasPlayedTutorial)
            {
                showAllInput(0);
            }
            _playerActionIcon.DOFade(0, fadeTime);
            timer.DOFade(0, fadeTime);
        } else if (inOrOut == 1)
        {
            if (_hasPlayedTutorial)
            {
                showAllInput(1);
            }
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
    public void activateS()
    {
        if (sKey.gameObject.activeInHierarchy)
        {
            sKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            sKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(() => activateS());
        }
    }

    public void activateX()
    {
        if (xKey.gameObject.activeInHierarchy)
        {
            xKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            xKey.DOScale(1, 0.1f).SetDelay(0.6f).OnComplete(() => activateX());
        }
    }
    public void activateLeftRight()
    {
        if (leftKey.gameObject.activeInHierarchy && rightKey.gameObject.activeInHierarchy)
        {
            animateLeft();
            animateRight();
            void animateLeft()
            {
                leftKey.DOScale(0.7f, 0.1f).OnComplete(animateRight);
                rightKey.DOScale(0.8f, 0.1f);
            }
            void animateRight()
            {
                rightKey.DOScale(0.7f, 0.1f).OnComplete(animateLeft);
                leftKey.DOScale(0.8f, 0.1f);
            }
        }
    }
    // Shield
    void updateShield()
    {
        shieldBar.DOValue(_player.getCurrentShield(), .3f);
        shieldNumber.text = _player.getCurrentShield().ToString();
    }

    public void repeatTutorial()
    {
        _hasPlayedTutorial = true;
        _dialogueManager.showEndScreen(false);
        _dialogueManager.Overlay(0);
        _dialogueManager.toogleEndTutorial(1);
        showAllInput(1);
    }

    public void showAllInput(int inOrOut)
    {
        // 1 = in
        if (inOrOut == 1)
        {
            _dialogueManager.toogleEndTutorial(1);
            for (int i = 0; i < _inputs.Length; i++)
            {
                if (i <= 5)
                {
                    toggleInput(i, 1);
                }
                else
                {
                    _inputs[6].DOLocalMoveX(200, 0.5f);
                    _inputs[7].DOLocalMoveX(200, 0.5f);
                    _inputs[8].DOLocalMoveX(200, 0.5f);
                }
            }
        } else
        {
            _dialogueManager.toogleEndTutorial(0);
            for (int i = 0; i < _inputs.Length; i++)
            {
                toggleInput(i, 0);
            }
        }
    }

    // Stamina Alarm
    private bool hasPlayed = false;
    public void alarmStamina()
    {
        if (!hasPlayed)
        {
            fadeON();
            _playerStamina.transform.DOShakePosition(0.6f, 4, 50);
            _staminaAlarm.transform.DOShakePosition(0.6f, 4, 50).OnComplete(() => fadeOFF());
            hasPlayed = true;
        }

        void fadeON()
        {
            foreach (Transform child in _staminaAlarm.transform)
            {
                child.GetComponent<Image>().DOFade(1, 0.2f);
            }
        }
        void fadeOFF()
        {
            foreach (Transform child in _staminaAlarm.transform)
            {
                child.GetComponent<Image>().DOFade(0, 0.2f);
            }
            hasPlayed = false;
        }
    }

    // Detail attack

    public bool hasShownDetail_attack = false;
    public void attackDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(0).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(0).transform;
        SpriteRenderer overlay = _overlay.GetComponent<SpriteRenderer>();
        float cursorDelay = .6f;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_attack)
                {
                    overlay.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-336, 13.3f), 0.8f).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[0].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.01f;
                }
                break;

            case 2: // On comand click
                if (!hasShownDetail_attack)
                {
                    Time.timeScale = 0.8f;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    overlay.DOFade(0, 0.4f);
                }
                break;

            case 3: // On attack animation
                Time.timeScale = 0.3f;
                break;
        }
    }

    public bool hasShownDetail_defend = false;
    public void defendDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(1).GetComponent<Image>();
        SpriteRenderer overlay = _overlay.GetComponent<SpriteRenderer>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(1).transform;
        float cursorDelay = .8f;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_defend)
                {
                    overlay.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-336, -30), 0.8f).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[1].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.01f;
                }
                break;

            case 2: // Player clicks
                if (!hasShownDetail_defend)
                {
                    Time.timeScale = 0.8f;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    overlay.DOFade(0, 0.4f);
                }
                break;
        }
    }
}
