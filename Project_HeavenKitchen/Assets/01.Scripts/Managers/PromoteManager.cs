using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PromoteManager : MonoBehaviour
{
    public static PromoteManager Promote;
    public static Calender Calender;
    public static PromoteResultText PromoteResult;

    public static List<PromoteType> PromoteScheduleList = new List<PromoteType>();
    [HideInInspector] public int ScheduleIndex = 0;

    [HideInInspector] public int ScheduleRepeatCount;
    [HideInInspector] public int ScheduleCurrentRepeatCount;

    public Transform PromoteObjectsTrm;
    public Transform promoteArrowBoxTrm;

    [Header("StressSprite")]
    public Sprite stressRedSpr;
    public Sprite stressYellowSpr;
    public Sprite stressGreenSpr;

    [Header("StatArrow")]
    public Sprite statArrowRedUp;
    public Sprite statArrowRedDown;
    public Sprite statArrowBlueUp;
    public Sprite statArrowBlueDown;

    [Header("Test")]
    public CanvasGroup blackFadeScreen;

    private void Awake()
    {
        if (!Promote)
        {
            Promote = this;
        }

        Calender = FindObjectOfType<Calender>();
        PromoteResult = FindObjectOfType<PromoteResultText>();
    }

    private void Start()
    {
        GameObject promoteStatArrowUI = Global.Resource.Load<GameObject>("UI/Promote/PromoteStatArrowUI");
        Global.Pool.CreatePool<PromoteStatArrowUI>(promoteStatArrowUI, promoteArrowBoxTrm, 5);
    }

    public static void GoNextPromote()
    {
        if(Promote.ScheduleIndex < PromoteScheduleList.Count)
        {
            PromoteOff();
            Transform statName = Promote.PromoteObjectsTrm.Find(PromoteScheduleList[Promote.ScheduleIndex].studySO.studyType.ToString());
            Transform promoteName = statName.Find($"{PromoteScheduleList[Promote.ScheduleIndex].studySO.studyType}{PromoteScheduleList[0].level}");

            if (statName == null) print("스탯이 없음!");
            if (promoteName == null) print("육성 이름이 없음!");

            statName.gameObject.SetActive(true);
            promoteName.gameObject.SetActive(true);

            PromoteAct act = promoteName.GetComponent<PromoteAct>();
            act.StartPromote();

            Promote.ScheduleRepeatCount = PromoteScheduleList[Promote.ScheduleIndex].repeatCount;
            Promote.ScheduleIndex++;
        }
        else
        {
            print("한달이 끝남!");
            Time.timeScale = 1;
            Global.UI.UIFade(Promote.blackFadeScreen, Define.UIFadeType.IN, 1, false, () =>
             {
                 Promote.StartCoroutine(GoToKitchen());
             });
        }
    }

    private static IEnumerator GoToKitchen()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Cooking");
    }

    private static void PromoteOff()
    {
        for (int i = 0; i < Promote.PromoteObjectsTrm.childCount; i++)
        {
            Promote.PromoteObjectsTrm.GetChild(i).gameObject.SetActive(false);
        }

        PromoteAct[] allActs = Promote.PromoteObjectsTrm.GetComponentsInChildren<PromoteAct>(true);
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