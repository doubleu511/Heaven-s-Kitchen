using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInventoryTab
{
    public SpriteRenderer basketItem;
    public Image ingredientImgTab;

    [HideInInspector] public PlayerInventoryDragableUI ingredientInventoryUI;
    private ParticleSystem inventorySmokeParticle;
    private Image inventoryDishImg;

    public IngredientSO ingredient;

    private bool _isDish = false;
    public bool isDish
    {
        get
        {
            return _isDish;
        }
        set
        {
            inventoryDishImg.gameObject.SetActive(value);
            _isDish = value;
        }
    }

    public void SetIngredient(IngredientSO so)
    {
        ingredient = so;

        if (ingredient != null)
        {
            basketItem.sprite = ingredient.ingredientMiniSpr;
            ingredientInventoryUI.SetIngredient(ingredient);

            if (ingredient.isHot) inventorySmokeParticle.Play();
            else inventorySmokeParticle.Stop();
        }
        else
        {
            basketItem.sprite = null;
            ingredientInventoryUI.SetIngredient(null);
            inventorySmokeParticle.Stop();
        }
    }

    public void Init()
    {
        inventorySmokeParticle = basketItem.transform.Find("SmokeParticle").GetComponent<ParticleSystem>();
        ingredientInventoryUI = ingredientImgTab.transform.Find("IngredientImg").GetComponent<PlayerInventoryDragableUI>();
        inventoryDishImg = ingredientImgTab.transform.Find("Dish").GetComponent<Image>();

        isDish = false;
    }
}

public class PlayerInventory : MonoBehaviour
{
    private MemoHandler memoHandler;
    public PlayerInventoryTab[] inventoryTabs;

    [SerializeField]
    [Range(3, 5)]
    int inventoryCount = 3;

    private void Awake()
    {
        memoHandler = FindObjectOfType<MemoHandler>();
    }

    private void Start()
    {
        InventoryInit(inventoryCount);
    }

    public void InventoryInit(int count)
    {
        for (int i = 0; i < inventoryTabs.Length; i++)
        {
            inventoryTabs[i].Init();
            inventoryTabs[i].SetIngredient(null);
            inventoryTabs[i].basketItem.gameObject.SetActive        (i < count);

            Transform lockTrm = inventoryTabs[i].ingredientImgTab.transform.Find("Lock");
            lockTrm.gameObject.SetActive(i >= count);
        }
    }

    public bool InventoryAdd(IngredientSO ingredient)
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            if(inventoryTabs[i].ingredient == null)
            {
                inventoryTabs[i].SetIngredient(ingredient);
                memoHandler.SearchNavIngredient();

                return true;
            }
        }

        return false;
    }

    public bool InventoryAdd(IngredientSO ingredient, out int addIndex)
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            if (inventoryTabs[i].ingredient == null)
            {
                inventoryTabs[i].SetIngredient(ingredient);
                addIndex = i;
                memoHandler.SearchNavIngredient();

                return true;
            }
        }

        addIndex = -1;
        return false;
    }

    public void InventoryRemoveAt(int index)
    {
        inventoryTabs[index].SetIngredient(null);

        for (int i = index; i < inventoryCount; i++)
        {
            if(i < inventoryCount - 1)
            {
                inventoryTabs[i].SetIngredient(inventoryTabs[i + 1].ingredient);
            }
            else
            {
                if (inventoryTabs[index].ingredient != null)
                {
                    inventoryTabs[i].SetIngredient(null);
                }
            }
        }
    }

    public bool IsItemExist(IngredientSO ingredient)
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            if (inventoryTabs[i].ingredient == ingredient)
            {
                return true;
            }
        }

        return false;
    }

    public IngredientSO GetIngredient(int index)
    {
        return inventoryTabs[index].ingredient;
    }

    public PlayerInventoryTab[] GetInventory()
    {
        List<PlayerInventoryTab> playerInventoryTabs = new List<PlayerInventoryTab>();

        for (int i = 0; i < inventoryCount; i++)
        {
            playerInventoryTabs.Add(inventoryTabs[i]);
        }

        return playerInventoryTabs.ToArray();
    }
}
