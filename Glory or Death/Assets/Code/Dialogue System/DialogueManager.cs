using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] string[] lines;
    [SerializeField] TMP_Text _playerText;
    [SerializeField] TMP_Text _guardText;

    [SerializeField] GameObject _playerFrame;
    [SerializeField] GameObject _guardFrame;

    [SerializeField] Image _overlay;

    Tutorial_UI tutorial_UI;

    void Start()
    {
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
        tutorial_UI.toggleInput(0, 1);
        //StartCoroutine(interactions(1, 2));
    }

    public IEnumerator interactions(int index, float delay)
    {
        float changeSpeakerTime = 0.4f;
        int In = 0;
        int Out = 1;
        yield return new WaitForSeconds(delay);
        Overlay(1);
        switch (index)
        {
            case 1: // Attack Tutorial
                moveGuardContainer(In);
                _guardText.text = lines[0];
                _playerText.text = lines[1];
                yield return new WaitForSeconds(4);
                moveGuardContainer(Out);
                yield return new WaitForSeconds(changeSpeakerTime);
                movePlayerContainer(In);
                yield return new WaitForSeconds(2);
                movePlayerContainer(Out);
                yield return new WaitForSeconds(changeSpeakerTime);
                moveGuardContainer(In);
                _guardText.text = lines[2];
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(0, 1);
                break;
            case 2:
                Overlay(1);
                movePlayerContainer(0);
                _playerText.text = lines[3];
                break;
        }
    }
    void Overlay(int showOrHide)
    {
        if (showOrHide == 0)
        {
            _overlay.DOFade(0, 0.4f).OnComplete(()=> _overlay.gameObject.SetActive(false));
        } else if (showOrHide == 1)
        {
            _overlay.gameObject.SetActive(true);
            _overlay.DOFade(0.6f, 0.4f);
        }
    }
    void moveGuardContainer(int inOrOut)
    {   // 0 = In - 1 = Out
        if (inOrOut == 0)
        {
            _guardFrame.SetActive(true);
            _guardFrame.transform.DOLocalMoveX(70, 0.4f);
        } else
        {
            _guardFrame.transform.DOLocalMoveX(600, 0.4f).OnComplete(()=> _guardFrame.SetActive(false));
        }
    }
    void movePlayerContainer(int inOrOut)
    {
        // 0 = In - 1 = Out
        if (inOrOut == 0)
        {
            _playerFrame.SetActive(true);
            _playerFrame.transform.DOLocalMoveX(-70, 0.4f);
        }
        else
        {
            _playerFrame.transform.DOLocalMoveX(-600, 0.4f).OnComplete(() => _playerFrame.SetActive(false));
        }
    }
}
