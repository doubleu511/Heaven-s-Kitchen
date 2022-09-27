using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class Calender : MonoBehaviour
{
    DateTime now;

    [SerializeField] TextMeshProUGUI yearMonthText;

    [SerializeField] CalenderNumberUI numberUIPrefab;
    [SerializeField] Transform numberUITrm;
    private CalenderNumberUI[] numberUIs;
    private List<CalenderNumberUI> currentMonthNumberUIs = new List<CalenderNumberUI>();

    private int[] monthSegments = new int[3] { 10, 10, 10 };
    private int monthSegmentIndex = 0;

    [Header("Schedule")]
    [SerializeField] Button scheduleResetButton;
    [SerializeField] Button studyBookmark;
    [SerializeField] Button restBookmark;
    public PromoteStatDetailHandler statDetailHandler;
    private bool isStudyPage = true;

    [Header("Highlighter")]
    [SerializeField] CanvasGroup highlighter;
    private readonly float scheduleWriteSpeed = 0.15f;
    private Sequence highlighterSeq;
    private Sequence scheduleSeq;

    [Header("Deco")]
    [SerializeField] Sprite testSpr;
    [SerializeField] Sprite onSpr;
    [SerializeField] Sprite offSpr;
    [SerializeField] Color sunColor;
    [SerializeField] Color satColor;

    private void Start()
    {
        for (int i = 0; i < 42; i++)
        {
            Instantiate(numberUIPrefab.gameObject, numberUITrm);
        }
        numberUIs = GetComponentsInChildren<CalenderNumberUI>();
        UtilClass.ForceRefreshGrid(numberUITrm);

        scheduleResetButton.onClick.AddListener(CallResetScheduleBtnOnClicked);
        studyBookmark.onClick.AddListener(() => CallBookmarkBtnOnClicked(true));
        restBookmark.onClick.AddListener(() => CallBookmarkBtnOnClicked(false));

        now = new DateTime(2023, 3, 1);
        Refresh();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            now = now.AddMonths(-1);
            Refresh();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            now = now.AddMonths(1);
            Refresh();
        }
    }

    private void Refresh()
    {
        currentMonthNumberUIs.Clear();

        yearMonthText.text = $"{now.Year - 2023 + 1}���� {now.Month}��";

        DateTime firstOfMonth = new DateTime(now.Year, now.Month, 1);
        int firstDay = FirstOfMonthDay(now);

        int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
        SetMonthSegment(daysInMonth);

        for (int i = 0; i < numberUIs.Length; i++)
        {
            DayInit(numberUIs[i], firstOfMonth, i - firstDay);
        }
    }

    private void DayInit(CalenderNumberUI numberUI, DateTime date, int index)
    {
        DateTime myDate = date.AddDays(index);

        numberUI.numberText.text = myDate.Day.ToString();
        if (index < 0 || index >= DateTime.DaysInMonth(date.Year, date.Month))
        {
            numberUI.numberText.color = Color.gray;
            numberUI.image.sprite = offSpr;
        }
        else
        {
            switch (myDate.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    numberUI.numberText.color = sunColor;
                    break;
                case DayOfWeek.Saturday:
                    numberUI.numberText.color = satColor;
                    break;
                default:
                    numberUI.numberText.color = Color.black;
                    break;
            }

            currentMonthNumberUIs.Add(numberUI);
            numberUI.image.sprite = onSpr;
        }
    }

    private int FirstOfMonthDay(DateTime _date)
    {
        DateTime firstOfMonth = new DateTime(_date.Year, _date.Month, 1);
        return (int)firstOfMonth.DayOfWeek;
    }

    public void AddSchedule(Define.StatType statType, int level, Sprite fillSprite)
    {
        if (monthSegmentIndex > monthSegments.Length - 1) return;

        highlighterSeq.Complete();
        scheduleSeq.Complete();

        int startDay = 1;
        int endDay = 0;

        for (int i = 0; i < monthSegmentIndex; i++)
        {
            startDay += monthSegments[i];
        }

        for (int i = 0; i < monthSegmentIndex + 1; i++)
        {
            endDay += monthSegments[i];
        }

        ScheduleMove(startDay, endDay, fillSprite);
        monthSegmentIndex++;
    }

    private void ScheduleMove(int fromDay, int toDay, Sprite fillSprite)
    {
        if (toDay < fromDay) return;

        highlighterSeq = DOTween.Sequence();
        scheduleSeq = DOTween.Sequence();

        highlighterSeq.AppendCallback(() =>
        {
            highlighter.alpha = 0;
            highlighter.DOFade(1, 0.2f);
        });

        for (int i = fromDay; i <= toDay; i++)
        {
            scheduleSeq.Append(currentMonthNumberUIs[i - 1].FillSprite(fillSprite, scheduleWriteSpeed));
        }
        HighlighterMove(fromDay, toDay);
    }

    public void HighlighterMove(int fromDay, int toDay)
    {
        if (toDay < fromDay) return;

        highlighterSeq.AppendCallback(() =>
        {
            highlighter.transform.position = GetNumberUIPos(fromDay - 1, 1);
        });

        for (int i = fromDay; i <= toDay; i++)
        {
            DateTime day = new DateTime(now.Year, now.Month, i);
            if (day.DayOfWeek == DayOfWeek.Saturday && i != toDay)
            {
                highlighterSeq.Append(highlighter.transform.DOMove(GetNumberUIPos(i - 1, 2), (i - fromDay + 1) * scheduleWriteSpeed).SetEase(Ease.Linear));
                HighlighterMove(i + 1, toDay);
                return;
            }
        }

        highlighterSeq.Append(highlighter.transform.DOMove(GetNumberUIPos(toDay - 1, 2), (toDay - fromDay + 1) * scheduleWriteSpeed).SetEase(Ease.Linear));
        highlighterSeq.AppendInterval(0.2f);
        highlighterSeq.Append(highlighter.DOFade(0, 0.2f));
    }

    private void SetMonthSegment(int daysInMonth)
    {
        switch (daysInMonth)
        {
            case 28:
                monthSegments = new int[3] { 9, 9, 10 };
                break;
            case 29:
                monthSegments = new int[3] { 9, 10, 10 };
                break;
            case 30:
                monthSegments = new int[3] { 10, 10, 10 };
                break;
            case 31:
            default:
                monthSegments = new int[3] { 10, 10, 11 };
                break;
        }
    }

    private void CallResetScheduleBtnOnClicked()
    {
        highlighterSeq.Complete();
        scheduleSeq.Complete();

        for (int i = 0; i < currentMonthNumberUIs.Count; i++)
        {
            currentMonthNumberUIs[i].EmptySprite();
        }

        monthSegmentIndex = 0;
    }

    private void CallBookmarkBtnOnClicked(bool isStudy)
    {
        if(isStudy != isStudyPage)
        {
            ScheduleBookmarkUI study = studyBookmark.GetComponent<ScheduleBookmarkUI>();
            ScheduleBookmarkUI rest = restBookmark.GetComponent<ScheduleBookmarkUI>();

            study.SetFade(isStudy);
            rest.SetFade(!isStudy);

            isStudyPage = !isStudyPage;
        }
    }

    /// <summary>
    /// NumberUI�� �����ǰ� �ޱ�
    /// </summary>
    /// <param name="index">NumberUI�� �ε���</param>
    /// <param name="posIndex">0�̸� ���, 1�̸� ����, 2�� ������</param>
    /// <returns></returns>
    private Vector3 GetNumberUIPos(int index, int posIndex)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentMonthNumberUIs[index].transform.position);

        switch (posIndex)
        {
            default:
            case 0:
                break;
            case 1:
                screenPos.x -= (currentMonthNumberUIs[index].image.rectTransform.sizeDelta.x / 2) * Screen.width / 1920;
                break;
            case 2:
                screenPos.x += (currentMonthNumberUIs[index].image.rectTransform.sizeDelta.x / 2) * Screen.width / 1920;
                break;
        }

        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}