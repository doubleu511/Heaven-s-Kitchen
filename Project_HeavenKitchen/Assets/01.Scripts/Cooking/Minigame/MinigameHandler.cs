using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHandler : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("�κ��丮")]
    [SerializeField] Image[] inventoryTabs;

    [Header("�̴ϰ��� �κ��丮")]
    [SerializeField] Transform minigameListContentTrm;
    [SerializeField] Transform ingredientInventoryTrm;
    [SerializeField] GameObject minigameNotExistTab;

    [Header("��ư")]
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

    public void ReceiveInfo(MinigameInfo[] info)
    {
        Global.UI.UIFade(canvasGroup, true);
        minigameNotExistTab.SetActive(info.Length == 0);


        // �κ��丮 �ҷ�����
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

        // �̴ϰ��� ��� �ҷ�����
        for (int i = 0; i < info.Length; i++)
        {
            UI_IngredientTab tab = Global.Pool.GetItem<UI_IngredientTab>();
            tab.InventoryClear(ingredientInventoryTrm);
            tab.InitName(TranslationManager.Instance.GetLangDialog(info[i].minigameNameTranslationId));

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
}
