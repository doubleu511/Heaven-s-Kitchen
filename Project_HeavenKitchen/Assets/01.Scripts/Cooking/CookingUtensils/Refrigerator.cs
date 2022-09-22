using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Refrigerator : MinigameStarter
{
    private readonly int REFRIGERATOR_WIDTH = 2;
    private readonly int REFRIGERATOR_HEIGHT = 2;
    private readonly int REFRIGERATOR_SIZE = 800;

    private int curIndex = 0;
    private bool isOpen = false;

    [SerializeField] RectTransform content;

    [Header("Buttons")]
    [SerializeField] Button leftBtn;
    [SerializeField] Button rightBtn;
    [SerializeField] Button topBtn;
    [SerializeField] Button bottomBtn;
    private Button[] buttons;

    [Header("Deco")]
    public Sprite spr_refeigeratorOpen;
    public Sprite spr_refeigeratorClose;
    public GameObject leftWing;
    public GameObject rightWing;
    public ParticleSystem coldAirParticle;

    private OutlineController outline;
    private SpriteRenderer refrigeratorBaseSR;
    private AudioSource audioSource;

    private void Awake()
    {
        outline = GetComponent<OutlineController>();
        refrigeratorBaseSR = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void Start()
    {
        base.Start();
        buttons = new Button[4] { leftBtn, rightBtn, topBtn, bottomBtn };
        RefreshButtonAppear();

        audioSource.volume = 0.3f;
        transform.DOScaleX(1.02f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOScaleY(1.1f, 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public override void OnInteract()
    {
        base.OnInteract();
        RefreshButtonAppear();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
        }

        leftBtn.onClick.AddListener(() => MoveContent(curIndex - 1));
        rightBtn.onClick.AddListener(() => MoveContent(curIndex + 1));
        topBtn.onClick.AddListener(() => MoveContent(curIndex - REFRIGERATOR_WIDTH));
        bottomBtn.onClick.AddListener(() => MoveContent(curIndex + REFRIGERATOR_WIDTH));

        if (!isOpen)
        {
            isOpen = true;

            refrigeratorBaseSR.sprite = spr_refeigeratorOpen;

            leftWing.gameObject.SetActive(true);
            rightWing.gameObject.SetActive(true);
            outline.RefreshOutline();

            coldAirParticle.Play();
            audioSource.volume = 0.9f;
            Global.Sound.Play("SFX/Utensils/refrigerator_01", Define.Sound.Effect);
        }
    }

    private void MoveContent(int index)
    {
        curIndex = index;
        int col = curIndex % REFRIGERATOR_WIDTH;
        int row = curIndex / REFRIGERATOR_HEIGHT;

        content.DOAnchorPos(new Vector2(col * -REFRIGERATOR_SIZE, row * REFRIGERATOR_SIZE), 1).OnComplete(() =>
        {
            RefreshButtonAppear();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
        });

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
    }

    private void RefreshButtonAppear()
    {
        int col = curIndex % REFRIGERATOR_WIDTH;
        int row = curIndex / REFRIGERATOR_HEIGHT;

        leftBtn.gameObject.SetActive(col > 0);
        rightBtn.gameObject.SetActive(col < REFRIGERATOR_WIDTH - 1);
        topBtn.gameObject.SetActive(row > 0);
        bottomBtn.gameObject.SetActive(row < REFRIGERATOR_WIDTH - 1);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOpen)
            {
                isOpen = false;

                refrigeratorBaseSR.sprite = spr_refeigeratorClose;
                leftWing.gameObject.SetActive(false);
                rightWing.gameObject.SetActive(false);
                outline.RefreshOutline();

                coldAirParticle.Stop();
                Global.Sound.Play("SFX/Utensils/refrigerator_01", Define.Sound.Effect);
                audioSource.volume = 0.3f;
            }
        }
    }
}
