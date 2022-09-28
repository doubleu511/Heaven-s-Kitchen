using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PageArrowUI : MonoBehaviour
{
    [SerializeField] Button leftArrow;
    [SerializeField] Button rightArrow;
    [SerializeField] TextMeshProUGUI pageValue;

    [Header("Pages")]
    [SerializeField] GameObject[] pages;

    private int currentPage = 0;

    private void Start()
    {
        leftArrow.onClick.AddListener(() =>
        {
            currentPage--;
            RefreshPage();
        });

        rightArrow.onClick.AddListener(() =>
        {
            currentPage++;
            RefreshPage();
        });
    }

    private void OnEnable()
    {
        currentPage = 0;
        RefreshPage();
    }

    private void RefreshPage()
    {
        pageValue.text = $"{currentPage + 1} / {pages.Length}";

        leftArrow.gameObject.SetActive(currentPage > 0);
        rightArrow.gameObject.SetActive(currentPage < pages.Length - 1);

        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }
    }
}
