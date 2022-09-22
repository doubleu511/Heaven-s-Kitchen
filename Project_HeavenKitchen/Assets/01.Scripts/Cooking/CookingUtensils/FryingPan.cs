using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryingPan : MinigameStarter
{
    [SerializeField] ParticleSystem grillSmokeParticle;
    [SerializeField] Stove stoveObject;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        base.OnInteract();
        Global.Sound.Play("SFX/gas_stove", Define.Sound.Effect);
    }

    public void SetState(int index)
    {
        var main = grillSmokeParticle.main;
        var emission = grillSmokeParticle.emission;

        switch (index)
        {
            case 0: // 없음
                grillSmokeParticle.Stop();
                audioSource.Stop();
                break;
            case 1: // 하얀 연기
                main.startColor = Color.white;
                grillSmokeParticle.Play();
                emission.rateOverTime = 2;

                AudioClip grillSFX = Global.Sound.GetOrAddAudioClip("SFX/Utensils/frypan_00");
                audioSource.clip = grillSFX;
                audioSource.Play();
                break;
            case 2: // 검은 연기
                main.startColor = Color.gray;
                grillSmokeParticle.Play();
                emission.rateOverTime = 5;

                Global.Sound.Play("SFX/Utensils/frypan_01", audioSource);
                break;
        }
    }

    public override void OnMinigameStart()
    {
        SetState(1);
        stoveObject.SetFireIndex(3, true);
    }

    public override void OnMinigameEnd()
    {
        SetState(0);
        stoveObject.SetFireIndex(3, false);
    }
}
