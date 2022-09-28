using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromoteManager : MonoBehaviour
{
    public static PromoteManager Global;
    public static Calender Calender;
    public static PromoteResultText PromoteResult;

    public static List<PromoteType> PromoteScheduleList = new List<PromoteType>();

    [HideInInspector] public int ScheduleRepeatCount;
    [HideInInspector] public int ScheduleCurrentRepeatCount;

    public Transform PromoteObjectsTrm;

    public Sprite stressRedSpr;
    public Sprite stressYellowSpr;
    public Sprite stressGreenSpr;

    private void Awake()
    {
        if (!Global)
        {
            Global = this;
        }

        Calender = FindObjectOfType<Calender>();
        PromoteResult = FindObjectOfType<PromoteResultText>();
    }

    public static void GoNextPromote()
    {
        if(PromoteScheduleList.Count > 0)
        {
            PromoteOff();
            Transform statName = Global.PromoteObjectsTrm.Find(PromoteScheduleList[0].studySO.studyType.ToString());
            Transform promoteName = statName.Find($"{PromoteScheduleList[0].studySO.studyType}{PromoteScheduleList[0].level}");

            if (statName == null) print("스탯이 없음!");
            if (promoteName == null) print("육성 이름이 없음!");

            statName.gameObject.SetActive(true);
            promoteName.gameObject.SetActive(true);

            PromoteAct act = promoteName.GetComponent<PromoteAct>();
            act.StartPromote();

            Global.ScheduleRepeatCount = PromoteScheduleList[0].repeatCount;

            PromoteScheduleList.RemoveAt(0);
        }
        else
        {
            print("한달이 끝남!");
        }
    }

    private static void PromoteOff()
    {
        for (int i = 0; i < Global.PromoteObjectsTrm.childCount; i++)
        {
            Global.PromoteObjectsTrm.GetChild(i).gameObject.SetActive(false);
        }

        PromoteAct[] allActs = Global.PromoteObjectsTrm.GetComponentsInChildren<PromoteAct>(true);
        for (int i = 0; i < allActs.Length; i++)
        {
            allActs[i].gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class PromoteType
{
    public PromoteStudySO studySO;
    public int level;
    public int repeatCount;

    public PromoteType(PromoteStudySO _studySO, int _level, int _repeatCount)
    {
        studySO = _studySO;
        level = _level;
        repeatCount = _repeatCount;
    }
}