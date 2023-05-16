using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace TheAshBot
{
    public static class Logger
    {

        public static string Color(this string _string, string color)
        {
            return "<color=" + color + ">" + _string + "</color>";
        }
        public static string Size(this string _string, int size)
        {
            return "<size=" + size + ">" + _string + "</size>";
        }
        public static string Bold(this string _string)
        {
            return "<b>" + _string + "</b>";
        }


        public static void Log(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.Log, "<size=14>", _Object, "</size>", message);
        }

        public static void LogSuccess(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.Log, "<size=14><color=green>", _Object, "</color></size>", message);
        }


        public static void LogError(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.LogError, "<color=red><b><size=16>!!!", _Object, "</size></b></color>", message);
        }

        public static void LogFakeError(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.Log, "<color=red><b><size=16>!!!", _Object, "</size></b></color>", message);
        }


        public static void LogWarning(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.LogWarning, "<color=yellow><size=16>", _Object, "</size></color>", message);
        }

        public static void LogFakeWarning(this UnityEngine.Object _Object, params object[] message)
        {
            LogBase(Debug.Log, "<color=yellow><size=16>", _Object, "</size></color>", message);
        }


        private static void LogBase(Action<string, UnityEngine.Object> logFunction, string prefix, UnityEngine.Object _Object, string sufix = "", params object[] message)
        {
#if UNITY_EDITOR
            logFunction(prefix + _Object.name.Color("lightblue") + ": " + String.Join("; ", message) + sufix, _Object);
#endif
        }
    }
}
