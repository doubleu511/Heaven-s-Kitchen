using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Define
{
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    public enum DialogType
    {
        TalkLeft = 1,
        TalkRight = 2,
        ActionEvent = 3,
    }

    public enum CharacterSpeaker
    {
        NONE = 0,
        OLIVE = 1,
        MACADAMIA = 2
    }

    public enum CharacterTextStyle
    {
        DEFAULT = 0,
        OLIVE = 1
    }

}
