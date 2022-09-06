using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using DG.Tweening;

public class CounterHandler : MonoBehaviour
{
    public float GivenTime { get; set; } = 0;
    public float RemainTime { get; set; } = 0;
    public bool IsInCounter { get; set; } = false;

    private bool isTimer = false;

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
    [SerializeField] ValuableEffectUI valuableEffectPrefab;
    [SerializeField] Transform valuableEffectTrm;
    public Sprite[] moneyTypeSprs;

    private DateTime todayDate;
    private float minuteTimer = 0f;

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
            }
            else
            {
                RemainTime = 0;
                isTimer = false;
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
                //Init ÇØ¾ßÇÔ
                int random = UnityEngine.Random.Range(0, allGuests.Count);
                currentGuest = allGuests[random];
                Dialog.GuestInit(currentGuest);

                Dialog.ShowSpeechBubble(false);
                guestAnimator.SetTrigger("Appear");
                StartCoroutine(GuestDialogPlay());
            }
            yield return new WaitForSeconds(10);
        }
    }

    private IEnumerator GuestDialogPlay()
    {
        yield return new WaitForSeconds(2.5f);
        int random = UnityEngine.Random.Range(0, currentGuest.canPlayDialogIds.Length);
        AddDialog(currentGuest.canPlayDialogIds[random]);
    }

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
        if (value)
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(0, 0);
                IsInCounter = value;
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(0, 0), 1).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    IsInCounter = value;
                });
            }
        }
        else
        {
            if (IsImmediately)
            {
                scrollRect.offsetMin = new Vector2(1920, 0);
                IsInCounter = value;
            }
            else
            {
                DOTween.To(() => scrollRect.offsetMin, value => scrollRect.offsetMin = value, new Vector2(1920, 0), 1).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    IsInCounter = value;
                });
            }
        }
    }

    public void OrderClear()
    {
        StopTimer();
        Dialog.ResetEvent();
        CookingManager.ClearRecipes();
        guestAnimator.SetTrigger("Disappear");
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
}
