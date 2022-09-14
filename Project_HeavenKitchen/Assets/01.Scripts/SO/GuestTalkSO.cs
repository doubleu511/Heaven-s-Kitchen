using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuestTalk Scriptable Object", menuName = "ScriptableObjects/GuestTalk Scriptable Object")]
public class GuestTalkSO : ScriptableObject
{
    [SerializeField] List<GuestTalkCondition> conditions = new List<GuestTalkCondition>();
    private List<GuestTalkCondition> filteredConditions = new List<GuestTalkCondition>();

    public GuestTalkCondition[] GetFilteredConditions()
    {
        return filteredConditions.ToArray();
    }

    public void FilteringConditions()
    {
        filteredConditions.Clear();

        for(int i =0; i<conditions.Count;i++)
        {
            if(conditions[i].specialTalkId == -1)
            {
                filteredConditions.Add(conditions[i]);
            }
        }

        RecipeSO[] recipes = CookingManager.GetRecipes();
        for(int i =0; i<recipes.Length;i++)
        {
            if(IsHaveSpecialId(recipes[i].foodIngredient.ingredientTranslationId, out GuestTalkCondition guestTalk))
            {
                filteredConditions.Add(guestTalk);
            }
        }
    }

    public bool IsHaveSpecialId(int id, out GuestTalkCondition specialTalk)
    {
        specialTalk = null;

        GuestTalkCondition condition = conditions.Find(x => x.specialTalkId == id);
        if (condition != null)
        {
            specialTalk = condition;
            return true;
        }

        return false;
    }
}

[System.Serializable]
public class GuestTalkCondition
{
    public int specialTalkId;
    public int[] translationIds;
}
