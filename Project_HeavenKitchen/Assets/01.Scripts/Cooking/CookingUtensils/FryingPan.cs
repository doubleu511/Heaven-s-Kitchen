using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MinigameStarter
{
    [Header("Deco")]
    [SerializeField] SpriteRenderer stoveButtonSR;

    [SerializeField] ParticleSystem grillSmokeParticle;
    [SerializeField] GameObject[] fireAnimation;

    public void SetSmokeState(int index)
    {
        var main = grillSmokeParticle.main;
        var emission = grillSmokeParticle.emission;

        switch (index)
        {
            case 0: // 없음
                grillSmokeParticle.Stop();
                break;
            case 1: // 하얀 연기
                main.startColor = Color.white;
                grillSmokeParticle.Play();
                emission.rateOverTime = 2;
                break;
            case 2: // 검은 연기
                main.startColor = Color.gray;
                grillSmokeParticle.Play();
                emission.rateOverTime = 5;
                break;
        }
    }

    public override void OnMinigameStart()
    {
        SetSmokeState(1);
        for (int i = 0; i < fireAnimation.Length; i++)
        {
            fireAnimation[i].SetActive(true);
        }
        stoveButtonSR.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
    }

    public override void OnMinigameEnd()
    {
        SetSmokeState(0);
        for (int i = 0; i < fireAnimation.Length; i++)
        {
            fireAnimation[i].SetActive(false);
        }
        stoveButtonSR.transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
