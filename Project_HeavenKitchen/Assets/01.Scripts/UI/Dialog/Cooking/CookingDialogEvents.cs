using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingDialogEvents : MonoBehaviour
{
    [Header("SetID")]
    [SerializeField] CounterIngredientInventoryUI counterIngredientInventoryPrefab;
    [SerializeField] CanvasGroup ingredientPanelTrm;

    [Header("Choice")]
    [SerializeField] CounterSelectButtonUI counterSelectButtonPrefab;
    [SerializeField] CanvasGroup choicePanelTrm;

    [Header("Timer")]
    [SerializeField] CanvasGroup timerGroup;

    private List<CounterIngredientInventoryUI> counterIngredientInventories = new List<CounterIngredientInventoryUI>();
    private Action onTextEndAction;

    private void Start()
    {
        Global.Pool.CreatePool<CounterIngredientInventoryUI>(counterIngredientInventoryPrefab.gameObject, ingredientPanelTrm.transform, 3);
        Global.Pool.CreatePool<CounterSelectButtonUI>(counterSelectButtonPrefab.gameObject, choicePanelTrm.transform, 3);
    }

    public void ThrowEvent(string _eventMethod)
    {
        // 이곳에 _eventMethod를 해석하는 코드 작성
        string[] methods = _eventMethod.Split('\n');
        if (methods.Length > 1)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                ThrowEvent(methods[i]);
            }

            return;
        }

        string[] methodParameters = _eventMethod.Split(' ');

        if(methodParameters.Length > 0)
        {
            switch(methodParameters[0])
            {
                case "CHOOSE":
                    ExtractCHOOSEParameters(methodParameters[1], methodParameters[2]);
                    break;
                case "SETRANDOM":
                    ExtractSETRANDOMParameters(methodParameters);
                    break;
                case "SETID":
                    ExtractSETIDParameters(methodParameters[1]);
                    break;
            }
        }
    }

    private void ExtractCHOOSEParameters(string param1, string param2)
    {
        string[] choicesSplit = param1.Split(',');
        string[] affectResultSplit = param2.Split(',');

        int[] choices = Array.ConvertAll(choicesSplit, (e) => int.Parse(e));
        int[] affectResults = Array.ConvertAll(affectResultSplit, (e) => int.Parse(e));

        CHOOSE(choices, affectResults);
    }

    private void ExtractSETRANDOMParameters(string[] param1)
    {
        if (param1.Length > 1) // 파라미터가 있다면
        {
            int count = int.Parse(param1[1]);
            SETRANDOM(count);
        }
        else
        {
            SETRANDOM(1); // 아무런 파라미터가 없었다면 1로 실행
        }
    }

    private void ExtractSETIDParameters(string param1)
    {
        string[] IDSplit = param1.Split(',');
        int[] ids = Array.ConvertAll(IDSplit, (e) => int.Parse(e));

        SETID(ids);
    }
    /// <summary>
    /// translation_id 중에 선택하여 선택한 순서에 따른 dialog_id로 이동
    /// </summary>
    /// <param name="choices"> translation_id 배열 </param>
    /// <param name="affectResults"> dialog_id 배열 </param>
    /// <returns></returns>
    private void CHOOSE(int[] choices, int[] affectResults)
    {
        if (choices.Length != affectResults.Length)
        {
            Debug.LogError("에러 : CHOOSE의 파라미터 길이가 각각 다릅니다.");
            return;
        }

        for (int i = 0; i < choicePanelTrm.transform.childCount; i++)
        {
            choicePanelTrm.transform.GetChild(i).gameObject.SetActive(false);
        }

        choicePanelTrm.gameObject.SetActive(true);
        Global.UI.UIFade(choicePanelTrm, false);

        for (int i = 0; i < choices.Length; i++)
        {
            CounterSelectButtonUI selectButton = Global.Pool.GetItem<CounterSelectButtonUI>();
            int affectResult = affectResults[i];
            selectButton.Init(TranslationManager.Instance.GetLangDialog(choices[i]), () =>
            {
                CookingManager.Counter.AddDialog(affectResult);
                choicePanelTrm.gameObject.SetActive(false);
                CookingDialogPanel.eventWaitFlag = false;
            });
        }

        onTextEndAction += () =>
        {
            Global.UI.UIFade(choicePanelTrm, Define.UIFadeType.IN, 0.5f, false);
        };
    }

    private void SETRANDOM(int count)
    {
        RecipeSO[] allRecipes = CookingManager.Global.canAppearRecipes;
        List<int> randomIndexList = new List<int>();
        List<int> chosenIndexs = new List<int>();

        for (int i = 0; i < allRecipes.Length; i++)
        {
            randomIndexList.Add(i);
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, randomIndexList.Count);
            chosenIndexs.Add(allRecipes[randomIndexList[randomIndex]].recipeNameTranslationId);
            randomIndexList.Remove(randomIndexList[randomIndex]);
        }

        SETID(chosenIndexs.ToArray());
    }

    private void SETID(int[] id)
    {
        counterIngredientInventories.Clear();

        for (int i = 0; i < ingredientPanelTrm.transform.childCount; i++)
        {
            ingredientPanelTrm.transform.GetChild(i).gameObject.SetActive(false);
        }

        ingredientPanelTrm.gameObject.SetActive(true);
        timerGroup.gameObject.SetActive(true);
        Global.UI.UIFade(ingredientPanelTrm, false);
        Global.UI.UIFade(timerGroup, false);

        int totalCookingTime = 0;
        List<RecipeSO> orderRecipes = new List<RecipeSO>();

        for (int i = 0; i < id.Length; i++)
        {
            CounterIngredientInventoryUI counterInventory = Global.Pool.GetItem<CounterIngredientInventoryUI>();
            RecipeSO recipePair = CookingManager.Global.RecipeIDPairDic[id[i]];
            totalCookingTime += recipePair.averageCookingTime;
            orderRecipes.Add(recipePair);

            counterInventory.Init(recipePair.foodIngredient, counterIngredientInventories);
            counterIngredientInventories.Add(counterInventory);
        }

        onTextEndAction += () =>
        {
            Global.UI.UIFade(ingredientPanelTrm, Define.UIFadeType.IN, 0.5f, false);
            CookingManager.SetRecipes(orderRecipes.ToArray());
            Global.UI.UIFade(timerGroup, true);
            CookingManager.Counter.SetTimer(totalCookingTime);

            CookingDialogInfo info = CookingDialogPanel.currentDialog;
            //CookingManager.Counter.guestTalk.AddBubbleMessage(TranslationManager.Instance.GetLangDialog(info.tranlationId));
        };
    }

    public void ResetEvent()
    {
        CookingDialogPanel.eventWaitFlag = false;
        choicePanelTrm.gameObject.SetActive(false);
        ingredientPanelTrm.gameObject.SetActive(false);
        timerGroup.gameObject.SetActive(false);
    }

    public void OnTextEnd()
    {
        if(onTextEndAction != null)
        {
            onTextEndAction.Invoke();
        }

        onTextEndAction = null;
    }
}
