using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GoalManager : MonoBehaviour
{
    private TMP_Text _goalText;
    private TMP_Text _goalTrack;

    private int _tryIndex;
    private int _maxTry;

    void Start()
    {
        _goalText = transform.GetChild(1).GetComponent<TMP_Text>();
        _goalTrack = transform.GetChild(2).GetComponent<TMP_Text>();

        _tryIndex = 0;
        _maxTry = 0;
    }



    public void MoveGoal(int inOut, bool reset = false)
    {
        // 1 = in
        if (inOut == 1)
        {
            transform.DOLocalMoveX(307, 0.3f);
        } else
        {
            transform.DOLocalMoveX(495, 0.3f).OnComplete(()=> {
                if (reset)
                {
                    _tryIndex = 0;
                }
            });
        }
    }

    public void SetGoal(string GoalText, int maxTry) // Set the objective and the number of tries
    {
        _maxTry = maxTry;
        _goalText.text = GoalText;
        _goalTrack.text = _tryIndex + " / " + _maxTry;
    }

    public void UpdateGoalIndex() // Increases the number of tries that the player has made
    {
        if (_tryIndex != _maxTry)
        {
            _tryIndex++;
            _goalTrack.text = _tryIndex + " / " + _maxTry;
        }
    }
}
