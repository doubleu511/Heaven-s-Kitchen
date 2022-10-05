using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TranslationManager : MonoBehaviour
{
    [System.Serializable]
    public class Lang
    {
        public string lang;
        public List<LangInfo> langList = new List<LangInfo>();
    }

    [System.Serializable]
    public class LangInfo
    {
        public int id;
        public string value;

        public LangInfo(int id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }

    const string langURL = "https://docs.google.com/spreadsheets/d/e/2PACX-1vSnhHa74L_5xlGg0tsKu1IRmU1P4-lt6twf7n0jiVbZUaaGGD84Ml6n_vNvItvoUT5lqRtjsm5kMCAM/pub?gid=0&single=true&output=tsv";
    public event System.Action LocalizeChanged = () => { };
    public int curLangIndex;
    public List<Lang> Langs;

    private Dictionary<int, string> langDic = new Dictionary<int, string>();

    public FostDialogDic FostDialog;
    public CookingDialogDic CookingDialog;

    public static TranslationManager s_instance;
    public static TranslationManager Instance
    {
        get
        {
            Init();
            return s_instance;
        }
    }

    static void Init()
    {
        if (!s_instance)
        {
            GameObject resource = Resources.Load<GameObject>("Init/Translation");
            GameObject translation = Instantiate(resource, null);

            s_instance = translation.GetComponent<TranslationManager>();
            s_instance.FostDialog = translation.transform.Find("FostDialog").GetComponent<FostDialogDic>();
            s_instance.FostDialog.TransformDialogDic();

            s_instance.CookingDialog = translation.transform.Find("CookingDialog").GetComponent<CookingDialogDic>();
            s_instance.CookingDialog.TransformDialogDic();

            DontDestroyOnLoad(translation);
            s_instance.StartCoroutine(s_instance.InitLang());
        }
    }

    IEnumerator InitLang()
    {
        int langIndex = PlayerPrefs.GetInt("LangIndex", -1);
        int systemIndex = Langs.FindIndex(x => x.lang.ToLower() == Application.systemLanguage.ToString().ToLower()); // 시스템 랭귀지와 똑같은걸로 맞춘다.
        if (systemIndex == -1) systemIndex = 0; // 똑같은게 없다면 기본 한국어로 설정
        int index = langIndex == -1 ? systemIndex : langIndex; // 저장되지 않았다면 위에서 구한 시스템 인덱스로 설정

        index = 0;
        SetLangIndex(index);
        yield return null;
    }

    [ContextMenu("RefreshLangDic")]
    private void RefreshLangDic()
    {
        List<LangInfo> langInfo = Langs[curLangIndex].langList;

        for (int i = 0; i< langInfo.Count;i++)
        {
            langDic[langInfo[i].id] = langInfo[i].value;
        }
    }

    public void SetLangIndex(int index)
    {
        curLangIndex = index;
        PlayerPrefs.SetInt("LangIndex", curLangIndex);
        RefreshLangDic();

        LocalizeChanged();
    }

    public bool HasKey(int id)
    {
        return langDic.ContainsKey(id);
    }

    public string GetLangDialog(int dialogId)
    {
        if (langDic.ContainsKey(dialogId))
        {
            return langDic[dialogId];
        }
        else
        {
            return "Missing";
        }

    }

#if UNITY_EDITOR
    [ContextMenu("언어 가져오기")]
    void GetLang()
    {
        StartCoroutine(GetLangCo());
    }

    IEnumerator GetLangCo()
    {
        UnityWebRequest www = UnityWebRequest.Get(langURL);
        yield return www.SendWebRequest();
        SetLangList(www.downloadHandler.text);
    }

    void SetLangList(string tsv)
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
        Langs = new List<Lang>();
        for (int i = 1; i < columnSize; i++)
        {
            Lang lang = new Lang();
            lang.lang = Sentence[0, i];

            for (int j = 1; j < rowSize; j++)
            {
                int id = int.Parse(Sentence[j, 0]);

                LangInfo info = new LangInfo(id, Sentence[j, i]);
                lang.langList.Add(info);
            }
            Langs.Add(lang);
        }
    }
#endif
}