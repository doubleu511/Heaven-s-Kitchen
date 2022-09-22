using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [Header("Deco")]
    [SerializeField] SpriteRenderer[] stoveButtonSR;
    [SerializeField] GameObject[] fireAnimation;

    private bool[] isFire = new bool[4];
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        isFire = new bool[stoveButtonSR.Length];
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < fireAnimation.Length; i++)
        {
            isFire[i] = false;
            fireAnimation[i].SetActive(false);
            stoveButtonSR[i].transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }

    public void SetFireIndex(int index, bool value)
    {
        isFire[index] = value;
        fireAnimation[index].SetActive(value);
        stoveButtonSR[index].transform.rotation = Quaternion.Euler(value ? new Vector3(0, 0, 90) : Vector3.zero);
        GasSoundPlay();
    }

    public void BurnerSoundPlay()
    {
        Global.Sound.Play("SFX/gas_stove", Define.Sound.Effect);
    }

    private void GasSoundPlay()
    {
        bool play = false;

        for (int i = 0; i < isFire.Length; i++)
        {
            if(isFire[i])
            {
                play = true;
            }
        }

        if(play)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}