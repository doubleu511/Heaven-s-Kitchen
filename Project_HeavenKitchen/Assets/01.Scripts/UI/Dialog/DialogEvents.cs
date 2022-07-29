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
        // �̰��� _eventName�� �ؼ��ϴ� �ڵ� �ۼ�
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
