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
    [SerializeField] Transform _fightButton;
    [SerializeField] Transform _resetButton;
    [SerializeField] Transform _readyText;

    Tutorial_UI tutorial_UI;

    void Start()
    {
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
        //tutorial_UI.toggleInput(8, 1);
        StartCoroutine(interactions(1, 1));
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
        Overlay(1, .6f);
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
                _guardText.text = "Select the attack command, wait for the timer and click as many targets as you can";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(0, 1);
                break;
            case 2: // Defense tutorial
                movePlayerContainer(In);
                _playerText.text = "I'ts harder than I thought...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "You better get use to it... let's try some defense";
                yield return new WaitForSeconds(3);
                _guardText.text = "Click the defend command and press the A key in the green area...";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(1, 1);
                break;
            case 3:
                movePlayerContainer(In); _playerText.text = "not bad...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Don't get too confident, let's test your feet...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Select the dodge command and press the arrow keys in the right order, this will avoid the next enemy attack...";
                yield return new WaitForSeconds(6);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(2, 1);
                break;
            case 4:
                moveGuardContainer(In); _guardText.text = "Decent enough, don't forget to use them later in the arena";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                change();

                movePlayerContainer(In); _playerText.text = "I think i get it";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "We will see abou that...";
                yield return new WaitForSeconds(3);
                _guardText.text = "You need to focus now, focus will give you a speed boost in your timer and you will deal more damage";
                yield return new WaitForSeconds(6);
                _guardText.text = "Select the focus command and press the S key the the target is in the moving box";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(3, 1);
                break;
            case 5:
                movePlayerContainer(In); _playerText.text = "I can't keep up...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Look at you, all weak and tired, get some rest!";
                yield return new WaitForSeconds(5);
                _guardText.text = "Select the rest command and smash the arrow keys left and right to recover stamina";
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
                yield return new WaitForSeconds(2);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Easy???";
                yield return new WaitForSeconds(2);
                _guardText.text = "Let's see how easy is to block a super attack...";
                yield return new WaitForSeconds(5);
                _guardText.text = "Move the arrows left and right to control the shield and block the swords";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(7, 1);
                break;
            case 8:
                movePlayerContainer(In); _playerText.text = "Wow, how can I even survive that??";
                yield return new WaitForSeconds(4);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "Complain less and train more, that's it";
                yield return new WaitForSeconds(4);
                _guardText.text = "Let's try your supper attack now";
                yield return new WaitForSeconds(3);
                _guardText.text = "Select the super attack command and click as many targets as you can";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.fadeTimer(1);
                tutorial_UI.toggleInput(5, 1);
                break;
            case 9:
                moveGuardContainer(In); _guardText.text = "Good... but fights are not always fair";
                yield return new WaitForSeconds(4);
                _guardText.text = "Click and drag from side to side to clean the dirt";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.fadeTimer(0);
                tutorial_UI.toggleInput(8, 1);
                break;
            case 10:
                moveGuardContainer(In); _guardText.text = "Enough, it's all you need to know...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Now go to your cell and get some sleep, your first fight will be soon";
                yield return new WaitForSeconds(4);
                moveGuardContainer(Out);
                showEndScreen(true);
                Overlay(1, .8f);
                break;
        }
    }
    public void Overlay(int showOrHide, float intensity = 0)
    {
        if (showOrHide == 0)
        {
            _overlay.DOFade(0, 0.4f).OnComplete(()=> _overlay.gameObject.SetActive(false));
        } else if (showOrHide == 1)
        {
            _overlay.gameObject.SetActive(true);
            _overlay.DOFade(intensity, 0.4f);
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

    public void showEndScreen(bool InOut)
    {
        if (InOut == true)
        {
            _readyText.gameObject.SetActive(true);
            _readyText.DOLocalMoveY(100, 1);
            _fightButton.gameObject.SetActive(true);
            _fightButton.DOLocalMoveY(-110, 1);
            _resetButton.gameObject.SetActive(true);
            _resetButton.DOLocalMoveY(-110, 1);
        } else
        {
            _readyText.DOLocalMoveY(250, 1).OnComplete(()=> _readyText.gameObject.SetActive(false));
            _fightButton.DOLocalMoveY(-300, 1).OnComplete(() => _fightButton.gameObject.SetActive(false));
            _resetButton.DOLocalMoveY(-300, 1).OnComplete(() => _resetButton.gameObject.SetActive(false));
        }
    }
}
