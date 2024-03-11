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

    private static int _interactionIndex;
    private static int _currentBlock;
    private bool canChangeInteraction = true;


    // Inputs:
    /*
     - 0 = Attack
     - 1 = Defend
     - 2 = Dodge
     - 3 = Focus
     - 4 = Rest
     - 5 = Attack 2
     - 6 = Counter
     - 7 = Counter 2
     - 8 = Dirt
     */

    // Interactions:
    /*
     - 1 = Attack
     - 2 = Defend
     - 3 = Dodge
     - 4 = Focus
     - 5 = Rest
     - 6 = Counter
     - 7 = Counter 2
     - 8 = Super Attack
     - 9 = Dirt
     - 10 = end 
     */
    void Start()
    {
        importTutorial();
        _interactionIndex = 10;
        _currentBlock = 1;

        StartCoroutine(Interactions(_interactionIndex, 0.2f, _currentBlock));
    }

    private void Update()
    {
        manageControlClick();
    }

    void manageControlClick()
    {
        if (Input.GetMouseButtonDown(0) && canChangeInteraction)
        {
            _currentBlock++;
            StartCoroutine(Interactions(_interactionIndex, 0.1f, _currentBlock));
        }
    }

    public IEnumerator Interactions(int index, float delay, int block = 0)
    {
        int In = 0;
        int Out = 1;
        yield return new WaitForSeconds(delay);

        Overlay(1, .6f);

        switch (index)
        {
            case 1: // Attack Tutorial
                switch (block)
                {
                    case 1:
                        moveGuardContainer(In);
                        _guardText.text = "I hope you know how to use a sword...";
                        break;

                    case 2:
                        moveGuardContainer(Out);
                        movePlayerContainer(In);
                        _playerText.text = "...";
                        break;

                    case 3:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        toogleSymbols(0, true);
                        toogleSymbols(1, true);
                        _guardText.text = "Select the attack command     , wait for the timer and click as many targets     as you can";
                        break;

                    case 4:
                        toogleSymbols(0, false);
                        toogleSymbols(1, false);
                        _guardText.text = "Try it a few times...";
                        break;

                    case 5:
                        moveGuardContainer(Out);
                        Overlay(0);
                        StartCoroutine(tutorial_UI.toggleInput(0, 1));
                        tutorial_UI.attackDetailTutorial(1);
                        FinishInteraction();
                        break;
                }
                break;

            case 2: // Defense tutorial
                switch (block)
                {
                    case 1:
                        movePlayerContainer(In); 
                        _playerText.text = "Not bad...";
                        StartInteraction();
                        break;

                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "You better get use to it... let's try some defense";
                        break;

                    case 3:
                        toogleSymbols(2, true);
                        toogleSymbols(3, true);
                        _guardText.text = "Click the defend command     and press     when the circle is \nin the green area...";
                        break;

                    case 4:
                        toogleSymbols(2, false);
                        toogleSymbols(3, false);
                        _guardText.text = "If you succeed you will get a shield point, which will allow you to counter the enemy attack";
                        break;

                    case 5:
                        moveGuardContainer(Out);
                        Overlay(0);
                        StartCoroutine(tutorial_UI.toggleInput(1, 1));
                        tutorial_UI.defendDetailTutorial(1);
                        FinishInteraction();
                        break;
                }
                break;

            case 3: // Dodge tutorial
                switch (block)
                {
                    case 1:
                        movePlayerContainer(In); 
                        _playerText.text = "I think I got this...";
                        StartInteraction();
                        break;

                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "Don't get too confident, let's see your moves...";
                        break;

                    case 3:
                        toogleSymbols(4, true);
                        _guardText.fontSize = 20;
                        _guardText.text = "Select the dodge command     , wait for the timer and press the arrow keys in the right order, this will avoid the next enemy attack...";
                        break;

                    case 4:
                        toogleSymbols(4, false);
                        moveGuardContainer(Out);
                        Overlay(0);
                        StartCoroutine(tutorial_UI.toggleInput(2, 1));
                        tutorial_UI.dodgeDetailTutorial(1);
                        FinishInteraction();
                        break;
                }
                break;

            case 4: // Focus tutorial
                switch (block)
                {
                    case 1:
                        _guardText.fontSize = 22;
                        moveGuardContainer(In); 
                        _guardText.text = "Decent enough, don't forget to use them later in the arena";
                        StartInteraction();
                        break;

                    case 2:
                        moveGuardContainer(Out);
                        movePlayerContainer(In); 
                        _playerText.text = "I will do my best...";
                        break;

                    case 3:
                        movePlayerContainer(Out);
                        moveGuardContainer(In); 
                        _guardText.text = "We will see about that...";
                        break;

                    case 4:
                        _guardText.text = "Let's learn to focus now, this will give you a speed boost and you will deal more damage for a moment";
                        break;

                    case 5:
                        for (int i = 5; i < 9; i++) { toogleSymbols(i, true); }
                        _guardText.text = "Select the focus command     and press     when the \nblock     is inside the \nmoving box     ";
                        break;

                    case 6:
                        for (int i = 5; i < 9; i++) { toogleSymbols(i, false); }
                        moveGuardContainer(Out);
                        Overlay(0);
                        tutorial_UI.focusDetailTutorial(1);
                        StartCoroutine(tutorial_UI.toggleInput(3, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 5: // Rest tutorial
                switch (block)
                {
                    case 1:
                        movePlayerContainer(In);
                        _playerText.text = "Can I have a break?";
                        FindObjectOfType<Player>().SetCurrentStamina(0);
                        StartInteraction();
                        break;
                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "A break??, your oponent won't give you a break...";
                        break;
                    case 3:
                        for (int i = 9; i < 12; i++) { toogleSymbols(i, true); }
                        _guardText.text = "Select the rest command     and smash the arrow keys \nleft     and right     to recover stamina";
                        break;
                    case 4:
                        for (int i = 9; i < 12; i++) { toogleSymbols(i, false); }
                        _guardText.text = "Stamina allows you to execute commands, if your stamina is empty you won't be able to perform any action!";
                        break;
                    case 5:
                        moveGuardContainer(Out);
                        Overlay(0);
                        tutorial_UI.restDetailTutorial(1);
                        StartCoroutine(tutorial_UI.toggleInput(4, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 6: // Counter tutorial
                switch (block)
                {
                    case 1:
                        movePlayerContainer(In);
                        _playerText.text = "I think I'm ready for a real fight now...";
                        StartInteraction();
                        break;
                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "Hell no, you still have much to learn, you don't even know how to block an attack...";
                        break;
                    case 3:
                        toogleSymbols(12, true);
                        _guardText.text = "Smash     to rotate the shield and block the atack";
                        break;
                    case 4:
                        toogleSymbols(12, false);
                        _guardText.text = "Give it a few tries...";
                        break;
                    case 5:
                        moveGuardContainer(Out);
                        Overlay(0);
                        StartCoroutine(tutorial_UI.toggleInput(6, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 7: // Super counter tutorial
                switch (block)
                {
                    case 1:
                        movePlayerContainer(In);
                        _playerText.text = "Easy...";
                        StartInteraction();
                        break;
                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "Hahahaha, don't make me laugh boy...";
                        break;
                    case 3:
                        _guardText.text = "Let's see how easy is to block a super attack...";
                        break;
                    case 4:
                        toogleSymbols(13, true);
                        toogleSymbols(14, true);
                        _guardText.text = "Move the arrows left     and right     to control the shield and block the swords";
                        break;
                    case 5:
                        toogleSymbols(13, false);
                        toogleSymbols(14, false);
                        moveGuardContainer(Out);
                        Overlay(0);
                        StartCoroutine(tutorial_UI.toggleInput(7, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 8: // Super Attack tutorial
                switch (block)
                {
                    case 1:
                        tutorial_UI.hasShownDetail_superCounter = true;
                        movePlayerContainer(In);
                        _playerText.text = "How can I even survive that??";
                        StartInteraction();
                        break;

                    case 2:
                        movePlayerContainer(Out);
                        moveGuardContainer(In);
                        _guardText.text = "Less crying and more practice, that's it";
                        break;

                    case 3:
                        _guardText.text = "You will also have a Super Attack when your adrenaline bar is full...";
                        break;

                    case 4:
                        toogleSymbols(15, true);
                        _guardText.text = "Select the super attack command    , wait for the \ntimer and click as many \ntargets as you can";
                        break;

                    case 5:
                        toogleSymbols(15, false);
                        moveGuardContainer(Out);
                        Overlay(0);
                        tutorial_UI.fadeTimer(1);
                        tutorial_UI.superAttackDetailTutorial(1);
                        StartCoroutine(tutorial_UI.toggleInput(5, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 9: // Dirt tutorial
                switch (block)
                {
                    case 1:
                        moveGuardContainer(In);
                        _guardText.text = "Good... but fights are not always fair";
                        StartInteraction();
                        break;

                    case 2:
                        _guardText.text = "The enemy will try to blind you from time to time, you can't do anything if you don't see";
                        break;

                    case 3:
                        _guardText.text = "Hold the left click and drag from side to side to clean the dirt";
                        break;

                    case 4:
                        moveGuardContainer(Out);
                        Overlay(0);
                        tutorial_UI.fadeTimer(0);
                        StartCoroutine(tutorial_UI.toggleInput(8, 1));
                        FinishInteraction();
                        break;
                }
                break;

            case 10: // End tutorial
                switch (block)
                {
                    case 1:
                        moveGuardContainer(In);
                        _guardText.text = "Enough, that's all you need to know...";
                        StartInteraction();
                        break;

                    case 2:
                        _guardText.text = "Now, go to your cell and get some sleep, your first fight will be soon";
                        break;

                    case 3:
                        moveGuardContainer(Out);
                        showEndScreen(true);
                        Overlay(1, .8f);
                        FinishInteraction();
                        break;
                }
                break;
        }
    }

    void StartInteraction()
    {
        _currentBlock = 1;
        canChangeInteraction = true;
    }

    void FinishInteraction() 
    {
        _interactionIndex++;
        _currentBlock = 0;
        canChangeInteraction = false;
    }

    void toogleSymbols(int symbol, bool inOut)
    {
        _symbols.GetChild(symbol).gameObject.SetActive(inOut);
    }
    public void Overlay(int showOrHide, float intensity = 0)
    {
        if (showOrHide == 0)
        {
            _overlay.DOFade(0, 0.4f).OnComplete(() => _overlay.gameObject.SetActive(false));
        } else if (showOrHide == 1)
        {
            _overlay.gameObject.SetActive(true);
            _overlay.DOFade(intensity, 0.4f);
        }
    }
    void moveGuardContainer(int inOrOut)
    {   // 0 = In  1 = Out
        if (inOrOut == 0)
        {
            _guardFrame.SetActive(true);
            _guardFrame.transform.DOLocalMoveX(70, 0.2f);
        } else
        {
            _guardFrame.transform.DOLocalMoveX(600, 0.2f).OnComplete(() => _guardFrame.SetActive(false));
        }
    }

    // Special interactions
    public IEnumerator specialGuardInteraction(bool Pass, int interactionIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        Overlay(1, .6f);
        moveGuardContainer(0);
        switch (interactionIndex)
        {
            case 1: // Normal counter
                if (!Pass)
                {
                    _guardText.text = "You can do better than that... Try again";
                }
                else
                {
                    _guardText.text = "Not bad rookie, try again!";
                }
                StartCoroutine(tutorial_UI.toggleInput(6, 1));
                break;

            case 2: // Super counter
                if (!Pass)
                {
                    _guardText.text = "Not so easy anymore right?, try again!";
                } else
                {
                    _guardText.text = "Impressive, try again!";
                }
                StartCoroutine(tutorial_UI.toggleInput(7, 1));
                break;

            case 3: // Super Attack
                if (!Pass)
                {
                    _guardText.text = "You can do much better than that!";
                }
                else
                {
                    _guardText.text = "You're learning fast!";
                }
                StartCoroutine(tutorial_UI.toggleInput(5, 1));
                break;

            case 4: // Dirt
                _guardText.text = "Good job, try again!";
                StartCoroutine(tutorial_UI.toggleInput(8, 1));
                break;
            default:
                Debug.Log("Out of index");
                break;
        }

        yield return new WaitForSeconds(3);

        Overlay(0);
        moveGuardContainer(1);
    }
    public void superHitCheck()
    {
        if (!tutorial_UI.hasShownDetail_superAttack && tutorial_UI.GetNumberOfTries() >= 1)
        {
            if (superATKManager.GetHits() <= 5)
            {
                StartCoroutine(specialGuardInteraction(false, 3, 1));
            }
            else
            {
                StartCoroutine(specialGuardInteraction(true, 3, 1));
            }
        }
    }
    void movePlayerContainer(int inOrOut)
    {
        // 0 = In - 1 = Out
        if (inOrOut == 0)
        {
            _playerFrame.SetActive(true);
            _playerFrame.transform.DOLocalMoveX(-70, 0.2f);
        }
        else
        {
            _playerFrame.transform.DOLocalMoveX(-600, 0.2f).OnComplete(() => _playerFrame.SetActive(false));
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
            _readyText.DOLocalMoveY(250, 1).OnComplete(() => _readyText.gameObject.SetActive(false));
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


    // Imports
    void importTutorial()
    {
        tutorial_UI = FindObjectOfType<Tutorial_UI>();
    }

    // Getter Setters
    public static int GetCurrentInteraction()
    {
        return _interactionIndex;
    }
}
