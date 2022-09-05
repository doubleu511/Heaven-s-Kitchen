using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CounterHandler : MonoBehaviour
{
    public float GivenTime { get; set; } = 0;
    public float RemainTime { get; set; } = 0;

    private bool isTimer = false;

    [SerializeField] CookingDialogPanel Dialog;
    [SerializeField] Button goToCookingBtn;
    [SerializeField] CounterTimeBarUI[] timeBarUis;

    [Header("Deco")]
    [SerializeField] CounterDishUI counterDishPrefab;
    [SerializeField] Transform counterDishTrm;

    private RectTransform scrollRect;

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
        Dialog.StartDialog(TranslationManager.Instance.CookingDialog.GetDialog(1001));
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
        if(value)
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
        StopTimer();
        Dialog.ResetEvent();
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
