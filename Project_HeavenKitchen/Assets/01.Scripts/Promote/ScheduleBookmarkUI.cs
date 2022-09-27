using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduleBookmarkUI : MonoBehaviour
{
    private readonly static Color behindColor = new Color(242 / 255f, 227 / 255f, 222 / 255f);

    [SerializeField] Transform onText;
    [SerializeField] Transform offText;
    [SerializeField] Transform scrollRect;

    private Image bookmarkImage;

    private void Awake()
    {
        bookmarkImage = GetComponent<Image>();
    }

    public void SetFade(bool value)
    {
        if(value)
        {
            transform.SetAsLastSibling();
        }
        else
        {
            transform.SetAsFirstSibling();
        }

        bookmarkImage.color = value ? Color.white : behindColor;
        onText.gameObject.SetActive(value);
        offText.gameObject.SetActive(!value);
        scrollRect.gameObject.SetActive(value);
    }
}
