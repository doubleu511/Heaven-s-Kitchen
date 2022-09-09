using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class CookingDialog
{
    public int dialogid;
    public List<CookingDialogInfo> dialogInfos = new List<CookingDialogInfo>();
}

[System.Serializable]
public class CookingDialogInfo
{
    public int tranlationId;
    public int faceIndex;
    public int speechbubble_type;
    public int text_animation_type;
    public string eventMethod;
    // ...더 추가

    public CookingDialogInfo(int _translationId, int _faceIndex, int _speechbubbleType, int _textAnimationType, string _eventMethod)
    {
        tranlationId = _translationId;
        faceIndex = _faceIndex;
        speechbubble_type = _speechbubbleType;
        text_animation_type = _textAnimationType;
        eventMethod = _eventMethod;
    }

    public CookingDialogInfo(int _translationId)
    {
        tranlationId = _translationId;
        faceIndex = 0;
        speechbubble_type = 0;
        text_animation_type = 0;
        eventMethod = "";
    }
}

public class CookingDialogDic : MonoBehaviour
{
    const string dialogURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSnhHa74L_5xlGg0tsKu1IRmU1P4-lt6twf7n0jiVbZUaaGGD84Ml6n_vNvItvoUT5lqRtjsm5kMCAM/pub?gid=2057539885&single=true&output=tsv";

    public List<CookingDialog> Dialogs;

    private Dictionary<int, CookingDialog> dialogDic = new Dictionary<int, CookingDialog>();

    [ContextMenu("TransformDialogDic")]
    public void TransformDialogDic()
    {
        for (int i = 0; i< Dialogs.Count;i++)
        {
            dialogDic[Dialogs[i].dialogid] = Dialogs[i];
        }
        Dialogs.Clear();
    }

    public bool HasKey(int id)
    {
        return dialogDic.ContainsKey(id);
    }

    public CookingDialog GetDialog(int dialogId)
    {
        return dialogDic[dialogId];
    }

#if UNITY_EDITOR
    [ContextMenu("요리 다이얼로그 가져오기")]
    void GetDialog()
    {
        StartCoroutine(GetDialogCo());
    }

    IEnumerator GetDialogCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(dialogURL);
        yield return www.SendWebRequest();
        SetDialogList(www.downloadHandler.text);
    }

    void SetDialogList(string tsv)
    {
        // 이차원 배열
        string[] row = tsv.Split('\n');
        int rowSize = row.Length;
        int columnSize = row[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
        string[,] Sentence = new string[rowSize, columnSize];

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');
            for (int j = 0; j < columnSize; j++) Sentence[i, j] = column[j];
        }

        // 클래스 리스트
        Dialogs = new List<CookingDialog>();
        CookingDialog curDialog = null;
        int savedDialogId = -1;

        for (int j = 1; j < rowSize; j++)
        {
            //TO DO : 다이얼로그 옵션 추가될 때마다 이곳에 정보를 추가할것

            int _dialogId = TryParse(Sentence[j, 0], ref savedDialogId, false);
            int _translationId = int.Parse(Sentence[j, 1]);
            if(!int.TryParse(Sentence[j, 2], out int _faceIndex))
            {
                _faceIndex = 0;
            }
            if (!int.TryParse(Sentence[j, 3], out int _speechbubbleType))
            {
                _speechbubbleType = 0;
            }
            if (!int.TryParse(Sentence[j, 4], out int _textAnimationType))
            {
                _textAnimationType = 0;
            }

            string _eventMethod = Sentence[j, 5];
            if (string.IsNullOrEmpty(_eventMethod))
            {
                _eventMethod = "";
            }

            CookingDialogInfo info = new CookingDialogInfo(_translationId, _faceIndex, _speechbubbleType, _textAnimationType, _eventMethod);

            if (savedDialogId != _dialogId)
            {
                if (curDialog != null)
                {
                    Dialogs.Add(curDialog);
                }

                curDialog = new CookingDialog();
                savedDialogId = _dialogId;

                curDialog.dialogid = savedDialogId;
            }

            curDialog.dialogInfos.Add(info);
        }
        Dialogs.Add(curDialog);


        int TryParse(string value, ref int defaultValue, bool IsDefaultValueTranslation)
        {
            if (false == string.IsNullOrEmpty(value))
            {
                if(IsDefaultValueTranslation) defaultValue = int.Parse(value);
                return int.Parse(value);
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
#endif