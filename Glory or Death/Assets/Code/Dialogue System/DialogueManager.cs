using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TMP_Text _playerText;
    [SerializeField] TMP_Text _guardText;

    [SerializeField] GameObject _playerFrame;
    [SerializeField] GameObject _guardFrame;

    [SerializeField] Image _overlay;

    Tutorial_UI tutorial_UI;

    void Start()
    {
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
        //tutorial_UI.toggleInput(7, 1);
        StartCoroutine(interactions(1, 3));
    }

    public IEnumerator interactions(int index, float delay)
    {
        float changeSpeakerTime = 0.4f;
        IEnumerator change()
        {
            yield return new WaitForSeconds(changeSpeakerTime);
        }
        int In = 0;
        int Out = 1;
        yield return new WaitForSeconds(delay);
        Overlay(1);
        switch (index)
        {
            case 1: // Attack Tutorial
                moveGuardContainer(In);
                _guardText.text = "I hope you know how to use a sword...";
                _playerText.text = "...";
                yield return new WaitForSeconds(4);
                moveGuardContainer(Out);
                change();

                movePlayerContainer(In);
                yield return new WaitForSeconds(2);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "Click the attack command, wait for the timer and click as many targets as you can";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(0, 1);
                break;
            case 2: // Defense tutorial
                movePlayerContainer(In);
                _playerText.text = "That's hard...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "You better get use to it... let's try some defense";
                yield return new WaitForSeconds(3);
                _guardText.text = "Click the defend command and press the A key in the green area...";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(1, 1);
                break;
            case 3:
                movePlayerContainer(In); _playerText.text = "That was better";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Don't get too confident, let's see your feet...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Click the dodge command and press the arrow keys in the right order, this will avoid the next enemy attack...";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(2, 1);
                break;
            case 4:
                moveGuardContainer(In); _guardText.text = "Decent enough, use them later in the arena";
                yield return new WaitForSeconds(4);
                moveGuardContainer(Out);
                change();

                movePlayerContainer(In); _playerText.text = "I know how to move";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "We'll see... focus now, click the focus command and press the S key right in the center";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(3, 1);
                break;
            case 5:
                movePlayerContainer(In); _playerText.text = "aghh...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Look at you, all weak and tired, get some rest!";
                yield return new WaitForSeconds(5);
                _guardText.text = "Click the rest command and smash the arrow keys left and right to recover stamina";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(4, 1);
                break;
            case 6:
                movePlayerContainer(In); _playerText.text = "That feels good, I think I'm ready to fight...";
                yield return new WaitForSeconds(4);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "No no no, you still have much to learn.";
                yield return new WaitForSeconds(4);
                _guardText.text = "You don't even know how to block...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Smash the X key to rotate the shield and block the opponent";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(6, 1);
                break;
            case 7:
                movePlayerContainer(In); _playerText.text = "Easy...";
                yield return new WaitForSeconds(4);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Easy???";
                yield return new WaitForSeconds(5);
                _guardText.text = "Let's see how easy is to block a super attack...";
                yield return new WaitForSeconds(5);
                _guardText.text = "Move the arrows left and right to control the shield and block the sowrds";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(7, 1);
                break;
            case 8:
                movePlayerContainer(In); _playerText.text = "Wow, I'd better be careful...";
                yield return new WaitForSeconds(4);
                movePlayerContainer(Out);
                Overlay(0);
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
