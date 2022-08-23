using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingDialogEvents : MonoBehaviour
{
    public void ThrowEvent(string _eventMethod)
    {
        // 이곳에 _eventMethod를 해석하는 코드 작성
        string[] methods = _eventMethod.Split('\n');
        if (methods.Length > 1)
        {
            for (int i = 0; i < methods.Length; i++)
            {
                ThrowEvent(methods[i]);
            }

            return;
        }

        string[] methodParameters = _eventMethod.Split(' ');

        if(methodParameters.Length > 0)
        {
            switch(methodParameters[0])
            {
                case "CHOOSE":
                    ExtractCHOOSEParameters(methodParameters[1], methodParameters[2], out int[] choices, out int[] affectResults);
                    CHOOSE(choices, affectResults);
                    break;
                case "SETRANDOM":
                    SETRANDOM();
                    break;
                case "SETID":
                    int id = int.Parse(methodParameters[1]);
                    SETID(id);
                    break;
            }
        }
    }

    private void ExtractCHOOSEParameters(string param1, string param2, out int[] choices, out int[] affectResults)
    {
        string[] choicesSplit = param1.Split(',');
        string[] affectResultSplit = param2.Split(',');

        choices = Array.ConvertAll(choicesSplit, (e) => int.Parse(e));
        affectResults = Array.ConvertAll(affectResultSplit, (e) => int.Parse(e));
    }

    /// <summary>
    /// translation_id 중에 선택하여 선택한 순서에 따른 dialog_id로 이동
    /// </summary>
    /// <param name="choices"> translation_id 배열 </param>
    /// <param name="affectResults"> dialog_id 배열 </param>
    /// <returns></returns>
    private void CHOOSE(int[] choices, int[] affectResults)
    {

    }

    private void SETRANDOM()
    {

    }

    private void SETID(int id)
    {

    }
}
