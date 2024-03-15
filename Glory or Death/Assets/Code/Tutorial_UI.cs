using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    // Fight presentation and scene change
    [SerializeField] Transform panelContainer;
    [SerializeField] Transform playerPanel;
    [SerializeField] Transform enemyPanel;

    // Shield
    [SerializeField] Slider shieldBar;
    [SerializeField] TMP_Text shieldNumber;

    static GameObject _overlay;
    private SpriteRenderer _overlaySprite;
    static GameObject _cursorContainer;

    private string _slectedAction;
    private int _numberOfTries;

    cameraManager _cameraManager;
    Player _player;
    TargetManager _targetManager;
    defendManager _defendManager;
    AudioManager _audioManager;
    DialogueManager _dialogueManager;
    GoalManager _goalManager;

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
        _cameraManager = FindObjectOfType<cameraManager>();
        _goalManager = FindObjectOfType<GoalManager>();

        _overlay = transform.GetChild(3).gameObject;
        _overlaySprite = _overlay.GetComponent<SpriteRenderer>();
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
    public void showUI()
    {
        float move_in_speed = 0.3f;

        _playerStamina.DOLocalMoveX(_playerStamina.localPosition.x + 200, move_in_speed).SetEase(Ease.InOutSine);
        _playerStats.DOLocalMoveX(_playerStats.localPosition.x + 350, move_in_speed).SetEase(Ease.InOutSine);
        _playerTimer.DOLocalMoveY(_playerTimer.localPosition.y - 160, move_in_speed);
    }
    public void hideUI()
    {
        float move_in_speed = 0.3f;

        _playerStamina.DOLocalMoveX(_playerStamina.localPosition.x - 200, move_in_speed).SetEase(Ease.InOutSine);
        _playerStats.DOLocalMoveX(_playerStats.localPosition.x - 350, move_in_speed).SetEase(Ease.InOutSine);
        _playerTimer.DOLocalMoveY(_playerTimer.localPosition.y + 160, move_in_speed);
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

    public IEnumerator toggleInput(int index, int inOut, float delay = 0)
    {   // 1 = in
        yield return new WaitForSeconds(delay);
        try
        {
            if (inOut == 1)
            {
                _inputs[index].DOLocalMoveX(140, 0.7f);
            }
            else
            {
                _inputs[index].DOLocalMoveX(0, 0.5f);
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Index out of array");
            throw;
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

    // Enemy commands
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
            {
                _goalManager.UpdateGoalIndex();
                tryLimit(7, 4, 6, 3);
            }
                
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
                _goalManager.UpdateGoalIndex();
                tryLimit(8, 8, 7, 3);
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
            {
                _goalManager.UpdateGoalIndex();
                tryLimit(10, 6, 8, 3);
            }
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
                    _goalManager.UpdateGoalIndex();
                    fadeTimer(0);
                    _player.DecrementCurrentStamina(15);
                    attack();
                    tryLimit(2, 5, 0, 3);
                }
                void attack() {
                    StartCoroutine(timeManager.slowMotion(1.8f, 0.5f));
                    _player.GetComponent<Animator>().Play("ATK_jump");
                    _targetManager.attack();
                }
                break;
            case "ATK2":
                hideUI();
                _goalManager.MoveGoal(0);
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
                    _goalManager.UpdateGoalIndex();
                    fadeTimer(0);
                    tryLimit(9, 4, 5, 3);
                    _player.DecrementCurrentStamina(30);
                    _superAttackManager.SetActive(true);
                    superAttackDetailTutorial(3);
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
                    _goalManager.UpdateGoalIndex();
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
                    _goalManager.UpdateGoalIndex();
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
                    _goalManager.UpdateGoalIndex();
                    fadeTimer(0);
                    tryLimit(5, 7, 3, 2);
                    _player.DecrementCurrentStamina(15);
                    _focusManager.SetActive(true);
                }
                break;
            case "RST":
                fadeTimer(0);
                if (!_hasPlayedTutorial)
                {
                    _goalManager.UpdateGoalIndex();
                    tryLimit(6, 5, 4, 2);
                }
                _restManager.SetActive(true);
                break;
        }
    }

    void tryLimit(int interaction, float delay, int leavingInput, int tries)
    {
        _numberOfTries++;
        if (_numberOfTries == tries)
        {
            StartCoroutine(toggleInput(leavingInput, 0)); 
            StartCoroutine(_dialogueManager.Interactions(interaction, delay, 1));
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
            aKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(activateA);
        }
    }
    public void activateS()
    {
        if (sKey.gameObject.activeInHierarchy)
        {
            sKey.DOScale(0.8f, 0.1f).SetDelay(0.3f);
            sKey.DOScale(1, 0.1f).SetDelay(1f).OnComplete(activateS);
        }
    }

    public void activateX()
    {
        if (xKey.gameObject.activeInHierarchy)
        {
            xKey.DOScale(0.8f, 0.1f).SetDelay(0.1f);
            xKey.DOScale(1, 0.1f).SetDelay(0.2f).OnComplete(activateX);
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
        setHelpOff();
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
            if (_hasPlayedTutorial)
            {
                foreach (var input in _inputs)
                {
                    input.GetComponent<Button>().interactable = true;
                }
            }
            _dialogueManager.toogleEndTutorial(1);
            for (int i = 0; i < _inputs.Length; i++)
            {
                if (i <= 5)
                {
                    StartCoroutine(toggleInput(i, 1));
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
                StartCoroutine(toggleInput(i, 0));
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

    // Detailed tutorial section
    private float cursorDelay = 0.4f;
    private float cursorSpeed = 0.6f;

    // Detail Attack
    public bool hasShownDetail_attack = false;
    public void attackDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(0).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(0).transform;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_attack)
                {
                    _overlaySprite.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-336, 13.3f), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
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
                    Time.timeScale = 0.9f;
                    cursorImage.DOFade(0, 0.2f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.3f);
                    StartCoroutine(toggleInput(0, 0));
                }
                break;

            case 3: // On attack animation
                Time.timeScale = 0.6f;
                break;
        }
    }

    // Detail Defend
    public bool hasShownDetail_defend = false;
    public void defendDetailTutorial(int step)
    {
        if (gameManager.isTutorial())
        {
            Image cursorImage = _cursorContainer.transform.GetChild(1).GetComponent<Image>();
            Transform cursorTransform = _cursorContainer.transform.GetChild(1).transform;

            switch (step)
            {
                case 1: // Shows cursor moving to command
                    if (!hasShownDetail_defend)
                    {
                        _overlaySprite.DOFade(.8f, 0.4f);
                        cursorTransform.gameObject.SetActive(true);
                        cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                        cursorTransform.DOLocalMove(new Vector2(-336, -30), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
                    }
                    void activateButton()
                    {
                        _inputs[1].GetComponent<Button>().interactable = true;
                        Time.timeScale = 0.05f;
                    }
                break;

            case 2: // Player clicks
                if (!hasShownDetail_defend)
                {
                    Time.timeScale = 0.8f;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.4f);
                    StartCoroutine(toggleInput(1, 0));
                }
                break;

            case 3: // Minigame starts
                if (!hasShownDetail_defend)
                {
                    _overlaySprite.DOFade(0.6f, 0.4f);
                    StartCoroutine(timeManager.slowMotion(1.2f, 0.4f));
                    StartCoroutine(fadeIn());
                }
                IEnumerator fadeIn()
                {
                    yield return new WaitForSeconds(1.2f);
                    _overlaySprite.DOFade(0, 0.3f);
                }
                break;
        }
        }
    }

    // Detail Dodge
    public bool hasShownDetail_dodge = false;
    public void dodgeDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(2).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(2).transform;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_dodge)
                {
                    _overlaySprite.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-337.6f, -70.5f), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[2].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.05f;
                }
                break;

            case 2: // Player clicks
                if (!_hasPlayedTutorial)
                {
                    StartCoroutine(toggleInput(2, 0));
                }
                if (!hasShownDetail_dodge)
                {
                    Time.timeScale = 1;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.4f);
                }
                break;

            case 3: // Minigame starts
                if (!hasShownDetail_dodge)
                {
                    _overlaySprite.DOFade(0.6f, 0.4f);
                    StartCoroutine(fadeIn());
                    hasShownDetail_dodge = true;
                    StartCoroutine(toggleInput(2, 1, 2));
                }
                IEnumerator fadeIn()
                {
                    yield return new WaitForSeconds(1);
                    _overlaySprite.DOFade(0, 0.5f);
                }
                break;
        }
    }

    // Detail Focus
    public bool hasShownDetail_focus = false;
    public void focusDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(3).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(3).transform;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_focus)
                {
                    _overlaySprite.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-339.7f, -87.9f), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[3].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.05f;
                }
                break;

            case 2: // Player clicks
                if (!_hasPlayedTutorial)
                {
                    StartCoroutine(toggleInput(3, 0));
                }
                if (!hasShownDetail_focus)
                {
                    Time.timeScale = 1;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.4f);
                }
                break;

            case 3: // Minigame starts
                if (!hasShownDetail_focus)
                {
                    _overlaySprite.DOFade(0.6f, 0.6f);
                    StartCoroutine(fadeIn());
                    hasShownDetail_focus = true;
                    StartCoroutine(toggleInput(3, 1, 4.2f));
                }
                IEnumerator fadeIn()
                {
                    yield return new WaitForSeconds(2.5f);
                    _overlaySprite.DOFade(0, 0.8f);
                }
                break;
        }
    }

    // Detail Rest
    public bool hasShownDetail_rest = false;
    public void restDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(4).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(4).transform;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_rest)
                {
                    _overlaySprite.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-337.3f, -136.5f), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[4].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.05f;
                }
                break;

            case 2: // Player clicks
                if (!_hasPlayedTutorial)
                {
                    StartCoroutine(toggleInput(4, 0));
                }
                if (!hasShownDetail_rest)
                {
                    Time.timeScale = 1;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.4f);
                }
                break;

            case 3: // Minigame starts
                if (!hasShownDetail_rest)
                {
                    _overlaySprite.DOFade(0.6f, 0.6f);
                    StartCoroutine(fadeIn());
                    StartCoroutine(timeManager.slowMotion(1f, 0.6f));
                    hasShownDetail_rest = true;
                    StartCoroutine(toggleInput(4, 1, 5));
                }
                IEnumerator fadeIn()
                {
                    yield return new WaitForSeconds(4.5f);
                    _overlaySprite.DOFade(0, 0.5f);
                }
                break;
        }
    }

    // Detail Counter
    public bool hasShownDetail_counter = false;
    public void counterDetailTutorial(int step)
    {
        if (gameManager.isTutorial() && _numberOfTries >= 1)
        {
            StartCoroutine(toggleInput(6, 0));

            switch (step)
            {
                case 1: // Stops counter before starts
                    if (!hasShownDetail_counter)
                    {
                        _cameraManager.playChrome();
                        StartCoroutine(timeManager.slowMotion(1.2f, .4f));
                    }
                    break;

                case 2: // Fails counter
                    if (!hasShownDetail_counter)
                    {
                        StartCoroutine(_dialogueManager.specialGuardInteraction(false, 1, 1));
                    }
                    break;

                case 3: // Perfect counter
                    if (!hasShownDetail_counter)
                    {
                        StartCoroutine(_dialogueManager.specialGuardInteraction(true, 1, 1));
                    }
                    break;
            }
        }
    }

    // Detail Super Counter
    public bool hasShownDetail_superCounter = false;
    public void superCounterDetailTutorial(int step)
    {
        if (gameManager.isTutorial() && _numberOfTries >= 1)
        {
            switch (step)
            {
                case 1: // Fails counter
                    if (!hasShownDetail_superCounter)
                    {
                        StartCoroutine(_dialogueManager.specialGuardInteraction(false, 2, 0));
                    }
                    break;

                case 2: // Perfect counter
                    if (!hasShownDetail_superCounter)
                    {
                        StartCoroutine(_dialogueManager.specialGuardInteraction(true, 2, 0));
                    }
                    break;
            }
        }
    }

    // Detail Supper Attack
    public bool hasShownDetail_superAttack = false;
    public void superAttackDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(5).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(5).transform;

        switch (step)
        {
            case 1: // Shows cursor moving to command
                if (!hasShownDetail_superAttack)
                {
                    _overlaySprite.DOFade(.8f, 0.4f);
                    cursorTransform.gameObject.SetActive(true);
                    cursorImage.DOFade(1, 0.4f).SetDelay(cursorDelay);
                    cursorTransform.DOLocalMove(new Vector2(-340, 51), cursorSpeed).SetDelay(cursorDelay).OnComplete(activateButton);
                }
                void activateButton()
                {
                    _inputs[5].GetComponent<Button>().interactable = true;
                    Time.timeScale = 0.01f;
                }
                break;

            case 2: // On comand click
                if (!hasShownDetail_superAttack)
                {
                    Time.timeScale = 1;
                    cursorImage.DOFade(0, 0.4f).OnComplete(() => cursorTransform.gameObject.SetActive(true));
                    _overlaySprite.DOFade(0, 0.4f);
                    StartCoroutine(toggleInput(5, 0));
                }
                break;

            case 3: // On start minigame
                _cameraManager.playChrome();
                StartCoroutine(timeManager.slowMotion(4, .8f));
                break;
        }
    }

    // Detail Dirt
    public bool hasShownDetail_dirt = false;
    public void dirtDetailTutorial(int step)
    {
        Image cursorImage = _cursorContainer.transform.GetChild(6).GetComponent<Image>();
        Transform cursorTransform = _cursorContainer.transform.GetChild(6).transform;

        if (gameManager.isTutorial() && _numberOfTries >= 1)
        {
            switch (step)
            {
                case 1: // Button pressed, animation showing
                    if (!hasShownDetail_dirt)
                    {
                        _cameraManager.playChrome();
                        StartCoroutine(timeManager.slowMotion(.2f, .4f));
                        // Move and appear cursor
                        StartCoroutine(toggleInput(8, 0));
                        cursorTransform.DOLocalMove(new Vector2(-140, -70), 0);
                        cursorTransform.gameObject.SetActive(true);
                        cursorImage.DOFade(1, .5f);
                        cursorTransform.DOLocalMove(new Vector2(170, 30), 0.3f)
                        .SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                    }
                    break;

                case 2: // Stops the hand when the dirt is gone and gives feedback
                    if (!hasShownDetail_dirt)
                    {
                        cursorImage.DOFade(0, .5f).OnComplete(kill);
                        void kill()
                        {
                            cursorTransform.gameObject.SetActive(false);
                            cursorTransform.DOKill();
                        }
                        StartCoroutine(toggleInput(8, 0));
                        StartCoroutine(_dialogueManager.specialGuardInteraction(true, 4, .5f));
                    }
                    break;
            }
        }
    }


    // Sets all detailed tutorials to already shown
    public void setHelpOff()
    {
        hasShownDetail_attack = true;
        hasShownDetail_defend = true;
        hasShownDetail_dodge = true;
        hasShownDetail_focus = true;
        hasShownDetail_rest = true;
        hasShownDetail_counter = true;
        hasShownDetail_superCounter = true;
        hasShownDetail_superAttack = true;
        hasShownDetail_dirt = true;
    }

    // Deal with end tutorial panels
    public void changeSceneToFight()
    {
        panelContainer.gameObject.SetActive(true);
        enemyPanel.DOLocalMoveX(308, .5f);
        playerPanel.DOLocalMoveX(-308, .5f);
        playerPanel.DOLocalMoveX(-308, 1).SetDelay(3).OnComplete(complete);
        void complete()
        {   
            SceneManager.LoadScene("Arena");
        }
    }


    // Getter & Setters
    public int GetNumberOfTries()
    {
        return _numberOfTries;
    }
}
