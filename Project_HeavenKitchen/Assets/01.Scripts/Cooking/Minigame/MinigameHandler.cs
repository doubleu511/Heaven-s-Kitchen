using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MinigameHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private bool isDuringMinigame = false;

    [Header("인벤토리")]
    [SerializeField] Image[] inventoryTabs;

    [Header("미니게임 인벤토리")]
    [SerializeField] Transform minigameListContentTrm;
    [SerializeField] Transform ingredientInventoryTrm;
    [SerializeField] GameObject minigameNotExistTab;
    List<UI_IngredientTab> currentIngredientTabs = new List<UI_IngredientTab>();

    [Header("미니게임 텍스트")]
    [SerializeField] TextMeshProUGUI minigameStartText;
    [SerializeField] TextMeshProUGUI cookingToolText;

    [Header("프로세스 바")]
    [SerializeField] CanvasGroup progressBar;
    [SerializeField] Transform progressValue;
    [SerializeField] TextMeshProUGUI progressText;
    
    [Header("버튼")]
    [SerializeField] Button minigameExitButton;

    [Header("주방기구들")]
    [SerializeField] Transform utensilsTrm;

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

        // 조리기구 이름 설정
        cookingToolText.text = TranslationManager.Instance.GetLangDialog(utensilsInfo.cookingUtensilsTranslationId);

        // 인벤토리 불러오기
        LoadInventory();

        // 미니게임 재료 불러오기
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
                    inventory.SetFade(true); // 만약 인벤토리에 저장된 아이템이라면 열 때부터 잠금 해제.
                }
            }
        }
    }

    private void CallMinigameStartBtnOnClicked(CookingUtensilsSO utensilsInfo, MinigameInfo minigame, int minigameIndex)
    {
        if (isDuringMinigame) return;
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
            isDuringMinigame = true;
            InventorySync();
            currentIngredientTabs[minigameIndex].InventoryClean();

            // 밑에처럼 하지말고 미니게임 프리팹을 생성한뒤 그걸 조리도구 자식으로 붙힌다.
            // 그리고 미니게임 객체에 현재 MinigameStarter 객체(부모)가 누구인지 대입한다.

            GameObject minigamePrefab = Global.Resource.Load<GameObject>($"Minigames/{minigame.minigameType}");
            Transform utensils = utensilsTrm.Find($"{utensilsInfo.cookingUtensilsType}");

            if(utensils != null)
            {
                if (minigamePrefab != null)
                {
                    Minigame game = Instantiate(minigamePrefab, utensils).GetComponent<Minigame>();
                    game.minigameParent = CookingManager.Global.CurrentUtensils;
                    game.StartMinigame(minigame);

                    curPlayingMinigames.Add(game);
                }
                else
                {
                    Debug.Log($"미니게임이 존재하지 않음 : {minigame.minigameType}");
                }
            }
            else
            {
                Debug.Log($"주방도구가 존재하지 않음 : {utensilsInfo.cookingUtensilsType}");
            }
        }
    }

    public void MinigameEnd(Minigame game)
    {
        curPlayingMinigames.Remove(game);
        LoadInventory();
        isDuringMinigame = false;
    }

    public void LoadInventory()
    {
        PlayerInventoryTab[] playerInventoryTabs = CookingManager.Player.Inventory.GetInventory();
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            if (i < playerInventoryTabs.Length)
            {
                inventoryTabs[i].gameObject.SetActive(true);
                DragableUI ingredientImg = inventoryTabs[i].transform.GetChild(0).GetComponent<DragableUI>();

                ingredientImg.SetIngredient(playerInventoryTabs[i].ingredient);
            }
            else
            {
                inventoryTabs[i].gameObject.SetActive(false);
            }
        }
    }

    private void InventorySync()
    {
        // 요리 UI 인벤토리 재료를 게임 인벤토리로
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

        float valuePercentage = value / 100f;
        targetProcessValue = valuePercentage;
        if (animation) processValue = valuePercentage;

        progressText.text = $"{value}%";
    }

    public void HideProgress()
    {
        Global.UI.UIFade(progressBar, Define.UIFadeType.OUT, 0.5f, false);
    }
}
