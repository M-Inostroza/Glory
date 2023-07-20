using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssetKits.ParticleImage;
using TMPro;

public class endManager : MonoBehaviour
{
    AudioManager audioManager;
    Combat_UI combat_UI;
    SoundPlayer soundPlayer;

    [SerializeField] Image endOverlay;
    [SerializeField] Image dialogueBubble;
    [SerializeField] ParticleImage starParticle;

    [SerializeField] GameObject playerAvatar;
    [SerializeField] GameObject endConfeti;

    [SerializeField] Transform endStarSymbol;
    [SerializeField] Transform summeryWindow;

    [SerializeField] TMP_Text endStarCount;
    [SerializeField] TMP_Text dialogueText;
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
        activateEndElements();
        audioManager.Play("End_Horn");
        audioManager.Play("End_Drums");
        endOverlay.DOFade(0.85f, 1f);
        yield return new WaitForSeconds(delay);
        combat_UI.showStars();
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
    void activateEndElements()
    {
        summeryWindow.gameObject.SetActive(true);
        playerAvatar.gameObject.SetActive(true);
        dialogueBubble.gameObject.SetActive(true);
        endOverlay.gameObject.SetActive(true);
        endConfeti.gameObject.SetActive(true);
    }
    public void addToStarCounter()
    {
        Debug.Log("Stars from cui: " + combat_UI.GetStars());
        if (combat_UI.GetStars() != 0)
        {
            starCounter++;
            endStarCount.text = starCounter.ToString();
            combat_UI.removeStar();
        }
        else
        {
            playerAvatar.transform.DOLocalMoveX(-318, 1);
            dialogueBubble.transform.DOLocalMoveX(-190, 1);
            dialogueBubble.DOFade(1, 0.8f);
            dialogueText.DOFade(1, 0.8f);
            combat_UI.hideStars();
        }
    }
    public void starPunchEnd()
    {
        audioManager.Play("Star_Shimes_2");
        endStarSymbol.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f).OnComplete(() => endStarSymbol.DOScale(1, 0));
    }
}
