using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calender : MonoBehaviour
{
    DateTime now;

    [SerializeField] TextMeshProUGUI yearMonthText;

    [SerializeField] CalenderNumberUI numberUIPrefab;
    [SerializeField] Transform numberUITrm;
    private CalenderNumberUI[] numberUIs;

    [Header("Deco")]
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
        yearMonthText.text = $"{now.Year - 2023 + 1}³âÂ÷ {now.Month}¿ù";

        DateTime firstOfMonth = new DateTime(now.Year, now.Month, 1);

        int firstDay = FirstOfMonthDay(now);

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

            numberUI.image.sprite = onSpr;
        }
    }

    public int FirstOfMonthDay(DateTime _date)
    {
        DateTime firstOfMonth = new DateTime(_date.Year, _date.Month, 1);
        return (int)firstOfMonth.DayOfWeek;
    }

    public int DaysInMonth(DateTime _date)
    {
        return DateTime.DaysInMonth(_date.Year, _date.Month);
    }
}
