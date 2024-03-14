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
        _goalText = transform.GetChild(0).GetComponent<TMP_Text>();
        _goalTrack = transform.GetChild(1).GetComponent<TMP_Text>();

        _tryIndex = 0;
        _maxTry = 0;
    }



    public void MoveGoal(int inOut)
    {
        // 1 = in
        if (inOut == 1)
        {
            transform.DOLocalMoveX(307, 0.3f);
        } else
        {
            transform.DOLocalMoveX(495, 0.3f).OnComplete(ResetTry);
        }
    }

    public void SetGoal(string Text, int Goal)
    {
        _maxTry = Goal;
        _goalText.text = Text;
        _goalTrack.text = _tryIndex + " / " + _maxTry.ToString();
    }

    public void UpdateTry()
    {
        _tryIndex++;
        _goalTrack.text = _tryIndex + " / " + _maxTry.ToString();
    }

    public bool CheckGoal()
    {
        if (_tryIndex == _maxTry)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void ResetTry()
    {
        _tryIndex = 0;
        _maxTry = 0;
    }
}
