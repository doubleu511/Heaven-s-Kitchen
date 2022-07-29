using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Dialog
{
    public int dialogid;
    public List<DialogInfo> dialogInfos = new List<DialogInfo>();
}

[System.Serializable]
public class DialogInfo
{
    public int tranlationId;
    public int background;
    public int type;
    public int leftChracter;
    public int rightChracter;
    public int faceIndex;
    public int leftClothes;
    public int rightClothes;
    public string eventName;
    // ...더 추가
}

public class DialogDic : MonoBehaviour
{
    const string dialogURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSnhHa74L_5xlGg0tsKu1IRmU1P4-lt6twf7n0jiVbZUaaGGD84Ml6n_vNvItvoUT5lqRtjsm5kMCAM/pub?gid=1863484244&single=true&output=tsv";

    public List<Dialog> Dialogs;

    private Dictionary<int, Dialog> dialogDic = new Dictionary<int, Dialog>();

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

    public Dialog GetDialog(int dialogId)
    {
        return dialogDic[dialogId];
    }

#if UNITY_EDITOR
    [ContextMenu("다이얼로그 가져오기")]
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
        Dialogs = new List<Dialog>();
        Dialog curDialog = null;
        int savedDialogId = -1;
        int savedBackground = -1;
        int savedLeftClothes = -1;
        int savedRightClothes = -1;

        for (int j = 1; j < rowSize; j++)
        {
            //TO DO : 다이얼로그 옵션 추가될 때마다 이곳에 정보를 추가할것
            DialogInfo info = new DialogInfo();

            int _dialogId = TryParse(Sentence[j, 0], ref savedDialogId, false);
            int _type = int.Parse(Sentence[j, 3]);
            string _eventName = Sentence[j, 9];

            info.type = _type;
            if (false == string.IsNullOrEmpty(_eventName))
            {
                info.eventName = _eventName;
            }
            else
            {
                int _translationId = int.Parse(Sentence[j, 1]);
                int _background = TryParse(Sentence[j, 2], ref savedBackground, true);
                int _leftChara = int.Parse(Sentence[j, 4]);
                int _rightChara = int.Parse(Sentence[j, 5]);
                int _faceIndex = int.Parse(Sentence[j, 6]);

                int _leftClothes = TryParse(Sentence[j, 7], ref savedLeftClothes, true);
                int _rightClothes = TryParse(Sentence[j, 8], ref savedRightClothes, true);

                info.tranlationId = _translationId;
                info.background = _background;
                info.leftChracter = _leftChara;
                info.rightChracter = _rightChara;
                info.faceIndex = _faceIndex;
                info.leftClothes = _leftClothes;
                info.rightClothes = _rightClothes;
            }

            if (savedDialogId != _dialogId)
            {
                if (curDialog != null)
                {
                    Dialogs.Add(curDialog);
                }

                curDialog = new Dialog();
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