using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_DialogSpeechBubble : MonoBehaviour
{
    [SerializeField] Image characterFaceImg;
    [SerializeField] TextMeshProUGUI tmpCharacterName;
    [SerializeField] TextMeshProUGUI tmpDialogText;
    [SerializeField] GameObject speechBubble;

    [SerializeField] GameObject leftBubbleArrow;
    [SerializeField] GameObject rightBubbleArrow;

    public void InitBubble(CharacterSO character, Define.DialogType speakPlace, string text)
    {
        characterFaceImg.sprite = character.characterPortrait;
        tmpCharacterName.text = TranslationManager.Instance.GetLangDialog(character.characterNameTranslationId);
        tmpDialogText.text = text;

        leftBubbleArrow.SetActive(speakPlace == Define.DialogType.TALKLEFT);
        rightBubbleArrow.SetActive(speakPlace == Define.DialogType.TALKRIGHT);

        if(speakPlace == Define.DialogType.TALKRIGHT)
        {
            speechBubble.transform.SetAsFirstSibling();
        }
    }
}
