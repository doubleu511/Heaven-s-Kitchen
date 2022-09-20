using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceCooker : IngredientSender
{
    private OutlineController outline;

    [Header("Design")]
    private SpriteRenderer riceCookerBaseSR;
    public SpriteRenderer riceCookerTopSR;
    public Sprite spr_riceCookerBase_Closed;
    public Sprite spr_riceCookerBase_Opened;
    public ParticleSystem smokeParticle;

    private AudioSource audioSource;
    private bool firstOpenFlag = false;
    private bool isOpen = false;

    private void Awake()
    {
        riceCookerBaseSR = GetComponent<SpriteRenderer>();
        outline = GetComponent<OutlineController>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (!isOpen)
        {
            if (!firstOpenFlag)
            {
                Global.Sound.Play("SFX/Utensils/ricecooker_00", audioSource);
                firstOpenFlag = true;
            }
            else
            {
                Global.Sound.Play("SFX/Utensils/ricecooker_01", audioSource);
            }
            isOpen = true;
            riceCookerBaseSR.sprite = spr_riceCookerBase_Opened;
            outline.RefreshOutline();
            riceCookerTopSR.gameObject.SetActive(true);
            smokeParticle.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOpen)
            {
                isOpen = false;
                Global.Sound.Play("SFX/Utensils/ricecooker_02", audioSource);
                riceCookerBaseSR.sprite = spr_riceCookerBase_Closed;
                riceCookerTopSR.gameObject.SetActive(false);
                outline.RefreshOutline();
                smokeParticle.Stop();
            }
        }
    }
}