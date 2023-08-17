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

    void Start()
    {
        interactions(1);
    }

    void interactions(int index)
    {
        showDialogue();
        switch (index)
        {
            case 1:
                _guardFrame.SetActive(true);
                _guardFrame.transform.DOLocalMoveX(70, 0.2f);
                _guardText.text = lines[0];
                _playerText.text = lines[1];
                Debug.Log("First interaction");
                break;
        }
    }
    void showDialogue()
    {
        _overlay.gameObject.SetActive(true);
        _overlay.DOFade(0.6f, 0.2f);
    }
}
