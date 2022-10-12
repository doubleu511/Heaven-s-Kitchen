using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipButton : MonoBehaviour
{
    private Button button;
    [SerializeField] GameObject tipPanel;
    [SerializeField] Button cancelButton;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            tipPanel.SetActive(true);
        });

        cancelButton.onClick.AddListener(() =>
        {
            tipPanel.SetActive(false);
        });
    }
}
