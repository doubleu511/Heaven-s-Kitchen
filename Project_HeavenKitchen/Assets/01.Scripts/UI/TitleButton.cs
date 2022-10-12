using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{
    private Button button;
    [SerializeField] CanvasGroup blackScreen;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        Global.UI.UIFade(blackScreen, Define.UIFadeType.IN, 1, false, () =>
        {
            StatHandler.StatReset();
            Global.Pool.Clear();
            SceneManager.LoadScene("Promote");
        });
    }
}
