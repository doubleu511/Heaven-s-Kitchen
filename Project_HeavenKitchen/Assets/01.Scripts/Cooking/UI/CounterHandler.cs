using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Deco")]
    [SerializeField] CounterDishUI counterDishPrefab;
    [SerializeField] Transform counterDishTrm;


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

        // Test
        SetScroll(true, true);
        StartCoroutine(GuestEncounter());
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
                int random = Random.Range(0, allGuests.Count);
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
        yield return new WaitForSeconds(1.5f);
        int random = Random.Range(0, currentGuest.canPlayDialogIds.Length);
        AddDialog(currentGuest.canPlayDialogIds[random]);
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
