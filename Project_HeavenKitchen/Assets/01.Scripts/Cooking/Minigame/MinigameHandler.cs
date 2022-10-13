using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MinigameHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("배경")]
    [SerializeField] Image minigameBackground;

    [Header("인벤토리")]
    [SerializeField] Image[] inventoryTabs;

    [Header("미니게임 인벤토리")]
    [SerializeField] Transform minigameListContentTrm;
    [SerializeField] Transform ingredientInventoryTrm;
    [SerializeField] GameObject minigameNotExistTab;
    [SerializeField] GameObject minigameAlreadyPlayingTab;
    List<IngredientTabUI> currentIngredientTabs = new List<IngredientTabUI>();

    [Header("미니게임 텍스트")]
    [SerializeField] TextMeshProUGUI minigameStartText;
    [SerializeField] TextMeshProUGUI cookingToolText;

    [Header("프로세스 바")]
    [SerializeField] CanvasGroup progressBar;
    [SerializeField] Transform progressValue;
    [SerializeField] TextMeshProUGUI progressText;

    [Header("추가 디테일")]
    [SerializeField] Image ingredientAddAnimationDetail;
    
    [Header("버튼")]
    [SerializeField] Button minigameExitButton;
    [SerializeField] Transform arrowButtonTrm;
    private Button[] arrowButtons;

    [Header("주방기구들")]
    [SerializeField] Transform utensilsTrm;
    private Transform curUtensilsCanvasTrm;

    private float processValue = 0f;
    private float targetProcessValue = 0f;
    private List<Minigame> curPlayingMinigames = new List<Minigame>();
    private MemoHandler memoHandler;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        memoHandler = FindObjectOfType<MemoHandler>();

        minigameExitButton.onClick.AddListener(CloseWindow);
    }

    private void Start()
    {
        GameObject ingredientTabPrefab = Global.Resource.Load<GameObject>("UI/IngredientTab");
        GameObject ingredientInventoryPrefab = Global.Resource.Load<GameObject>("UI/IngredientInventory");

        Global.Pool.CreatePool<IngredientTabUI>(ingredientTabPrefab, minigameListContentTrm, 3);
        Global.Pool.CreatePool<IngredientInventoryUI>(ingredientInventoryPrefab, ingredientInventoryTrm, 10);

        arrowButtons = arrowButtonTrm.GetComponentsInChildren<Button>(true);
    }

    private void Update()
    {
        processValue = Mathf.Lerp(progressValue.transform.localScale.x, targetProcessValue, Time.deltaTime * 5);
        progressValue.transform.localScale = new Vector2(processValue, 1);

        for(int i =0; i<curPlayingMinigames.Count;i++)
        {
            curPlayingMinigames[i].minigameParent.SetProcessBar(curPlayingMinigames[i].GetProcressValue());
        }

        if(CookingManager.Global.CurrentUtensils != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                CloseWindow();
            }
        }
    }

    public void ReceiveInfo(CookingUtensilsSO utensilsInfo, MinigameInfo[] info)
    {
        Global.UI.UIFade(canvasGroup, true);
        minigameNotExistTab.SetActive(info.Length == 0);
        minigameAlreadyPlayingTab.SetActive(false);
        for (int i = 0; i < curPlayingMinigames.Count; i++)
        {
            curPlayingMinigames[i].OnWindowOpen();

            if(curPlayingMinigames[i].minigameParent == CookingManager.Global.CurrentUtensils) // 이미 미니게임이 진행중인 조리기구인가?
            {
                minigameAlreadyPlayingTab.SetActive(true);
            }
        }

        // 조리기구 이름 설정
        cookingToolText.text = TranslationManager.Instance.GetLangDialog(utensilsInfo.cookingUtensilsTranslationId);

        // 조리기구 배경 설정
        minigameBackground.sprite = utensilsInfo.minigameBGSpr.skinIndex[0]; // TO DO : 스킨 시스템

        // 인벤토리 불러오기
        LoadInventory();

        // 조리기구를 일단 모두 끄고
        for (int i = 0; i < utensilsTrm.childCount; i++)
        {
            CanvasGroup utensilsCanvas = utensilsTrm.GetChild(i).GetComponent<CanvasGroup>();
            Global.UI.UIFade(utensilsCanvas, false);
        }

        // 화살버튼 UI도 모두 끄고
        for (int i = 0; i < arrowButtons.Length; i++)
        {
            arrowButtons[i].gameObject.SetActive(false);
        }

        // 현재 조리기구 캔버스 찾은 후 켜주기
        curUtensilsCanvasTrm = utensilsTrm.Find($"{utensilsInfo.cookingUtensilsType}");
        if(curUtensilsCanvasTrm == null)
        {
            Debug.Log($"주방도구가 존재하지 않음 : {utensilsInfo.cookingUtensilsType}");
            return;
        }
        CanvasGroup curUtensilsCanvas = curUtensilsCanvasTrm.GetComponent<CanvasGroup>();
        Global.UI.UIFade(curUtensilsCanvas, true);

        // 미니게임 재료 불러오기
        currentIngredientTabs.Clear();
        for (int i = 0; i < info.Length; i++)
        {
            IngredientTabUI tab = Global.Pool.GetItem<IngredientTabUI>();
            tab.transform.SetSiblingIndex(i);
            tab.InventoryClear(ingredientInventoryTrm);
            tab.InitName(TranslationManager.Instance.GetLangDialog(info[i].minigameNameTranslationId));

            int index = i;
            tab.SetStartAction(() => CallMinigameStartBtnOnClicked(utensilsInfo, info[index], index));
            currentIngredientTabs.Add(tab);

            for (int j = 0; j < info[i].ingredients.Length; j++)
            {
                IngredientInventoryUI inventory = Global.Pool.GetItem<IngredientInventoryUI>();
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
            minigameAlreadyPlayingTab.SetActive(true);

            // 미니게임 프리팹을 생성한뒤 그걸 조리도구 자식으로 붙힌다.
            // 그리고 미니게임 객체에 현재 MinigameStarter 객체(부모)가 누구인지 대입한다.

            GameObject minigamePrefab = Global.Resource.Load<GameObject>($"Minigames/{minigame.minigameType}");

            if (minigamePrefab != null)
            {
                Minigame game = Instantiate(minigamePrefab, curUtensilsCanvasTrm).GetComponent<Minigame>();
                UtensilsSort();
                game.curMinigameIndex = minigameIndex;
                game.minigameParent = CookingManager.Global.CurrentUtensils;
                game.StartMinigame(minigame);

                curPlayingMinigames.Add(game);
            }
            else
            {
                Debug.Log($"미니게임이 존재하지 않음 : {minigame.minigameType}");
            }
        }
    }

    public void MinigameEnd(Minigame game)
    {
        minigameAlreadyPlayingTab.SetActive(false);
        currentIngredientTabs[game.curMinigameIndex].InventoryClean();
        game.minigameParent.HideProcessBar();
        curPlayingMinigames.Remove(game);
        LoadInventory();
    }

    public void CloseWindow()
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
            Transform dishTrm = inventoryTabs[i].transform.Find("Dish");
            DragableUI ingredientImg = inventoryTabs[i].GetComponentInChildren<DragableUI>();

            if (i < playerInventoryTabs.Length)
            {
                lockTrm.gameObject.SetActive(false);
                ingredientImg.SetIngredient(playerInventoryTabs[i].ingredient);
                ingredientImg.SetTabInfo(playerInventoryTabs[i].tabinfo);
            }
            else
            {
                lockTrm.gameObject.SetActive(true);
                ingredientImg.SetIngredient(null);
                ingredientImg.SetTabInfo(null);
            }

            dishTrm.gameObject.SetActive(ingredientImg.myInfo.isDish);
        }
    }

    public void InventorySync()
    {
        // 요리 UI 인벤토리 재료를 게임 인벤토리로
        List<IngredientSO> tempInventory = new List<IngredientSO>();
        List<TabInfo> tempInventoryInfo = new List<TabInfo>();
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            DragableUI ingredientImg = inventoryTabs[i].transform.GetComponentInChildren<DragableUI>();

            if (ingredientImg.myIngredient != null)
            {
                tempInventory.Add(ingredientImg.myIngredient);
                tempInventoryInfo.Add(ingredientImg.myInfo);
            }
        }

        for (int i = 0; i < CookingManager.Player.Inventory.inventoryTabs.Length; i++)
        {
            CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(null);
            CookingManager.Player.Inventory.inventoryTabs[i].InitInfo();

            if (i < tempInventory.Count)
            {
                CookingManager.Player.Inventory.inventoryTabs[i].SetIngredient(tempInventory[i]);
                CookingManager.Player.Inventory.inventoryTabs[i].SetInfo(tempInventoryInfo[i]);
            }
        }

        memoHandler.SearchNavIngredient();
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
        // TO DO : 이거 프리팹으로 해서 여러개 적용 가능하게!
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
