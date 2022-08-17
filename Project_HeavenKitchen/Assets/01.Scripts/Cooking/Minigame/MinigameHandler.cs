using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("�κ��丮")]
    [SerializeField] Image[] inventoryTabs;

    [Header("�̴ϰ��� �κ��丮")]
    [SerializeField] Transform minigameListContentTrm;
    [SerializeField] Transform ingredientInventoryTrm;
    [SerializeField] GameObject minigameNotExistTab;
    List<UI_IngredientTab> currentIngredientTabs = new List<UI_IngredientTab>();

    [Header("�̴ϰ��� �ؽ�Ʈ")]
    [SerializeField] TextMeshProUGUI minigameStartText;
    [SerializeField] TextMeshProUGUI cookingToolText;

    [Header("���μ��� ��")]
    [SerializeField] CanvasGroup progressBar;
    [SerializeField] Transform progressValue;
    [SerializeField] TextMeshProUGUI progressText;

    [Header("�߰� ������")]
    [SerializeField] Image ingredientAddAnimationDetail;
    
    [Header("��ư")]
    [SerializeField] Button minigameExitButton;

    [Header("�ֹ�ⱸ��")]
    [SerializeField] Transform utensilsTrm;
    private Transform curUtensilsCanvasTrm;

    private float processValue = 0f;
    private float targetProcessValue = 0f;
    private List<Minigame> curPlayingMinigames = new List<Minigame>();

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        minigameExitButton.onClick.AddListener(() =>
        {
            Global.UI.UIFade(canvasGroup, false);

            for (int i = 0; i < curPlayingMinigames.Count; i++)
            {
                curPlayingMinigames[i].OnWindowClose();
            }
            CookingManager.Global.CurrentUtensils = null;
            curUtensilsCanvasTrm = null;
            for (int i = 0; i < minigameListContentTrm.childCount; i++)
            {
                minigameListContentTrm.GetChild(i).gameObject.SetActive(false);
            }
            InventorySync();
        });
    }

    private void Start()
    {
        GameObject ingredientTabPrefab = Global.Resource.Load<GameObject>("UI/IngredientTab");
        GameObject ingredientInventoryPrefab = Global.Resource.Load<GameObject>("UI/IngredientInventory");

        Global.Pool.CreatePool<UI_IngredientTab>(ingredientTabPrefab, minigameListContentTrm, 3);
        Global.Pool.CreatePool<UI_IngredientInventory>(ingredientInventoryPrefab, ingredientInventoryTrm, 10);
    }

    private void Update()
    {
        processValue = Mathf.Lerp(progressValue.transform.localScale.x, targetProcessValue, Time.deltaTime * 5);
        progressValue.transform.localScale = new Vector2(processValue, 1);
    }

    public void ReceiveInfo(CookingUtensilsSO utensilsInfo, MinigameInfo[] info)
    {
        Global.UI.UIFade(canvasGroup, true);
        minigameNotExistTab.SetActive(info.Length == 0);
        for (int i = 0; i < curPlayingMinigames.Count; i++)
        {
            curPlayingMinigames[i].OnWindowOpen();
        }

        // �����ⱸ �̸� ����
        cookingToolText.text = TranslationManager.Instance.GetLangDialog(utensilsInfo.cookingUtensilsTranslationId);

        // �κ��丮 �ҷ�����
        LoadInventory();

        // �����ⱸ�� �ϴ� ��� ����
        for (int i = 0; i < utensilsTrm.childCount; i++)
        {
            CanvasGroup utensilsCanvas = utensilsTrm.GetChild(i).GetComponent<CanvasGroup>();
            Global.UI.UIFade(utensilsCanvas, false);
        }

        // ���� �����ⱸ ĵ���� ã�� �� ���ֱ�
        curUtensilsCanvasTrm = utensilsTrm.Find($"{utensilsInfo.cookingUtensilsType}");
        if(curUtensilsCanvasTrm == null)
        {
            Debug.Log($"�ֹ浵���� �������� ���� : {utensilsInfo.cookingUtensilsType}");
            return;
        }
        CanvasGroup curUtensilsCanvas = curUtensilsCanvasTrm.GetComponent<CanvasGroup>();
        Global.UI.UIFade(curUtensilsCanvas, true);

        // �̴ϰ��� ��� �ҷ�����
        currentIngredientTabs.Clear();
        for (int i = 0; i < info.Length; i++)
        {
            UI_IngredientTab tab = Global.Pool.GetItem<UI_IngredientTab>();
            tab.InventoryClear(ingredientInventoryTrm);
            tab.InitName(TranslationManager.Instance.GetLangDialog(info[i].minigameNameTranslationId));

            int index = i;
            tab.SetStartAction(() => CallMinigameStartBtnOnClicked(utensilsInfo, info[index], index));
            currentIngredientTabs.Add(tab);

            for (int j = 0; j < info[i].ingredients.Length; j++)
            {
                UI_IngredientInventory inventory = Global.Pool.GetItem<UI_IngredientInventory>();
                inventory.transform.SetParent(tab.ingredientInventoryTrm);
                inventory.InitIngredient(info[i].ingredients[j], i, j);

                inventory.SetFade(CookingManager.Global.CurrentUtensils.utensilsInventories[i].ingredients[j] != null);

                if (info[i].ingredients[j] == CookingManager.Global.CurrentUtensils.utensilsInventories[i].ingredients[j])
                {
                    inventory.SetFade(true); // ���� �κ��丮�� ����� �������̶�� �� ������ ��� ����.
                }
            }
        }
    }

    private void CallMinigameStartBtnOnClicked(CookingUtensilsSO utensilsInfo, MinigameInfo minigame, int minigameIndex)
    {
        if (minigame.ingredients.Length != CookingManager.Global.CurrentUtensils.utensilsInventories[minigameIndex].ingredients.Length)
            return;

        bool isStartable = true;

        for (int i = 0; i < minigame.ingredients.Length; i++)
        {
            if(minigame.ingredients[i] != CookingManager.Global.CurrentUtensils.utensilsInventories[minigameIndex].ingredients[i])
            {
                isStartable = false;
            }
        }

        if (isStartable)
        {
            InventorySync();
            currentIngredientTabs[minigameIndex].InventoryClean();

            // �̴ϰ��� �������� �����ѵ� �װ� �������� �ڽ����� ������.
            // �׸��� �̴ϰ��� ��ü�� ���� MinigameStarter ��ü(�θ�)�� �������� �����Ѵ�.

            GameObject minigamePrefab = Global.Resource.Load<GameObject>($"Minigames/{minigame.minigameType}");

            if (minigamePrefab != null)
            {
                Minigame game = Instantiate(minigamePrefab, curUtensilsCanvasTrm).GetComponent<Minigame>();
                UtensilsSort();
                game.minigameParent = CookingManager.Global.CurrentUtensils;
                game.StartMinigame(minigame);

                curPlayingMinigames.Add(game);
            }
            else
            {
                Debug.Log($"�̴ϰ����� �������� ���� : {minigame.minigameType}");
            }
        }
    }

    public void MinigameEnd(Minigame game)
    {
        curPlayingMinigames.Remove(game);
        LoadInventory();
    }

    private void UtensilsSort()
    {
        Transform front = curUtensilsCanvasTrm.Find("FRONT");
        if(front)
        {
            front.SetAsLastSibling();
        }
    }

    public void LoadInventory()
    {
        PlayerInventoryTab[] playerInventoryTabs = CookingManager.Player.Inventory.GetInventory();
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            Transform lockTrm = inventoryTabs[i].transform.Find("Lock");
            DragableUI ingredientImg = inventoryTabs[i].transform.GetChild(0).GetComponent<DragableUI>();

            if (i < playerInventoryTabs.Length)
            {
                lockTrm.gameObject.SetActive(false);
                ingredientImg.SetIngredient(playerInventoryTabs[i].ingredient);
            }
            else
            {
                lockTrm.gameObject.SetActive(true);
                ingredientImg.SetIngredient(null);
            }
        }
    }

    private void InventorySync()
    {
        // �丮 UI �κ��丮 ��Ḧ ���� �κ��丮��
        List<IngredientSO> tempInventory = new List<IngredientSO>();
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            DragableUI ingredientImg = inventoryTabs[i].transform.GetChild(0).GetComponent<DragableUI>();

            if (ingredientImg.myIngredient != null)
            {
                tempInventory.Add(ingredientImg.myIngredient);
            }
        }

        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(null);

            if (i < tempInventory.Count)
            {
                CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(tempInventory[i]);
            }
        }
    }

    public void ShowStartText(string text)
    {
        minigameStartText.text = text;
        minigameStartText.transform.DOKill();
        minigameStartText.transform.localScale = Vector2.zero;

        minigameStartText.transform.DOScale(Vector2.one, 0.5f).SetLoops(2, LoopType.Yoyo);
    }

    public void ShowProgress(bool init)
    {
        Global.UI.UIFade(progressBar, true);

        if(init)
        {
            processValue = 0f;
            targetProcessValue = 0f;
            progressValue.transform.localScale = new Vector2(0, 1);
            progressText.text = "0%";
        }
    }

    public void ShowProgress(int value, bool animation = true)
    {
        Global.UI.UIFade(progressBar, true);
        value = Mathf.Clamp(value, 0, 100);

        float valuePercentage = value / 100f;
        targetProcessValue = valuePercentage;
        if (!animation)
        {
            processValue = valuePercentage;
            progressValue.transform.localScale = new Vector2(processValue, 1);
        }

        progressText.text = $"{value}%";
    }

    public void HideProgress()
    {
        Global.UI.UIFade(progressBar, Define.UIFadeType.OUT, 0.5f, false);
    }

    public void IngredientAddAnimation(IngredientSO ingredient, Vector2 startPos, Vector2 startSize, int inventoryIndex)
    {
        // TO DO : �̰� ���������� �ؼ� ������ ���� �����ϰ�!
        ingredientAddAnimationDetail.sprite = ingredient.ingredientMiniSpr;

        ingredientAddAnimationDetail.transform.DOKill();
        ingredientAddAnimationDetail.DOKill();
        ingredientAddAnimationDetail.transform.position = startPos;
        ingredientAddAnimationDetail.rectTransform.sizeDelta = startSize;
        ingredientAddAnimationDetail.color = Color.white;

        Vector2 endPos = inventoryTabs[inventoryIndex].transform.position;
        ingredientAddAnimationDetail.transform.DOMove(endPos, 0.75f).SetEase(Ease.InCubic);
        ingredientAddAnimationDetail.rectTransform.DOSizeDelta(Vector2.one * 100, 0.75f).SetEase(Ease.InCubic);
        ingredientAddAnimationDetail.DOFade(0, 0.75f).SetEase(Ease.InCubic);
    }
}
