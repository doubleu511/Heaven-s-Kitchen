using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

using static Define;

public class CounterHandler : MonoBehaviour
{
    public float GivenTime { get; set; } = 0;
    public float RemainTime { get; set; } = 0;
    public bool IsInCounter { get; set; } = false;

    // 주문 타이머의 간격 (x,y,z로 자른다)
    public Vector3 TimerBarInterval { get; set; }
    // 손님의 성격
    public GuestPersonality GuestPersonality { get; set; }
    // 현재 타이머의 상태
    public TimerInterval BarIntervalIndex { get; set; } = TimerInterval.GREEN;

    // 지금 타이머가 흐르는가, 시간이 얼마 없는가
    private bool isTimer = false;
    private bool isHurry = false;

    private RectTransform scrollRect;
    private GuestSO currentGuest;

    [SerializeField] CookingDialogPanel Dialog;
    [SerializeField] Button goToCookingBtn;
    [SerializeField] CounterTimeBarUI[] timeBarUis;

    [SerializeField] Animator guestAnimator;

    // 모든 손님의 풀
    [SerializeField] List<GuestSO> allGuests = new List<GuestSO>(); // Temp
    private int textIndex = 0; //realTemp;
    private Queue<GuestSO> guestQueue = new Queue<GuestSO>();

    // 가게의 현재 시간(창문)UI -> 따로빼야할까??
    [Header("WindowUI")]
    [SerializeField] Image cuttonImg;
    [SerializeField] Image[] windowBGs;
    private int bgIndex = 0;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject timeOverText;
    [SerializeField] Sprite morningBG;
    [SerializeField] Sprite daytimeBG;
    [SerializeField] Sprite sunsetBG;
    [SerializeField] Sprite duskBG;

    // 카운터에서 음식을 낼때 접시 데코
    [Header("Deco")]
    [SerializeField] CounterDishUI counterDishPrefab;
    [SerializeField] Transform counterDishTrm;
    public GuestTalkInKitchenUI guestTalk;
    
    // 오늘 날짜, 영업 종료 여부
    private DateTime todayDate;
    private bool isDayTimeOver = false;
    private float minuteTimer = 0f;

    // 여기 밑은 일단 여기 두는데 나중에 데이터관리할때 참고할것

    [SerializeField] ValuableEffectUI valuableEffectPrefab;
    [SerializeField] Transform valuableEffectTrm;
    public Sprite[] moneyTypeSprs;
    public Color[] plusMinusColors;
    private int gold = 0;
    private int starCandy = 0;

    private void Awake()
    {
        scrollRect = GetComponent<RectTransform>();

        goToCookingBtn.onClick.AddListener(() =>
        {
            SetScroll(false, false);
        });
    }

    private void Start()
    {
        Global.Pool.CreatePool<CounterDishUI>(counterDishPrefab.gameObject, counterDishTrm, 3);
        Global.Pool.CreatePool<ValuableEffectUI>(valuableEffectPrefab.gameObject, valuableEffectTrm, 3);

        // Test
        SetScroll(true, true);
        StartCoroutine(GuestEncounter());

        SetMoney(MoneyType.GOLD, 0);
        SetMoney(MoneyType.STARCANDY, 0);
        todayDate = new DateTime(2023, 1, 1, 9, 0, 0);
        RefreshWindow(true);
    }

    private void Update()
    {
        if(isTimer)
        {
            if(RemainTime > 0)
            {
                if (GuestPersonality != GuestPersonality.VERYGENEROUS)
                {
                    RemainTime -= Time.deltaTime;
                    RefreshTimeBarValue();
                }

                TimerInterval currentIntervalIndex = GetTimerIntervalIndex();
                if(currentIntervalIndex != BarIntervalIndex)
                {
                    // 수다스럽다면 구간을 지날때마다 일정 대화 출력
                    if (GuestPersonality == GuestPersonality.TALKATIVE)
                    {
                        GuestTalkCondition[] talkCondition = currentGuest.talkativeTalk.GetFilteredConditions();
                        int randomIdx = UnityEngine.Random.Range(0, talkCondition.Length);
                        GuestTalkCondition condition = talkCondition[randomIdx];

                        for (int i = 0; i < condition.translationIds.Length; i++)
                        {
                            CookingDialogInfo info = new CookingDialogInfo(condition.translationIds[i], 0, 0, (int)TextAnimationType.NONE, "");
                            guestTalk.AddBubbleMessage(info);
                        }
                    }

                    BarIntervalIndex = currentIntervalIndex;
                    guestTalk.SetEmotion(BarIntervalIndex);
                }

                if (!isHurry)
                {
                    if (RemainTime / GivenTime < TimerBarInterval.y) // 주황색 구간이 되면 빨리하라고 재촉
                    {
                        isHurry = true;
                        Global.Sound.Play("SFX/CookingScene/clock_hurry", Sound.Effect);
                        for (int i = 0; i < currentGuest.hurryUpTranlationIds.Length; i++)
                        {
                            CookingDialogInfo info = new CookingDialogInfo(currentGuest.hurryUpTranlationIds[i], 0, 0, (int)TextAnimationType.SHAKE, "");

                            guestTalk.AddBubbleMessage(info);
                        }
                    }
                }
            }
            else
            {
                // 요리 시간 초과
                RemainTime = 0;
                OrderFail();
            }
        }

        minuteTimer += Time.deltaTime;
        if(minuteTimer > 10)
        {
            minuteTimer -= 10;
            AddMinuteTime(20);
        }
    }

    public void AddDialog(int dialogId)
    {
        Dialog.StartDialog(TranslationManager.Instance.CookingDialog.GetDialog(dialogId));
    }

    private IEnumerator GuestEncounter()
    {
        while(true)
        {
            if (guestAnimator.GetCurrentAnimatorStateInfo(0).IsName("Guest_None"))
            {
                if (!isDayTimeOver)
                {
                    if (textIndex < allGuests.Count)
                    {
                        guestQueue.Enqueue(allGuests[textIndex]);
                        textIndex++;
                    }
                    else
                    {
                        int random = UnityEngine.Random.Range(0, allGuests.Count);
                        guestQueue.Enqueue(allGuests[random]);
                    }
                }

                if (guestQueue.Count > 0)
                {
                    currentGuest = guestQueue.Dequeue();

                    Dialog.GuestInit(currentGuest);
                    SetGuestPersonality();

                    Dialog.ShowSpeechBubble(false);
                    CookingManager.Counter.guestTalk.ShowGuestTalk(currentGuest);
                    if (!IsInCounter)
                    {
                        CookingDialogInfo info = new CookingDialogInfo(currentGuest.heyTranslationId);
                        info.text_animation_type = (int)TextAnimationType.SHAKE;

                        CookingManager.Counter.guestTalk.AddBubbleMessage(info);
                    }

                    guestAnimator.SetTrigger("Appear");
                    StartCoroutine(GuestDialogPlay());

                    Global.Sound.Play("SFX/CookingScene/counter_enter", Sound.Effect);
                }
                else
                {
                    // 끗
                }
            }
            yield return new WaitForSeconds(10);
        }
    }

    private IEnumerator GuestDialogPlay()
    {
        yield return new WaitForSeconds(2.5f);
        yield return new WaitUntil(() => IsInCounter);
        int random = UnityEngine.Random.Range(0, currentGuest.canPlayDialogIds.Length);
        AddDialog(currentGuest.canPlayDialogIds[random]);
    }

    public GuestSO GetCurrentGuest => currentGuest;

    private void SetGuestPersonality()
    {
        List<GuestPersonality> personalityList = new List<GuestPersonality>();

        foreach (GuestPersonality personality in Enum.GetValues(typeof(GuestPersonality)))
        {
            if (UtilClass.IsIncludeFlag(currentGuest.personalitys, personality))
            {
                personalityList.Add(personality);
            }
        }

        int randomIdx = UnityEngine.Random.Range(0, personalityList.Count);
        GuestPersonality = personalityList[randomIdx];
        BarIntervalIndex = (GuestPersonality == GuestPersonality.PATIENT) ? TimerInterval.YELLOW : TimerInterval.GREEN;
        guestTalk.SetEmotion(BarIntervalIndex);
    }

    private void AddMinuteTime(int value)
    {
        if (isDayTimeOver) return;

        todayDate = todayDate.AddMinutes(value);
        RefreshWindow();
        
        if(todayDate.Hour >= 21)
        {
            // 영업 종료!
            isDayTimeOver = true;
            timeOverText.gameObject.SetActive(true);
        }
    }

    private void RefreshWindow(bool immediately = false)
    {
        timeText.text = $"{todayDate:h:mm tt}";

        if (todayDate.Hour >= 19)
        {
            SetBackground(duskBG, immediately);
        }
        else if (todayDate.Hour >= 18)
        {
            SetBackground(sunsetBG, immediately);
        }
        else if (todayDate.Hour >= 14)
        {
            SetBackground(daytimeBG, immediately);
        }
        else if (todayDate.Hour >= 9)
        {
            SetBackground(morningBG, immediately);
        }
    }

    private void SetBackground(Sprite bg, bool immediately)
    {
        if(windowBGs[bgIndex].sprite != bg)
        {
            if (!immediately)
            {
                windowBGs[1].DOFade((bgIndex == 0) ? 1 : 0, 1);
            }
            else
            {
                windowBGs[1].color = new Color(1, 1, 1, (bgIndex == 0) ? 1 : 0);
            }
            bgIndex = (bgIndex + 1) % 2;
            windowBGs[bgIndex].sprite = bg;
        }
    }

    public void SetTimer(int timeSec)
    {
        isTimer = true;

        float timerScale = 1.0f;
        if (GuestPersonality == GuestPersonality.PATIENT) timerScale = 1.2f;
        if (GuestPersonality == GuestPersonality.FEISTY) timerScale = 0.8f;

        GivenTime = timeSec * timerScale;
        RemainTime = GivenTime;

        TimerBarInterval = GetTimerIntervalToPersonality();
        SetTimerInterval(TimerBarInterval);
        AppearTimerBar();
    }

    public void StopTimer()
    {
        isTimer = false;
        isHurry = false;
        RemainTime = GivenTime;
        RefreshTimeBarValue();
    }

    private Vector3 GetTimerIntervalToPersonality()
    {
        Vector3 interval = GuestPersonality switch
        {
            GuestPersonality.EASYGOING => new Vector3(0.1f, 0.25f, 0.4f),
            GuestPersonality.URGENT => new Vector3(0.2f, 0.4f, 0.6f),
            GuestPersonality.PATIENT => new Vector3(0.2f, 0.4f, 1),
            GuestPersonality.FEISTY => new Vector3(0.2f, 0.3f, 0.5f),
            GuestPersonality.TALKATIVE => new Vector3(0.2f, 0.3f, 0.5f),
            _ => new Vector3(0.1f, 0.25f, 0.4f),
        };
        return interval;
    }

    private TimerInterval GetTimerIntervalIndex()
    {
        float barValue = RemainTime / GivenTime;

        if (barValue <= TimerBarInterval.x)
        {
            // RED
            return TimerInterval.RED;
        }
        else if (barValue <= TimerBarInterval.y)
        {
            // ORANGE
            return TimerInterval.ORANGE;
        }
        else if (barValue <= TimerBarInterval.z)
        {
            // YELLOW
            return TimerInterval.YELLOW;
        }
        else
        {
            // GREEN
            return TimerInterval.GREEN;
        }
    }

    public void SetTimerInterval(Vector3 interval)
    {
        for (int i = 0; i < timeBarUis.Length; i++)
        {
            timeBarUis[i].SetBarInterval(interval);
        }
    }

    private void RefreshTimeBarValue()
    {
        for (int i = 0; i < timeBarUis.Length; i++)
        {
            timeBarUis[i].SetBarValue(RemainTime / GivenTime);
        }
    }

    public void AppearTimerBar()
    {
        for (int i = 0; i < timeBarUis.Length; i++)
        {
            timeBarUis[i].PlayAppearSeq();
        }
    }

    public void SetScroll(bool value, bool IsImmediately)
    {
        IsInCounter = value;

        if (value)
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(0, 0);           
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(0, 0), 1).SetEase(Ease.OutBounce);
            }
        }
        else
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(1920, 0);
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(1920, 0), 1).SetEase(Ease.OutBounce);
            }
        }
    }

    public void OrderClear()
    {
        RecipeSO[] currentRecipes = CookingManager.GetRecipes();
        int allPrice = 0;

        for (int i = 0; i < currentRecipes.Length; i++)
        {
            allPrice += currentRecipes[i].foodPrice;
        }

        switch(BarIntervalIndex)
        {
            case TimerInterval.RED:
                AddMoney(MoneyType.GOLD, (int)(allPrice * 0.75f));
                break;
            case TimerInterval.ORANGE:
                AddMoney(MoneyType.GOLD, allPrice);
                break;
            case TimerInterval.YELLOW:
                AddMoney(MoneyType.GOLD, allPrice, allPrice / 20);
                break;
            case TimerInterval.GREEN:
                AddMoney(MoneyType.GOLD, allPrice, allPrice / 10);
                break;
        }

        StopTimer();
        Dialog.ResetEvent();
        CookingManager.ClearRecipes();
        guestAnimator.SetTrigger("Disappear");
        Dialog.SetQuitMessage(currentGuest.quitTranslationIds);
        CookingManager.Counter.guestTalk.HideGuestTalk();
        AppearTimerBar();

        Global.Sound.Play("SFX/CookingScene/cashregister_open", Sound.Effect, UnityEngine.Random.Range(0.75f, 1.5f));
        currentGuest = null;
    }

    public void OrderFail()
    {
        StopTimer();
        Dialog.ResetEvent();
        CookingManager.ClearRecipes();
        guestAnimator.SetTrigger("Disappear");
        Dialog.SetQuitMessage(currentGuest.gameOverTranslationIds);
        guestTalk.AddBubbleMessageFromLastMessage();
        CookingManager.Counter.guestTalk.HideEmotion();
        AppearTimerBar();

        currentGuest = null;
    }

    public void JustDisappear()
    {
        guestAnimator.SetTrigger("Disappear");
        CookingManager.Counter.guestTalk.HideGuestTalk();
        currentGuest = null;
    }

    public void AddDish(IngredientSO ingredient)
    {
        CounterDishUI dish = Global.Pool.GetItem<CounterDishUI>();
        dish.Init(ingredient);
    }

    public void RemoveAllDishes()
    {
        for(int i =0;i<counterDishTrm.childCount;i++)
        {
            counterDishTrm.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetMoney(MoneyType moneyType, int value)
    {
        ValuableBoxUI[] valuableBoxes = FindObjectsOfType<ValuableBoxUI>();

        for (int i = 0; i < valuableBoxes.Length; i++)
        {
            if (valuableBoxes[i].moneyType == moneyType)
            {
                valuableBoxes[i].SetText(value);
            }
        }

        if (moneyType == MoneyType.GOLD)
        {
            gold = value;
        }
        else if (moneyType == MoneyType.STARCANDY)
        {
            starCandy = value;
        }
    }

    public void AddMoney(MoneyType moneyType, int value, int tip = 0)
    {
        ValuableBoxUI[] valuableBoxes = FindObjectsOfType<ValuableBoxUI>();

        int addTemp = moneyType == MoneyType.GOLD ? gold : starCandy;

        for (int i = 0; i < valuableBoxes.Length; i++)
        {
            if (valuableBoxes[i].moneyType == moneyType)
            {
                if (tip > 0)
                {
                    valuableBoxes[i].SetText(addTemp, addTemp + value, tip);
                }
                else
                {
                    valuableBoxes[i].SetText(addTemp, addTemp + value);
                }
            }
        }

        addTemp += value;
        addTemp += tip;

        if (moneyType == MoneyType.GOLD)
        {
            gold = addTemp;
        }
        else if (moneyType == MoneyType.STARCANDY)
        {
            starCandy = addTemp;
        }
    }
}
