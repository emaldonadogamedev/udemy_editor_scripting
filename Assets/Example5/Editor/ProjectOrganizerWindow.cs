using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ProjectOrganizerWindow : EditorWindow
{
    class AssetTypeRow
    {
        public string name;
        public string fileExtension;
    }

    class OrganizerRow
    {
        public int selectedOptionIndex;
        public string folderPath;
        public Object obj;
    }

    int selectedTabIndex = 0;

    string[] tabs = { "Organizer", "Asset Type Mappings" };

    int countOfAssetTypeRows = 0;

    List<AssetTypeRow> assetTypeRows;

    int totalNumberOfFileExtensions;

    bool isDirty = false;

    string[] assetTypeNames;

    int countOforganizerRows;

    List<OrganizerRow> organizerRows;

    Dictionary<string, List<string>> assetTypes = new()
    {
        { "Prefabs", new() { ".prefab"} },
        { "Animations", new() { ".anim" } },
        { "Images", new() { ".png", ".jpeg" } }
    };

    private void Awake()
    {
        InitializeFields();
    }

    [MenuItem("Window/Project Organizer Tool")]
    public static void ShowWindow()
    {
        var window = GetWindow<ProjectOrganizerWindow>();

        var guiContent = new GUIContent
        {
            text = "Project Organizer"
        };
        window.titleContent = guiContent;
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        DrawToolbarTabs();

        EditorGUILayout.Space(20);

        if(selectedTabIndex == 0)
        {
            if(isDirty)
            {
                isDirty = false;

                UpdateAssetTypes(assetTypeNames.Length);
            }

            for(int i = 0; i < countOforganizerRows; ++i)
            {
                DrawOrganizerRow(i);
            }

            DrawAddAndRemoveControls();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if(GUILayout.Button("Organize"))
            {
                OrganizeFilesIntoFolders();
            }

        }
        else
        {
            for(int i = 0; i < countOfAssetTypeRows; ++i)
            {
                DrawAssetTypeRow(i);
            }

            DrawAddAndRemoveControls();
        }    
    }

    private void DrawToolbarTabs()
    {
        GUILayout.BeginHorizontal();
        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, tabs);
        GUILayout.EndHorizontal();
    }

    private void DrawAssetTypeRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Name");
        EditorGUI.BeginChangeCheck();
        if(assetTypeRows != null)
        {
            assetTypeRows[currentIndex].name = EditorGUILayout.TextField(assetTypeRows[currentIndex].name);
        }

        // if this is true, it means the value of the text field has been changed
        if(EditorGUI.EndChangeCheck())
        {
            isDirty = true;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("File Extension");
        EditorGUI.BeginChangeCheck();

        if(assetTypeRows != null)
        {
            assetTypeRows[currentIndex].fileExtension = EditorGUILayout.TextField(assetTypeRows[currentIndex].fileExtension);
        }

        // if this is true, it means the value of the text field has been changed
        if (EditorGUI.EndChangeCheck() && assetTypes.ContainsKey(assetTypeRows[currentIndex].name))
        {
            isDirty = true;
        }
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
    }

    private void InitializeFields()
    {
        foreach(string key in assetTypes.Keys)
        {
            totalNumberOfFileExtensions += assetTypes[key].Count;
        }

        countOfAssetTypeRows = totalNumberOfFileExtensions;

        assetTypeRows = new List<AssetTypeRow>();

        assetTypeNames = new string[totalNumberOfFileExtensions];
        assetTypes.Keys.CopyTo(assetTypeNames, 0);

        for(int i = 0; i < totalNumberOfFileExtensions; i++)
        {
            string key = assetTypeNames[i];

            if(key != null)
            {
                int numberOfFileExtensionsForAssetType = assetTypes[key].Count;

                for(int j = 0; j < numberOfFileExtensionsForAssetType; j++)
                {
                    assetTypeRows.Add(new AssetTypeRow()
                    {
                        name = assetTypeNames[i],
                        fileExtension = assetTypes[assetTypeNames[i]] [j]
                    });
                }
            }
        }

        countOforganizerRows = assetTypes.Keys.Count;
        organizerRows = new List<OrganizerRow>();

        for(int i = 0; i < countOforganizerRows; i++)
        {
            organizerRows.Add(new OrganizerRow()
            {
                selectedOptionIndex = i,
                folderPath = "Assets/" + assetTypeNames[i]
            });
        }
    }

    private void UpdateAssetTypes(int currentIndex)
    {
        assetTypes.Add(assetTypeRows[currentIndex].name, new List<string>() { });

        assetTypes[assetTypeRows[currentIndex].name].Add(
            assetTypeRows[currentIndex].fileExtension);

        totalNumberOfFileExtensions = 0;
        foreach (string key in assetTypes.Keys)
        {
            totalNumberOfFileExtensions += assetTypes[key].Count;
        }

        assetTypeNames = new string[totalNumberOfFileExtensions - 1];
        assetTypes.Keys.CopyTo(assetTypeNames, 0);
    }

    private void DrawOrganizerRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Asset Type");
        EditorGUI.BeginChangeCheck();

        organizerRows[currentIndex].selectedOptionIndex = EditorGUILayout.Popup(
            "",
            organizerRows[currentIndex].selectedOptionIndex,
            assetTypeNames);

        if(EditorGUI.EndChangeCheck())
        {
            organizerRows[currentIndex].folderPath = "Assets/" +
                assetTypeNames[organizerRows[currentIndex].selectedOptionIndex];
        }

        GUILayout.EndVertical();
        EditorGUILayout.Space();

        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path to Folder");

        organizerRows[currentIndex].folderPath = EditorGUILayout.TextField(organizerRows[currentIndex].folderPath);

        GUILayout.EndVertical();
        EditorGUILayout.Space();

        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("Select Folder");
        EditorGUI.BeginChangeCheck();

        organizerRows[currentIndex].obj =
            EditorGUILayout.ObjectField(
                organizerRows[currentIndex].obj,
                typeof(UnityEditor.DefaultAsset),
                true);

        if (EditorGUI.EndChangeCheck())
        {
            organizerRows[currentIndex].folderPath =
                "Assets/" + organizerRows[currentIndex].obj.name;
        }

        GUILayout.EndVertical();
        EditorGUILayout.Space();

        GUILayout.EndHorizontal();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DrawAddAndRemoveControls()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.Space(30f);

        GUIContent add = new();
        add.text = "+";

        if(GUILayout.Button(add))
        {
            if(selectedTabIndex == 0)
            {
                countOforganizerRows++;
                organizerRows.Add(new OrganizerRow());
            }
            else
            {
                countOfAssetTypeRows++;
                assetTypeRows.Add(new AssetTypeRow());
            }
        }

        GUIContent remove = new();
        remove.text = "-";

        if (GUILayout.Button(remove))
        {
            if (selectedTabIndex == 0)
            {
                countOforganizerRows--;
                organizerRows.RemoveAt(organizerRows.Count - 1);
            }
            else
            {
                countOfAssetTypeRows--;
                assetTypeRows.RemoveAt(assetTypeRows.Count - 1);
            }
        }

        GUILayout.EndHorizontal();
    }

    private void OrganizeFilesIntoFolders()
    {
        Dictionary<string, string> fileExtentionsToFolderPathsMap = new();

        foreach (string assetTypeName in assetTypes.Keys)
        {
            for(int i = 0; i < assetTypes[assetTypeName].Count; i++)
            {
                string folderPath = "Assets/" + assetTypeName + "/";
                fileExtentionsToFolderPathsMap.Add(
                    assetTypes[assetTypeName][i],
                    folderPath);
            }
        }

        DirectoryInfo directory = new("Assets/");
        foreach(string fileExtension in fileExtentionsToFolderPathsMap.Keys)
        {
            string query = "*" + fileExtension;

            FileInfo[] fileInfos = directory.GetFiles(query);

            foreach(FileInfo fileInfo in fileInfos)
            {
                string filePath =
                    fileExtentionsToFolderPathsMap[fileExtension] + fileInfo.Name;

                AssetDatabase.MoveAsset("Assets/" + fileInfo.Name, filePath);
            }
        }
    }
}
