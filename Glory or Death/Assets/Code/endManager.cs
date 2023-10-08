using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssetKits.ParticleImage;
using TMPro;

public class endManager : MonoBehaviour
{
    Combat_UI combat_UI;
    SoundPlayer soundPlayer;
    AudioManager audioManager;
    Player _player;

    [SerializeField] Image endOverlay;
    [SerializeField] Image dialogueBubble;
    [SerializeField] ParticleImage starParticle;

    [SerializeField] GameObject playerAvatar;
    [SerializeField] GameObject endConfeti;
    [SerializeField] GameObject _upgradeManager;

    [SerializeField] Transform endStarSymbol;
    [SerializeField] Transform summeryWindow;
    [SerializeField] Transform quitButton;
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

    // Victory
    [Header("Victory")]
    [SerializeField] Transform victoryLabelContainer;
    [SerializeField] GameObject victoryScreenContainer;
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        combat_UI = FindObjectOfType<Combat_UI>();
        soundPlayer = FindObjectOfType<SoundPlayer>();

        _player = FindObjectOfType<Player>();
    }

    // End
    int starCounter = 0;
    public IEnumerator showEndScreen(float delay)
    {
        updateStarUI();
        activateEndElements(true, 0);
        audioManager.Play("End_Horn");
        audioManager.Play("End_Drums");
        endOverlay.DOFade(0.85f, 1f);
        yield return new WaitForSeconds(delay);

        showSummary();
    }
    public void activateEndElements(bool state, int condition)
    {
        switch (condition)
        {
            case 0: // Time Out
                activateTimeOutElements(state);
                break;
            case 1: // Defeat
                activateDefeatElements(state);
                break;
            case 2: // Victory
                activateVictoryElements(state);
                break;
        }
    }
    public void addToStarCounter()
    {
        if (combat_UI.GetStars() != 0)
        {
            starCounter++;
            updateStarUI();
            combat_UI.removeStar();
        }
        else
        {
            StartCoroutine(animatePlayerAvatarIn("Just a warm up...", 0));
            combat_UI.hideStars();
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
        endOverlay.DOFade(0, 0.3f);
        animatePlayerAvatarOut();
        _upgradeManager.SetActive(false);
        hideUpgradeButton();
    }

    public void defeatScreen()
    {
        endOverlay.DOFade(0.85f, 1f);
        enemyContainer.DOLocalMoveX(0, 0.3f).SetDelay(2.5f);
        defeatLabelContainer.DOLocalMoveY(0, 1).SetDelay(1);
        quitButton.DOLocalMoveY(-160, 1).SetDelay(3.5f);
        foreach (var effect in defeatEffects)
        {
            effect.gameObject.SetActive(true);
        }
        defeatEffects[2].transform.DOLocalMoveY(-320, 1);
        defeatEffects[1].transform.DOLocalMoveY(-1200, 1);
    }

    public void victoryScreen()
    {
        endOverlay.DOFade(0.85f, 1f);
        victoryLabelContainer.DOLocalMoveY(0, 1).SetDelay(1);
        quitButton.DOLocalMoveY(-160, 1).SetDelay(3.5f);
        foreach (var effect in victoryEffects)
        {
            effect.gameObject.SetActive(true);
        }
        victoryEffects[0].transform.DOLocalMoveY(-60, 1);
        victoryEffects[1].transform.DOLocalMoveY(-70, 1);
    }

    // Avatar Anim
    public IEnumerator animatePlayerAvatarIn(string BubbleText, float delay)
    {
        yield return new WaitForSeconds(delay);
        playerAvatar.transform.DOLocalMoveX(-318, .2f);
        dialogueBubble.transform.DOLocalMoveX(-190, .2f);
        dialogueBubble.DOFade(1, 0.3f);
        dialogueText.DOFade(1, 0.3f);
        dialogueText.text = BubbleText;
        resetButton.gameObject.SetActive(true);
        showUpgradeButton(1);
    }
    public void animatePlayerAvatarOut()
    {
        playerAvatar.transform.DOLocalMoveX(-540, 0.2f);
        dialogueBubble.transform.DOLocalMoveX(-300, 0.2f);
        dialogueBubble.DOFade(0, 0.1f);
        dialogueText.DOFade(0, 0.1f);
    }

    // Summary window
    void showSummary()
    {
        float threshholdValue = 3;
        summeryWindow.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutBounce).OnUpdate(() =>
        {
            float curveY = summeryWindow.transform.localPosition.y;
            bool hasHitPlayed = false;
            if (curveY < threshholdValue && !hasHitPlayed)
            {
                soundPlayer.metalStone();
                hasHitPlayed = true;
                threshholdValue -= 0.6f;
            }
        }).OnComplete(playStarAnimation);
    }

    void playStarAnimation()
    {
        combat_UI.showStars();
        if (combat_UI.GetStars() > 0)
        {
            starParticle.Play();
        }
    }

    public void showUpgradeButton(int delay = 0)
    {
        upgradeButton.DOLocalMoveX(300, .4f).SetDelay(delay);
    }
    public void hideUpgradeButton()
    {
        audioManager.Play("DG_jump_1");
        upgradeButton.DOLocalMoveX(530, .3f);
    }

    public void showUpgradeScreen()
    {
        // onUpgradeButton
        audioManager.Play("UI_select");
        _upgradeManager.SetActive(true);
        animatePlayerAvatarOut();
        _upgradeManager.transform.DOLocalMoveX(200, 0.3f);
        summeryWindow.transform.DOLocalMoveX(-200, 0.3f);
        hideUpgradeButton();
    }
    public void hideUpgradeScreen(bool isReset)
    {
        _upgradeManager.transform.DOLocalMoveX(650, 0.3f);
        summeryWindow.transform.DOLocalMoveX(0, 0.3f);
        if (!isReset)
        {
            audioManager.Play("UI_select");
            showUpgradeButton();
        }
    }

    void activateTimeOutElements(bool state)
    {
        summeryWindow.gameObject.SetActive(state);
        playerAvatar.gameObject.SetActive(state);
        dialogueBubble.gameObject.SetActive(state);
        endOverlay.gameObject.SetActive(state);
        endConfeti.gameObject.SetActive(state);
        _upgradeManager.SetActive(state);
    }
    void activateDefeatElements(bool state)
    {
        endOverlay.gameObject.SetActive(state);
        defeatLabelContainer.gameObject.SetActive(state);
        defeatScreenContainer.SetActive(state);
    }
    void activateVictoryElements(bool state)
    {
        victoryScreenContainer.SetActive(state);
        endOverlay.gameObject.SetActive(state);
        victoryLabelContainer.gameObject.SetActive(state);
        playerAvatar.gameObject.SetActive(state);
        dialogueBubble.gameObject.SetActive(state);
        endConfeti.gameObject.SetActive(state);
    }

    public int GetStars()
    {
        return starCounter;
    }
    public void reduceStars(int starsToReduce)
    {
        starCounter -= starsToReduce;
    }
}
