using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("인벤토리")]
    [SerializeField] Image[] inventoryTabs;

    [Header("미니게임 인벤토리")]
    [SerializeField] Transform minigameListContentTrm;
    [SerializeField] Transform ingredientInventoryTrm;
    [SerializeField] GameObject minigameNotExistTab;

    [Header("버튼")]
    [SerializeField] Button minigameExitButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        minigameExitButton.onClick.AddListener(() =>
        {
            Global.UI.UIFade(canvasGroup, false);
            CookingManager.Global.CurrentUtensils = null;
            for (int i = 0; i < minigameListContentTrm.childCount; i++)
            {
                minigameListContentTrm.GetChild(i).gameObject.SetActive(false);
            }
        });
    }

    private void Start()
    {
        GameObject ingredientTabPrefab = Global.Resource.Load<GameObject>("UI/IngredientTab");
        GameObject ingredientInventoryPrefab = Global.Resource.Load<GameObject>("UI/IngredientInventory");

        Global.Pool.CreatePool<UI_IngredientTab>(ingredientTabPrefab, minigameListContentTrm, 3);
        Global.Pool.CreatePool<UI_IngredientInventory>(ingredientInventoryPrefab, ingredientInventoryTrm, 10);
    }

    public void ReceiveInfo(MinigameInfo[] info)
    {
        Global.UI.UIFade(canvasGroup, true);
        minigameNotExistTab.SetActive(info.Length == 0);


        // 인벤토리 불러오기
        PlayerInventoryTab[] playerInventoryTabs = CookingManager.Player.Inventory.GetInventory();
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            if (i < playerInventoryTabs.Length)
            {
                inventoryTabs[i].gameObject.SetActive(true);
                Image ingredientImg = inventoryTabs[i].transform.GetChild(0).GetComponent<Image>();

                if (playerInventoryTabs[i].ingredient != null)
                {
                    ingredientImg.gameObject.SetActive(true);
                    ingredientImg.sprite = playerInventoryTabs[i].ingredient.ingredientMiniSpr;
                }
                else
                {
                    ingredientImg.gameObject.SetActive(false);
                }
            }
            else
            {
                inventoryTabs[i].gameObject.SetActive(false);
            }
        }

        // 미니게임 재료 불러오기
        for (int i = 0; i < info.Length; i++)
        {
            UI_IngredientTab tab = Global.Pool.GetItem<UI_IngredientTab>();
            tab.InventoryClear(ingredientInventoryTrm);
            tab.InitName(TranslationManager.Instance.GetLangDialog(info[i].minigameNameTranslationId));

            for (int j = 0; j < info[i].ingredients.Length; j++)
            {
                UI_IngredientInventory inventory = Global.Pool.GetItem<UI_IngredientInventory>();
                inventory.transform.SetParent(tab.ingredientInventoryTrm);
                inventory.InitIngredient(info[i].ingredients[j]);
                inventory.SetFade(false);

                if (info[i].ingredients[j] == CookingManager.Global.CurrentUtensils.utensilsInventories[i].ingredients[j])
                {
                    inventory.SetFade(true); // 만약 인벤토리에 저장된 아이템이라면 열 때부터 잠금 해제.
                }
            }
        }
    }
}
