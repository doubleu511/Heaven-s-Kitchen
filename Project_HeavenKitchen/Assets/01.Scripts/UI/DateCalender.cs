using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Globalization;

public class DateCalender : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI yearText;
    [SerializeField] TextMeshProUGUI dayText;
    [SerializeField] TextMeshProUGUI dayOfWeekText;

    private void Start()
    {
        RefreshDateCalender();
    }

    public void RefreshDateCalender()
    {
        DateTime now = Calender.now;

        yearText.text = $"{now.Year - 2022}³âÂ÷";
        dayText.text = $"{now.Month:00}/{now.Day:00}";

        dayOfWeekText.text = $"({now.DayOfWeek.ToString().Substring(0, 3)})";
    }
}
