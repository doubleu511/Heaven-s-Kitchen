using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialogEvents : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen;
    [SerializeField] CanvasGroup whiteScreen;

    public void ThrowEvent(string _eventName)
    {
        // 이곳에 _eventName을 해석하는 코드 작성
        Invoke(_eventName, 0);
    }

    private void FADE_BLACK()
    {
        Global.UI.UIFade(blackScreen, true, 1, true, () =>
        {
            DialogPanel.eventWaitFlag = false;
            Global.UI.UIFade(blackScreen, false, 1, true);
        });
    }

    private void FADE_WHITE()
    {
        Global.UI.UIFade(whiteScreen, true, 1, true, () =>
        {
            DialogPanel.eventWaitFlag = false;
            Global.UI.UIFade(whiteScreen, false, 1, true);
        });
    }
}
