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

    #region 다이얼로그
    public enum DialogType
    {
        TALKLEFT = 1,
        TALKRIGHT = 2,
        ACTIONEVENT = 3,
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
    #endregion

    #region UI Type
    [Flags]
    public enum UIFadeType
    {
        FADE =          1,
        FLOAT =         2,
        //==================================
        OUT =           0,
        IN =            FADE,
        FLOATOUT =      FLOAT,
        FLOATIN =       FADE | FLOAT
    }

    public enum UIEffectType
    {
        SHAKE = 1,
    }
    #endregion

    #region 요리 씬
    public enum CookingTool
    {
        COOKING_BOWL,
        COOKING_CUPBOARD,
        COOKING_TRAY,
        COOKING_FRYINGPAN,
        COOKING_REFRIGERATOR,
        COOKING_KETTLE,
        COOKING_OVEN,
        COOKING_POT,
        COOKING_MICROWAVE,

        COOKING_TRASHCAN = 99
    }

    public enum MinigameType
    {
        MINIGAME_CLUMPING,
        MINIGAME_SPRINKLING,
        MINIGAME_GRILLING,
        MINIGAME_FILLING
    }

    [Flags]
    public enum GuestPersonality
    {
        EASYGOING = 1,
        URGENT = 2,
        FEISTY = 4,
        PATIENT = 8,
        TALKATIVE = 16
    }

    public enum TimerInterval
    {
        RED = 0,
        ORANGE,
        YELLOW,
        GREEN
    }
    #endregion

    #region 육성 씬
    public enum RankType
    {
        RANK_D,
        RANK_C,
        RANK_B,
        RANK_A,
        RANK_S
    }

    public enum StatType
    {
        KNOWLEDGE,
        DILIGENCE,
        SENSE,
        SPEED,
        HEALTH
    }
    #endregion

    #region 게임
    public enum MoneyType
    {
        GOLD,
        STARCANDY
    }

    public enum TextAnimationType
    {
        NONE = 0,
        SHAKE = 1,
    }
    #endregion
}
