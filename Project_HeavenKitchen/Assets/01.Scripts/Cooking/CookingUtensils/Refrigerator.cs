using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Refrigerator : MinigameStarter
{
    private readonly int REFRIGERATOR_WIDTH = 2;
    private readonly int REFRIGERATOR_HEIGHT = 2;
    private readonly int REFRIGERATOR_SIZE = 800;

    private int curIndex = 0;

    [SerializeField] RectTransform content;

    [Header("Buttons")]
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;
    [SerializeField] Button topBtn;
    [SerializeField] Button bottomBtn;
    private Button[] buttons;

    protected override void Start()
    {
        base.Start();
        buttons = new Button[4] { leftBtn, rightBtn, topBtn, bottomBtn };
        RefreshButtonAppear();

        leftBtn.onClick.AddListener(() => MoveContent(curIndex - 1));
        rightBtn.onClick.AddListener(() => MoveContent(curIndex + 1));
        topBtn.onClick.AddListener(() => MoveContent(curIndex - REFRIGERATOR_WIDTH));
        bottomBtn.onClick.AddListener(() => MoveContent(curIndex + REFRIGERATOR_WIDTH));
    }

    private void MoveContent(int index)
    {
        curIndex = index;
        int col = curIndex % REFRIGERATOR_WIDTH;
        int row = curIndex / REFRIGERATOR_HEIGHT;

        content.DOAnchorPos(new Vector2(col * -REFRIGERATOR_SIZE, row * REFRIGERATOR_SIZE), 1).OnComplete(() =>
        {
            RefreshButtonAppear();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
        });

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    private void RefreshButtonAppear()
    {
        int col = curIndex % REFRIGERATOR_WIDTH;
        int row = curIndex / REFRIGERATOR_HEIGHT;

        leftBtn.gameObject.SetActive(col > 0);
        rightBtn.gameObject.SetActive(col < REFRIGERATOR_WIDTH - 1);
        topBtn.gameObject.SetActive(row > 0);
        bottomBtn.gameObject.SetActive(row < REFRIGERATOR_WIDTH - 1);
    }
}
