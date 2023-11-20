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
    [SerializeField] Transform _endTutButton;
    [SerializeField] Transform _readyText;

    [SerializeField] Transform _symbols;

    Tutorial_UI tutorial_UI;

    void Start()
    {
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
        //tutorial_UI.toggleInput(8, 1);
        StartCoroutine(interactions(3, 2));
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
                StartCoroutine(change());

                movePlayerContainer(In);
                yield return new WaitForSeconds(2);
                movePlayerContainer(Out);
                StartCoroutine(change());

                moveGuardContainer(In);
                toogleSymbols(0, true);
                toogleSymbols(1, true);
                _guardText.text = "Select the attack command (   ), wait for the timer and click as many targets (   ) as you can"; 
                yield return new WaitForSeconds(6);
                toogleSymbols(0, false);
                toogleSymbols(1, false);
                _guardText.text = "Try it a few times...";
                yield return new WaitForSeconds(2);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(0, 1);
                yield return new WaitForSeconds(1);
                tutorial_UI.attackDetailTutorial(1);
                break;

            case 2: // Defense tutorial
                movePlayerContainer(In);
                _playerText.text = "Not bad...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                StartCoroutine(change());

                moveGuardContainer(In);
                _guardText.text = "You better get use to it... let's try some defense";
                yield return new WaitForSeconds(3);
                toogleSymbols(2, true);
                toogleSymbols(3, true);
                _guardText.text = "Click the defend command (   ) and press     when the circle is \nin the green area...";
                yield return new WaitForSeconds(6);
                toogleSymbols(2, false);
                toogleSymbols(3, false);
                _guardText.text = "If you succeed you will get a shield point, which will allow you to counter the enemy attack";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(1, 1);
                tutorial_UI.defendDetailTutorial(1);
                break;

            case 3: // Dodge tutorial
                movePlayerContainer(In); _playerText.text = "I think I got this...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "Don't get too confident, let's see how you move...";
                yield return new WaitForSeconds(4);
                toogleSymbols(4, true);
                _guardText.fontSize = 20;
                _guardText.text = "Select the dodge command (    ), wait for the timer and press the arrow keys in the right order, this will avoid the next enemy attack...";
                yield return new WaitForSeconds(6);
                toogleSymbols(4, false);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(2, 1);
                tutorial_UI.dodgeDetailTutorial(1);
                break;

            case 4:
                _guardText.fontSize = 22;
                moveGuardContainer(In); _guardText.text = "Decent enough, don't forget to use them later in the arena";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                change();

                movePlayerContainer(In); _playerText.text = "I will do my best...";
                yield return new WaitForSeconds(3);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In); _guardText.text = "We will see abou that...";
                yield return new WaitForSeconds(3);
                _guardText.text = "You need to focus, this will give you a speed boost in your timer and you will deal more damage";
                yield return new WaitForSeconds(6);
                _guardText.text = "Select the focus command and press the S key when the target is in the moving box";
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

                moveGuardContainer(In); _guardText.text = "Look at you, no energy left, weak and tired, get some rest!";
                yield return new WaitForSeconds(5);
                _guardText.text = "Select the rest command and smash the arrow keys left and right to recover stamina";
                yield return new WaitForSeconds(6);
                _guardText.text = "Stamina allows you to execute commands, if your stamina is empty you won't do a thing!";
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
                _guardText.text = "No no, you still have much to learn, you don't even know how to block...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Smash the X key to rotate the shield and block the sword";
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

                moveGuardContainer(In); _guardText.text = "Easy??";
                yield return new WaitForSeconds(2);
                _guardText.text = "Let's see how easy is to block a super attack...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Move the arrows left and right to control the shield and block the swords";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.toggleInput(7, 1);
                break;
            case 8:
                movePlayerContainer(In); _playerText.text = "How can I even survive that??";
                yield return new WaitForSeconds(4);
                movePlayerContainer(Out);
                change();

                moveGuardContainer(In);
                _guardText.text = "Less crying and more practice, that's it";
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
                _guardText.text = "The enemy will try to blind you from time to time, you can't do anything if you don't see";
                yield return new WaitForSeconds(5);
                _guardText.text = "Hold the left click and drag from side to side to clean the dirt";
                yield return new WaitForSeconds(5);
                moveGuardContainer(Out);
                Overlay(0);
                tutorial_UI.fadeTimer(0);
                tutorial_UI.toggleInput(8, 1);
                break;
            case 10:
                moveGuardContainer(In); _guardText.text = "Enough, that's all you need to know...";
                yield return new WaitForSeconds(4);
                _guardText.text = "Now go to your cell and get some sleep, your first fight will be soon";
                yield return new WaitForSeconds(4);
                moveGuardContainer(Out);
                showEndScreen(true);
                Overlay(1, .8f);
                break;
        }
    }

    void toogleSymbols(int symbol, bool inOut)
    {
        _symbols.GetChild(symbol).gameObject.SetActive(inOut);
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
            Overlay(1, .8f);
            _readyText.gameObject.SetActive(true);
            _readyText.DOLocalMoveY(100, 1);
            _fightButton.gameObject.SetActive(true);
            _fightButton.DOLocalMoveY(-110, 1);
            _resetButton.gameObject.SetActive(true);
            _resetButton.DOLocalMoveY(-110, 1);
        } else
        {
            Overlay(0);
            _readyText.DOLocalMoveY(250, 1).OnComplete(()=> _readyText.gameObject.SetActive(false));
            _fightButton.DOLocalMoveY(-300, 1).OnComplete(() => _fightButton.gameObject.SetActive(false));
            _resetButton.DOLocalMoveY(-300, 1).OnComplete(() => _resetButton.gameObject.SetActive(false));
        }
    }

    public void toogleEndTutorial(int inOrOut)
    {
        int In = -250;
        int Out = -190;
        //in = 1
        if (inOrOut == 1)
        {
            _endTutButton.gameObject.SetActive(true);
            _endTutButton.DOLocalMoveY(Out, .5f);
        } else
        {
            _endTutButton.DOLocalMoveY(In, .5f);
        }
    }

    public void OnFinishTutorualButton()
    {
        if (!tutorial_UI.timerRunning)
        {
            toogleEndTutorial(0);
            showEndScreen(true);
        }
    }
}
