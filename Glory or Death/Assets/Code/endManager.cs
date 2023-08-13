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

    [SerializeField] Image endOverlay;
    [SerializeField] Image dialogueBubble;
    [SerializeField] ParticleImage starParticle;

    [SerializeField] GameObject playerAvatar;
    [SerializeField] GameObject endConfeti;

    [SerializeField] Transform endStarSymbol;
    [SerializeField] Transform summeryWindow;
    [SerializeField] Transform quitButton;

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
    }

    // End
    int starCounter = 0;
    public IEnumerator showEndScreen(float delay)
    {
        activateEndElements(true, 0);
        audioManager.Play("End_Horn");
        audioManager.Play("End_Drums");
        endOverlay.DOFade(0.85f, 1f);
        yield return new WaitForSeconds(delay);

        showSummary(); 
        combat_UI.showStars();
    }
    public void activateEndElements(bool state, int condition)
    {
        switch (condition)
        {
            case 0: // Time Out
                summeryWindow.gameObject.SetActive(state);
                playerAvatar.gameObject.SetActive(state);
                dialogueBubble.gameObject.SetActive(state);
                endOverlay.gameObject.SetActive(state);
                endConfeti.gameObject.SetActive(state);
                break;
            case 1: // Defeat
                endOverlay.gameObject.SetActive(state);
                defeatLabelContainer.gameObject.SetActive(state);
                defeatScreenContainer.SetActive(state);
                break;
            case 2: // Victory
                victoryScreenContainer.SetActive(state);
                endOverlay.gameObject.SetActive(state);
                victoryLabelContainer.gameObject.SetActive(state);
                playerAvatar.gameObject.SetActive(state);
                dialogueBubble.gameObject.SetActive(state);
                endConfeti.gameObject.SetActive(state);
                break;
        }
    }
    public void addToStarCounter()
    {
        if (combat_UI.GetStars() != 0)
        {
            starCounter++;
            endStarCount.text = starCounter.ToString();
            combat_UI.removeStar();
        }
        else
        {
            StartCoroutine(animatePlayerAvatarIn("Just a warm up...", 1));
            combat_UI.hideStars();
        }
    }
    public void starPunchEnd()
    {
        audioManager.Play("Star_Shimes_2");
        endStarSymbol.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.1f).OnComplete(() => endStarSymbol.DOScale(1, 0));
    }

    public void resetFight()
    {
        summeryWindow.transform.DOLocalMoveY(455, 0.4f).OnComplete(()=> activateEndElements(false, 0));
        starCounter = 0;
        endOverlay.DOFade(0, 0.3f);
        animatePlayerAvatarOut();
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
        playerAvatar.transform.DOLocalMoveX(-318, 1);
        dialogueBubble.transform.DOLocalMoveX(-190, 1);
        dialogueBubble.DOFade(1, 0.8f);
        dialogueText.DOFade(1, 0.8f);
        dialogueText.text = BubbleText;
    }
    public void animatePlayerAvatarOut()
    {
        playerAvatar.transform.DOLocalMoveX(-540, 0.3f);
        dialogueBubble.transform.DOLocalMoveX(-300, 0.3f);
        dialogueBubble.DOFade(0, 0.2f);
        dialogueText.DOFade(0, 0.2f);
    }

    // Summary window
    void showSummary()
    {
        float threshholdValue = 2.7f;
        summeryWindow.transform.DOLocalMoveY(0, 2f).SetEase(Ease.OutBounce).OnUpdate(() =>
        {
            float curveY = summeryWindow.transform.localPosition.y;
            bool hasHitPlayed = false;
            if (curveY < threshholdValue && !hasHitPlayed)
            {
                soundPlayer.metalStone();
                hasHitPlayed = true;
                threshholdValue -= 0.5f;
            }
        }).OnComplete(() => starParticle.Play());
    }
}
