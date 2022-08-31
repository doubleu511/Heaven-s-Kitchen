using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    public float distance = 2;
    public float detectDistance = 3;

    [SerializeField] Transform playerHeadTrm;

    [Header("Ingredients")]
    [SerializeField] IngredientSO cookedRiceIngredient;
    [SerializeField] IngredientSO waterIngredient;
    [SerializeField] IngredientSO[] cupBoardIngredients;

    [Header("Senders")]
    [SerializeField] InteractiveObject counter;
    [SerializeField] InteractiveObject riceCooker;
    [SerializeField] InteractiveObject waterPurifier;
    [SerializeField] InteractiveObject cupBoard;
    [SerializeField] InteractiveObject refrigerator;

    private MemoHandler memoHandler;
    private MinigameStarter currentUtensils;
    private Transform currentTarget;

    private void Awake()
    {
        memoHandler = FindObjectOfType<MemoHandler>();
    }

    private void Update()
    {
        memoHandler.GetNavInfo(out MinigameInfo info, out IngredientSO ingredient);
        if (info != null)
        {
            currentUtensils = CookingManager.Global.TargetNavDic[info.minigameNameTranslationId];
        }

        if(info == null)
        {
            // 요리가 다 만들어졌으므로 네비게이션은 카운터로 향한다.
            RotateToTarget(counter.transform);
        }
        else
        {
            if(ingredient == null)
            {
                // 재료가 더이상 없다면 주방기구로 향하면 된다.
                RotateToTarget(currentUtensils.transform);
            }
            else
            {
                // 재료에 맞는 기구로 가기
                if(ingredient.Equals(cookedRiceIngredient))
                {
                    RotateToTarget(riceCooker.transform);
                }
                else if (ingredient.Equals(waterIngredient))
                {
                    RotateToTarget(waterPurifier.transform);
                }
                else
                {
                    CupboardORRefrigerator(ingredient);
                }
            }
        }
    }

    private void CupboardORRefrigerator(IngredientSO ingredient)
    {
        for (int i = 0; i < cupBoardIngredients.Length; i++)
        {
            if (ingredient.Equals(cupBoardIngredients[i]))
            {
                RotateToTarget(cupBoard.transform);
                return;
            }
        }

        RotateToTarget(refrigerator.transform);
    }

    private void RotateToTarget(Transform target)
    {
        if (currentTarget != target)
        {
            //Init
            currentTarget = target;
        }

        Vector3 dir = currentTarget.transform.position - playerHeadTrm.transform.position;
        dir.z = 0;

        Vector3 normalDir = dir.normalized;

        Vector2 distanceDir = currentTarget.transform.position - playerHeadTrm.position;
        float distanceSqr = distanceDir.sqrMagnitude;

        if (distanceSqr > detectDistance * detectDistance)
        {
            this.transform.position = playerHeadTrm.position + normalDir * distance;
            this.transform.eulerAngles = new Vector3(0, 0, UtilClass.GetAngleFromVector(normalDir) - 90);
        }
        else
        {
            Vector3 newPos = currentTarget.transform.position;
            newPos.y += 1;

            this.transform.position = new Vector3(newPos.x, newPos.y + Mathf.Sin(Time.time * 5) * 0.2f);
            this.transform.eulerAngles = new Vector3(0, 0, 180);
        }
    }
}