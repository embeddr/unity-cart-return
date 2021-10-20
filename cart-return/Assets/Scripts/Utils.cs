// General utilities

#define ASSERTS_ENABLED

using UnityEngine;

public static class Utils
{
    public static void ExitGame(string msg)
    {
        Debug.Log(msg);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static void Assert(bool condition, string msg)
    {
#if ASSERTS_ENABLED
        if (!condition)
        {
            ExitGame(msg);
        }
#endif
    }
}