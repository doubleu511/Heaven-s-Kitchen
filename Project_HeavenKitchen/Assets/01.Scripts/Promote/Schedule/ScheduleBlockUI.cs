using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScheduleBlockUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isHold = false;
    private float holdTime = 0.0f;

    [SerializeField] PromoteStudySO studyInfo;
    [SerializeField] int scheduleLevel = 0;

    private Image myImage;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }

    private void Update()
    {
        if(isHold)
        {
            holdTime += Time.deltaTime;
            if(holdTime > 0.8f)
            {
                isHold = false;
                PromoteManager.Calender.statDetailHandler.ShowInfo(studyInfo.statTranslationId, studyInfo.studys[scheduleLevel]);
            }
        }
    }

    private void CallScheduleAddBtnOnClicked()
    {
        PromoteManager.Calender.AddSchedule(studyInfo, scheduleLevel);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHold = true;
        myImage.color = new Color32(200, 200, 200, 255);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(holdTime < 0.8f)
        {
            CallScheduleAddBtnOnClicked();
        }
        else
        {
            PromoteManager.Calender.statDetailHandler.HideInfo();
        }

        isHold = false;
        holdTime = 0.0f;
        myImage.color = Color.white;
    }
}
