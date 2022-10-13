using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] GameObject[] tutorialPanels;
    [SerializeField] Button restartButton;

    private void Start()
    {
        restartButton.onClick.AddListener(() =>
        {
            DG.Tweening.DOTween.KillAll();
            SceneManager.LoadScene("Title");
        });
    }

    public void ShowTutorial(int index)
    {
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(i == index);
        }
    }

    public void HideTutorial()
    {
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(false);
        }
    }
}
