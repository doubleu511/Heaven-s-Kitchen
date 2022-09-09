using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class CounterHandler : MonoBehaviour
{
    public float GivenTime { get; set; } = 0;
    public float RemainTime { get; set; } = 0;
    public bool IsInCounter { get; set; } = false;

    private bool isTimer = false;
    private bool isHurry = false;

    private RectTransform scrollRect;
    private GuestSO currentGuest;

    [SerializeField] CookingDialogPanel Dialog;
    [SerializeField] Button goToCookingBtn;
    [SerializeField] CounterTimeBarUI[] timeBarUis;
    [SerializeField] Animator guestAnimator;
    [SerializeField] List<GuestSO> allGuests = new List<GuestSO>(); // Temp

    [Header("WindowUI")]
    [SerializeField] Image cuttonImg;
    [SerializeField] Image[] windowBGs;
    private int bgIndex = 0;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Sprite morningBG;
    [SerializeField] Sprite daytimeBG;
    [SerializeField] Sprite sunsetBG;
    [SerializeField] Sprite duskBG;

    [Header("Deco")]
    [SerializeField] CounterDishUI counterDishPrefab;
    [SerializeField] Transform counterDishTrm;
    public GuestTalkInKitchenUI guestTalk;

    private DateTime todayDate;
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

        SetMoney(Define.MoneyType.GOLD, 0);
        SetMoney(Define.MoneyType.STARCANDY, 0);
        todayDate = new DateTime(2023, 1, 1, 9, 0, 0);
        RefreshWindow(true);
    }

    private void Update()
    {
        if(isTimer)
        {
            if(RemainTime > 0)
            {
                RemainTime -= Time.deltaTime;
                RefreshTimeBarValue();

                if (!isHurry)
                {
                    if (RemainTime / GivenTime < 0.25f)
                    {
                        isHurry = true;

                        for (int i = 0; i < currentGuest.hurryUpTranlationIds.Length; i++)
                        {
                            CookingDialogInfo info = new CookingDialogInfo(currentGuest.hurryUpTranlationIds[i], 0, 0, (int)Define.TextAnimationType.SHAKE, "");

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
        if(minuteTimer > 2)
        {
            minuteTimer -= 2;
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
            if(guestAnimator.GetCurrentAnimatorStateInfo(0).IsName("Guest_None"))
            {
                //Init 해야함
                int random = UnityEngine.Random.Range(0, allGuests.Count);
                currentGuest = allGuests[random];
                Dialog.GuestInit(currentGuest);

                Dialog.ShowSpeechBubble(false);
                CookingManager.Counter.guestTalk.ShowGuestTalk(currentGuest.guestPortrait);
                if(!IsInCounter)
                {
                    CookingDialogInfo info = new CookingDialogInfo(currentGuest.heyTranslationId);
                    info.text_animation_type = (int)Define.TextAnimationType.SHAKE;

                    CookingManager.Counter.guestTalk.AddBubbleMessage(info);
                }

                guestAnimator.SetTrigger("Appear");
                StartCoroutine(GuestDialogPlay());
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

    private void AddMinuteTime(int value)
    {
        todayDate = todayDate.AddMinutes(value);
        RefreshWindow();
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
        GivenTime = timeSec;
        RemainTime = GivenTime;
    }

    public void StopTimer()
    {
        isTimer = false;
        isHurry = false;
    }

    private void RefreshTimeBarValue()
    {
        for (int i = 0; i < timeBarUis.Length; i++)
        {
            timeBarUis[i].SetBarValue(RemainTime / GivenTime);
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

        AddMoney(Define.MoneyType.GOLD, allPrice);
        StopTimer();
        Dialog.ResetEvent();
        CookingManager.ClearRecipes();
        guestAnimator.SetTrigger("Disappear");
        CookingManager.Counter.guestTalk.HideGuestTalk();
    }

    public void OrderFail()
    {
        StopTimer();
        Dialog.ResetEvent();
        CookingManager.ClearRecipes();
        guestAnimator.SetTrigger("Disappear");
        Dialog.SetQuitMessage(currentGuest.gameOverTranslationIds);
        guestTalk.AddBubbleMessageFromLastMessage();
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

    public void SetMoney(Define.MoneyType moneyType, int value)
    {
        ValuableBoxUI[] valuableBoxes = FindObjectsOfType<ValuableBoxUI>();

        for (int i = 0; i < valuableBoxes.Length; i++)
        {
            if (valuableBoxes[i].moneyType == moneyType)
            {
                valuableBoxes[i].SetText(value);
            }
        }

        if (moneyType == Define.MoneyType.GOLD)
        {
            gold = value;
        }
        else if (moneyType == Define.MoneyType.STARCANDY)
        {
            starCandy = value;
        }
    }

    public void AddMoney(Define.MoneyType moneyType, int value)
    {
        ValuableBoxUI[] valuableBoxes = FindObjectsOfType<ValuableBoxUI>();

        int addTemp = moneyType == Define.MoneyType.GOLD ? gold : starCandy;

        for (int i = 0; i < valuableBoxes.Length; i++)
        {
            if (valuableBoxes[i].moneyType == moneyType)
            {
                valuableBoxes[i].SetText(addTemp, addTemp + value);
            }
        }

        addTemp += value;

        if (moneyType == Define.MoneyType.GOLD)
        {
            gold = addTemp;
        }
        else if (moneyType == Define.MoneyType.STARCANDY)
        {
            starCandy = addTemp;
        }
    }
}
