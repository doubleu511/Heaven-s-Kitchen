using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DialogDic : MonoBehaviour
{
    [System.Serializable]
    public class Dialog
    {
        public int dialogid;
        public DialogInfo[] dialogInfos;
    }

    [System.Serializable]
    public class DialogInfo
    {
        public int tranlationId;
        // ...더 추가
    }

    const string dialogURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSnhHa74L_5xlGg0tsKu1IRmU1P4-lt6twf7n0jiVbZUaaGGD84Ml6n_vNvItvoUT5lqRtjsm5kMCAM/pub?gid=1863484244&single=true&output=tsv";

    public List<Dialog> Dialogs;

    private Dictionary<int, Dialog> dialogDic = new Dictionary<int, Dialog>();

    [ContextMenu("RefreshDialogDic")]
    public void RefreshDialogDic()
    {
        for (int i = 0; i< Dialogs.Count;i++)
        {
            dialogDic[Dialogs[i].dialogid] = Dialogs[i];
        }
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
        for (int i = 1; i < columnSize; i++)
        {
            Dialog diaog = new Dialog();

            for (int j = 1; j < rowSize; j++)
            {
                /* TO DO : 다이얼로그 옵션 추가될 때마다 이곳에 정보를 추가할것
                int id = int.Parse(Sentence[j, 0]);

                LangInfo info = new LangInfo(id, Sentence[j, i]);
                lang.langList.Add(info);
                */
            }
            Dialogs.Add(diaog);
        }
    }
}
#endif