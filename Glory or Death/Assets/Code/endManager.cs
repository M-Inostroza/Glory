using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssetKits.ParticleImage;
using TMPro;

public class EndManager : MonoBehaviour
{
    CombatManager CombatManager;
    SoundPlayer SoundPlayer;
    AudioManager AudioManager;
    TimeManager TimeManager;
    LoadingScreen LoadingScreen;
    Enemy Enemy;
 
    [SerializeField] Image endOverlay;
    [SerializeField] Image dialogueBubble;
    [SerializeField] ParticleImage starParticle;

    [SerializeField] GameObject playerAvatar;
    [SerializeField] GameObject _endConfeti;
    [SerializeField] GameObject _upgradeManager;

    [SerializeField] Transform endStarSymbol;
    [SerializeField] Transform summeryWindow;
    [SerializeField] Transform _TryAgainButton;
    [SerializeField] Transform resetButton;
    [SerializeField] Transform upgradeButton;

    [SerializeField] TMP_Text endStarCount;
    [SerializeField] TMP_Text dialogueText;

    [SerializeField] ParticleSystem[] defeatEffects;
    [SerializeField] ParticleSystem[] victoryEffects;

    // Defeat
    [Header("Defeat")]
    [SerializeField] Transform enemyContainer;
    [SerializeField] Transform defeatLabelContainer;
    [SerializeField] GameObject defeatScreenContainer;

    // Victory elements
    [Header("Victory")]
    [SerializeField] Transform victoryLabelContainer;
    [SerializeField] GameObject victoryScreenContainer;

    [SerializeField] Transform _finalDaysUI;

    private void Awake()
    {
        AudioManager = FindObjectOfType<AudioManager>();
        CombatManager = FindObjectOfType<CombatManager>();
        SoundPlayer = FindObjectOfType<SoundPlayer>();
        TimeManager = FindObjectOfType<TimeManager>();
        Enemy = FindObjectOfType<Enemy>();
        LoadingScreen = FindObjectOfType<LoadingScreen>();
    }

    // End
    int starCounter = 0;
    public IEnumerator showEndScreen(float delay)
    {
        updateStarUI();
        activateEndElements(true, 0);
        AudioManager.Play("End_Horn");
        AudioManager.Play("End_Drums");
        endOverlay.DOFade(0.85f, 1f);
        yield return new WaitForSeconds(delay);

        ShowSummaryWindow();
    }
    public void activateEndElements(bool state, int condition)
    {
        switch (condition)
        {
            case 0: // Time Out
                ActivateTimeOutElements(state);
                break;
            case 1: // Defeat
                ActivateDefeatElements(state);
                break;
            case 2: // Victory case
                ActivateVictoryElements(state);
                break;
        }
    }
    public void addToStarCounter()
    {
        if (CombatManager.GetStars() != 0)
        {
            starCounter++;
            updateStarUI();
            CombatManager.removeStar();
        }
        else
        {
            StartCoroutine(AnimatePlayerAvatarIn("Just a warm up...", 0));
            CombatManager.hideStars();
        }
    }
    public void updateStarUI()
    {
        endStarCount.text = starCounter.ToString();
    }
    public void starPunchEnd()
    {
        endStarSymbol.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.1f).OnComplete(() => endStarSymbol.DOScale(1, 0));
    }

    public void resetFight()
    {
        summeryWindow.transform.DOLocalMoveY(455, 0.4f).OnComplete(()=> activateEndElements(false, 0));
        //endOverlay.DOFade(0, 0.3f);
        animatePlayerAvatarOut();
        _upgradeManager.SetActive(false);
        hideUpgradeButton();
    }

    public void DefeatScreen()
    {
        endOverlay.DOFade(0.85f, 1f);
        enemyContainer.DOLocalMoveX(0, 0.3f).SetDelay(2.5f);
        defeatLabelContainer.DOLocalMoveY(0, 1).SetDelay(1);
        _TryAgainButton.DOLocalMoveY(-160, 1).SetDelay(3.5f);
        foreach (var effect in defeatEffects)
        {
            effect.gameObject.SetActive(true);
        }
        defeatEffects[2].transform.DOLocalMoveY(-320, 1);
        defeatEffects[1].transform.DOLocalMoveY(-1200, 1);
    }


    // ---------- Victory handler ---------- //
    public void VictoryScreen() 
    {
        ShowFinalDays(true);
        MoveVictoryUI(true);
        PlayVictoryVisuals(true);
    }
    void PlayVictoryVisuals(bool onOff)
    {
        if (onOff)
        {
            foreach (var effect in victoryEffects)
            {
                effect.gameObject.SetActive(true);
            }
            victoryEffects[0].transform.DOLocalMoveY(-60, 1);
            victoryEffects[1].transform.DOLocalMoveY(-70, 1);
        } else
        {
            foreach (var effect in victoryEffects)
            {
                effect.gameObject.SetActive(false);
            }
            victoryEffects[0].transform.DOLocalMoveY(6, 0.5f);
            victoryEffects[1].transform.DOLocalMoveY(800, 1);
        }
    }
    void MoveVictoryUI(bool inOut)
    {
        if (inOut)
        {
            _TryAgainButton.gameObject.SetActive(true);
            endOverlay.DOFade(0.85f, 1f);
            victoryLabelContainer.DOLocalMoveY(0, 1).SetDelay(1);
            _TryAgainButton.DOLocalMoveY(-160, 1).SetDelay(3.5f);
        } else
        {
            endOverlay.DOFade(0, 0.5f);
            victoryLabelContainer.DOLocalMoveY(300, 0.5f);
            _TryAgainButton.DOLocalMoveY(-325, 0.5f).OnComplete(()=> _TryAgainButton.gameObject.SetActive(false));
        }
    }
    public void ShowFinalDays(bool inOut)
    {
        if (inOut)
        {
            if (GameManager.UpdateNewRecord())
            {
                // Create the new record effect
                _finalDaysUI.GetChild(5).gameObject.SetActive(true);
            }
            _finalDaysUI.GetChild(4).GetComponent<TMP_Text>().text = GameManager.GetDayCounter().ToString() + " Days!";
            _finalDaysUI.DOLocalMoveY(67, 0.8f).SetDelay(2);
        } else
        {
            _finalDaysUI.DOLocalMoveY(-440, 0.8f).OnComplete(()=> _finalDaysUI.GetChild(5).gameObject.SetActive(false));
        }
        
    }
    // ---------- Victory handler ---------- //


    // ---------- Avatar Show ---------- //
    public IEnumerator AnimatePlayerAvatarIn(string BubbleText, float delay, bool isEnd = false)
    {
        yield return new WaitForSeconds(delay);
        playerAvatar.transform.DOLocalMoveX(-318, .2f);
        dialogueBubble.transform.DOLocalMoveX(-190, .2f);
        dialogueBubble.DOFade(1, 0.3f);
        dialogueText.DOFade(1, 0.3f);
        dialogueText.text = BubbleText;
        resetButton.gameObject.SetActive(true);
    }
    public void animatePlayerAvatarOut()
    {
        playerAvatar.transform.DOLocalMoveX(-540, 0.2f);
        dialogueBubble.transform.DOLocalMoveX(-300, 0.2f);
        dialogueBubble.DOFade(0, 0.1f);
        dialogueText.DOFade(0, 0.1f);
    }
    // ---------- Avatar Show ---------- //


    // ---------- Summary window ---------- //
    void ShowSummaryWindow()
    {
        summeryWindow.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutBounce).OnUpdate(() =>
        {
            float threshholdValue = 3;
            float curveY = summeryWindow.transform.localPosition.y;
            bool hasHitPlayed = false;
            if (curveY < threshholdValue && !hasHitPlayed)
            {
                SoundPlayer.metalStone();
                hasHitPlayed = true;
                threshholdValue -= 0.6f;
            }
        }).OnComplete(PlayStarAnimation);
    }
    // ---------- Summary window ---------- //


    // ---------- Upgrade ---------- //
    public void showUpgradeButton(int delay = 0)
    {
        upgradeButton.DOLocalMoveX(300, .4f).SetDelay(delay);
    }
    public void hideUpgradeButton()
    {
        AudioManager.Play("DG_jump_1");
        upgradeButton.DOLocalMoveX(530, .3f);
    }
    public void showUpgradeScreen()
    {
        // onUpgradeButton
        AudioManager.Play("UI_select");
        _upgradeManager.SetActive(true);
        animatePlayerAvatarOut();
        _upgradeManager.transform.DOLocalMoveX(200, 0.3f);
        summeryWindow.transform.DOLocalMoveX(-200, 0.3f);
        hideUpgradeButton();
    }
    public void HideUpgradeScreen(bool isReset)
    {
        _upgradeManager.transform.DOLocalMoveX(650, 0.3f);
        summeryWindow.transform.DOLocalMoveX(0, 0.3f);
        if (!isReset)
        {
            AudioManager.Play("UI_select");
            showUpgradeButton();
        }
    }
    // ---------- Upgrade ---------- //


    // ---------- End elements hadler ---------- //
    void ActivateTimeOutElements(bool state)
    {
        summeryWindow.gameObject.SetActive(state);
        playerAvatar.gameObject.SetActive(state);
        dialogueBubble.gameObject.SetActive(state);
        endOverlay.gameObject.SetActive(state);
        _endConfeti.gameObject.SetActive(state);
        _upgradeManager.SetActive(state);
    }
    void ActivateDefeatElements(bool state)
    {
        endOverlay.gameObject.SetActive(state);
        defeatLabelContainer.gameObject.SetActive(state);
        defeatScreenContainer.SetActive(state);
    }
    void ActivateVictoryElements(bool state)
    {
        victoryScreenContainer.SetActive(state);
        endOverlay.gameObject.SetActive(state);
        victoryLabelContainer.gameObject.SetActive(state);
        playerAvatar.gameObject.SetActive(state);
        dialogueBubble.gameObject.SetActive(state);
        _endConfeti.gameObject.SetActive(state);
    }
    // ---------- End elements hadler ---------- //


    // ---------- Stars ---------- //
    public int GetStars()
    {
        return starCounter;
    }
    public void ReduceStars(int starsToReduce)
    {
        starCounter -= starsToReduce;
    }
    void PlayStarAnimation()
    {
        CombatManager.showStars();
        if (CombatManager.GetStars() > 0)
        {
            starParticle.Play();
        }
        else
        {
            showUpgradeButton();
        }
    }
    // ---------- Stars ---------- //


    // RESET - in case of Victory, starts a new round --you keep upgrades--
    public void TryAgain()
    {
        _endConfeti.SetActive(false);
        TimeManager.ResetTimers(false);
        MoveVictoryUI(false);
        ShowFinalDays(false);
        PlayVictoryVisuals(false);
        Enemy.ResetStats();

        // Reset Anims
        Player.BackToIdle();
        Enemy.BackToIdle();
        // All cooldown refres
        // All stats go normal
    }
}
