using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMultiplicationUI : MonoBehaviour
{
    private Button button;
    [SerializeField] Image buttonImage;

    [SerializeField] Sprite scaleOne;
    [SerializeField] Sprite scaleTwo;
    [SerializeField] Sprite scaleFour;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(CallBackTimeScaleTwo);
    }

    private void CallBackTimeScaleOne()
    {
        Time.timeScale = 1;
        buttonImage.sprite = scaleOne;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CallBackTimeScaleTwo);
    }

    private void CallBackTimeScaleTwo()
    {
        Time.timeScale = 2;
        buttonImage.sprite = scaleTwo;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CallBackTimeScaleFour);
    }

    private void CallBackTimeScaleFour()
    {
        Time.timeScale = 4;
        buttonImage.sprite = scaleFour;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(CallBackTimeScaleOne);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
