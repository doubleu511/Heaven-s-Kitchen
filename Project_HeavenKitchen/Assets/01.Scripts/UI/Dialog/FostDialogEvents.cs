using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class FostDialogEvents : MonoBehaviour
{
    [SerializeField] CanvasGroup blackScreen;
    [SerializeField] CanvasGroup whiteScreen;

    public void ThrowEvent(string _eventMethod)
    {
        // �̰��� _eventMethod�� �ؼ��ϴ� �ڵ� �ۼ�
        string[] methods = _eventMethod.Split('\n');
        if (methods.Length > 1)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                ThrowEvent(methods[i]);
            }

            return;
        }

        Invoke(_eventMethod, 0);
    }

    private void FADE_BLACK()
    {
        Global.UI.UIFade(blackScreen, UIFadeType.IN, 1, true, () =>
        {
            DialogPanel.eventWaitFlag = false;
            Global.UI.UIFade(blackScreen, UIFadeType.OUT, 1, true);
        });
    }

    private void FADE_WHITE()
    {
        Global.UI.UIFade(whiteScreen, UIFadeType.IN, 1, true, () =>
        {
            DialogPanel.eventWaitFlag = false;
            Global.UI.UIFade(whiteScreen, UIFadeType.OUT, 1, true);
        });
    }
}
