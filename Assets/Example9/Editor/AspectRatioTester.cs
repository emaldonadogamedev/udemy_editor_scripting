using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[InitializeOnLoad]
public class AspectRatioTester : Editor
{ 
    static string applicationDataPath = Application.dataPath + "/";
    static int numberOfScreenshotsSaved = 0;
    static bool menuOptionWasClicked = false;
    static bool startedScreenshot = false;
    static readonly List<string> aspectRatios = new()
    {
        applicationDataPath + "Free Aspect.png",
        applicationDataPath + "16:9 Aspect.png",
        applicationDataPath + "16:10 Aspect.png",
        applicationDataPath + "Full HD (1920 x 1080).png"
    };

    static AspectRatioTester()
    {
        EditorApplication.update += EditorToolLoop;
    }

    [MenuItem("Custom Tools/Test Aspect Ratios")]
    static void TestAspectRatios()
    {
        menuOptionWasClicked = true;
        startedScreenshot = false;
        numberOfScreenshotsSaved = 0;
    }

    static void EditorToolLoop()
    {
        if (menuOptionWasClicked && startedScreenshot)
        {
            startedScreenshot = true;
        }


        if (menuOptionWasClicked && !startedScreenshot)
        {
            startedScreenshot = true;
            SaveScreenshotAtAspectRatio(
                numberOfScreenshotsSaved,
                aspectRatios[numberOfScreenshotsSaved]);
        }

        if (numberOfScreenshotsSaved < aspectRatios.Count && System.IO.File.Exists(aspectRatios[numberOfScreenshotsSaved]))
        {
            numberOfScreenshotsSaved++;
            startedScreenshot = false;

            Refresh();
        }
            
        if (numberOfScreenshotsSaved == aspectRatios.Count)
        {
            menuOptionWasClicked = false;
        }
    }

    static void Refresh()
    {
        AssetDatabase.Refresh();
    }

    public static void SetSize(int index)
    {
        var gameViewWindowType =
            typeof(Editor).Assembly.GetType("UnityEditor.GameView");

        var gameViewWindow = EditorWindow.GetWindow(gameViewWindowType);

        var SizeSelectionCallback =
            gameViewWindowType.GetMethod(
                "SizeSelectionCallback",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        SizeSelectionCallback.Invoke(gameViewWindow, new object[] { index, null });
    }

    public static void TakeScreenshot(string fileName)
    {
        ScreenCapture.CaptureScreenshot(fileName);
    }

    public static void SaveScreenshotAtAspectRatio(int index, string fileName)
    {
        SetSize(index);
        TakeScreenshot(fileName);
    }
}