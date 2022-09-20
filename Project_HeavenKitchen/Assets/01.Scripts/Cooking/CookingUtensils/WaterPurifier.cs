using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPurifier : IngredientSender
{
    [SerializeField] ParticleSystem waterSmokeParticle;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        if(CookingManager.Player.Inventory.InventoryAdd(ingredientBox))
        {
            if(!waterSmokeParticle.isPlaying)
            {
                Global.Sound.Play("SFX/Utensils/waterpurifier_00", audioSource);
            }
            Global.Sound.Play("SFX/Utensils/waterpurifier_01", audioSource);

            waterSmokeParticle.Play();
        }
    }
}