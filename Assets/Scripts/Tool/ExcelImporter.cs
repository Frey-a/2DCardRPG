using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEngine;

public class ExcelImporter : EditorWindow
{
    private string excelFolderPath = "Assets/Excels";
    private string savePath = "Assets/ScriptableObjects/Database.asset";

    [MenuItem("Tools/Excel → ScriptableObject(DB)")]
    public static void ShowWindow()
    {
        GetWindow<ExcelImporter>("Excel Importer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel Import Settings", EditorStyles.boldLabel);
        excelFolderPath = EditorGUILayout.TextField("Excel Folder", excelFolderPath);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        if (GUILayout.Button("Import All Excel Files"))
        {
            ImportAll();
        }
    }

    private void ImportAll()
    {
        SODatabase db = AssetDatabase.LoadAssetAtPath<SODatabase>(savePath);
        if (db == null)
        {
            db = ScriptableObject.CreateInstance<SODatabase>();
            AssetDatabase.CreateAsset(db, savePath);
        }

        string[] excelFiles = Directory.GetFiles(excelFolderPath, "*.xlsx");
        string mappingPath = "Assets/Excels/ExcelMapping.json";
        ExcelMapping mapping = JsonUtility.FromJson<ExcelMapping>(File.ReadAllText(mappingPath));

        foreach (string file in excelFiles)
        {
            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                string fileName = Path.GetFileName(file);
                string dtoName = mapping.GetDTO(fileName);

                if (dtoName == nameof(CharData))
                    db.chars = ParseTable<CharData>(reader);
                else if (dtoName == nameof(CardData))
                    db.cards = ParseTable<CardData>(reader);
            }
        }

        EditorUtility.SetDirty(db);
        AssetDatabase.SaveAssets();
        Debug.Log("DB화 성공");
    }

    private List<T> ParseTable<T>(IExcelDataReader reader) where T : new()
    {
        List<T> list = new List<T>();
        FieldInfo[] fields = typeof(T).GetFields();

        // 첫 행은 헤더 스킵
        reader.Read();

        while (reader.Read())
        {
            T obj = new T();

            for (int i = 0; i < fields.Length; i++)
            {
                if (i >= reader.FieldCount)
                { 
                    break;
                }

                object val = reader.GetValue(i);
                if (val == null)
                {
                    continue;
                }

                try
                {
                    object converted = Convert.ChangeType(val, fields[i].FieldType);
                    fields[i].SetValue(obj, converted);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"[{typeof(T).Name}] 변환 실패: {fields[i].Name}, 값: {val}, 에러: {e.Message}");
                }
            }

            list.Add(obj);
        }

        return list;
    }
}

[System.Serializable]
public class ExcelMapping
{
    public List<ExcelMap> maps;

    public string GetDTO(string fileName)
    {
        ExcelMap map = maps.Find(x => x.fileName == fileName);
        return map != null ? map.dtoName : null;
    }
}

[System.Serializable]
public class ExcelMap
{
    public string fileName; // "Char.xlsx"
    public string dtoName;  // "CharData"
}