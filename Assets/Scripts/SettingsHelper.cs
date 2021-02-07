using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class SettingsHelper
{
#if UNITY_ANDROID || UNITY_IOS
    private const int DEFAULT_POINTER_OFFSET = 1;
#else
    private const int DEFAULT_POINTER_OFFSET = 0;
#endif
    public static bool PointerDragOffsetEnabled
    {
        get => PlayerPrefs.GetInt("PointerOffsetEnabled", DEFAULT_POINTER_OFFSET) != 0;
        set => PlayerPrefs.SetInt("PointerOffsetEnabled", value ? 1 : 0);
    }
}