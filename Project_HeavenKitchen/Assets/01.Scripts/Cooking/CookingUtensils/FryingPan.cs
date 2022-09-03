using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MinigameStarter
{
    [SerializeField] ParticleSystem grillSmokeParticle;
    [SerializeField] GameObject[] fireAnimation;

    public void SetSmokeState(int index)
    {
        var main = grillSmokeParticle.main;
        var emission = grillSmokeParticle.emission;

        switch (index)
        {
            case 0: // ����
                grillSmokeParticle.Stop();
                break;
            case 1: // �Ͼ� ����
                main.startColor = Color.white;
                grillSmokeParticle.Play();
                emission.rateOverTime = 2;
                break;
            case 2: // ���� ����
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
    }

    public override void OnMinigameEnd()
    {
        SetSmokeState(0);
        for (int i = 0; i < fireAnimation.Length; i++)
        {
            fireAnimation[i].SetActive(false);
        }
    }
}
