using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PromoteAct : MonoBehaviour
{
    private Animator myAnim;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    public virtual void StartPromote()
    {
        myAnim.SetFloat("Speed", 0);
        StartCoroutine(StartDelay(2f));
        PromoteManager.PromoteResult.ShowStart();

        PromoteManager.Promote.ScheduleCurrentRepeatCount = 0;
    }

    private IEnumerator StartDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        myAnim.SetFloat("Speed", 1);
    }

    public virtual void OnFinishedPrepareAct()
    {
        bool isSuccess = StatHandler.TryResultFromStress();
        myAnim.SetBool("isSuccess", isSuccess);

        if(isSuccess)
        {
            StatHandler.TryAddStat(true);
            PromoteManager.PromoteResult.ShowSuccess();
        }
        else
        {
            StatHandler.TryAddStat(false);
            PromoteManager.PromoteResult.ShowFail();
        }
    }

    public virtual void OnFinishedResultAct()
    {
        PromoteManager.Promote.ScheduleCurrentRepeatCount++;
        PromoteManager.Calender.AddDay();

        if(PromoteManager.Promote.ScheduleRepeatCount > PromoteManager.Promote.ScheduleCurrentRepeatCount)
        {
            myAnim.SetTrigger("Restart");
        }
        else
        {
            print("다음 스케줄로");
            PromoteManager.GoNextPromote();
        }
    }
}
