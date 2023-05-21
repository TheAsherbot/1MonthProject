using System.IO;

using UnityEngine;


namespace TheAshBot
{
    public struct SaveSystem
    {

        public enum RootSavePath
        {
            Resources,
            DataPath,
            PersistentDataPath,
            ConsoleLogPath,
            StreamingAssetsPath,
            TemporaryCachePath,
            Costum,
        }

        public enum FileType
        {
            Txt,
            Json,
            Cs,
            Costum,
        }


        public static void SaveString(string text, RootSavePath rootSavePath, string path, string name, FileType fileType, bool canOveride)
        {
            string saveFolder = GetSavePathRoot(rootSavePath) + path;
            string wholePath = saveFolder + "\\" + name + GetFileType(fileType);

            if (File.Exists(saveFolder))
            {
                SaveFile();
            }
            else
            {
                Directory.CreateDirectory(saveFolder);

                SaveFile();
            }

            void SaveFile()
            {
                // The save folder does exist
                if (File.Exists(wholePath))
                {
                    // the file does exist
                    if (!canOveride)
                    {
                        int saveNumber = 0;
                        wholePath = saveFolder + "\\" + name + "_" + saveNumber + GetFileType(fileType);
                        while (File.Exists(wholePath))
                        {
                            saveNumber++;
                            wholePath = saveFolder + "\\" + name + "_" + saveNumber + GetFileType(fileType);
                        }
                    }

                    File.WriteAllText(wholePath, text);
                    return;
                }

                File.WriteAllText(wholePath, text);
            }
        }

        public static void SaveJson(object jsonObject, RootSavePath rootSavePath, string path, string name, bool canOveride)
        {
            string text = JsonUtility.ToJson(jsonObject);

            SaveString(text, rootSavePath, path, name, FileType.Json, canOveride);
        }

        public static string LoadString(RootSavePath savePathRoot, string path, string name, FileType fileType)
        {
            string saveFolder = GetSavePathRoot(savePathRoot) + path;
            string wholePath = saveFolder + "\\" + name + GetFileType(fileType);

            if (!File.Exists(wholePath))
            {
                Debug.LogError("Can not find " + name + GetFileType(fileType) + " at " + saveFolder);
                return default;
            }

            return File.ReadAllText(wholePath);
        }

        public static TSaveObject LoadJson<TSaveObject>(RootSavePath savePathRoot, string path, string name)
        {
            string json = LoadString(savePathRoot, path, name, FileType.Json);

            return JsonUtility.FromJson<TSaveObject>(json);
        }


        private static string GetSavePathRoot(RootSavePath savePathRoot)
        {
            switch (savePathRoot)
            {
                case RootSavePath.Resources:
                    return Application.dataPath + "\\Resources\\";
                case RootSavePath.DataPath:
                    return Application.dataPath + "\\";
                case RootSavePath.PersistentDataPath:
                    return Application.persistentDataPath + "\\";
                case RootSavePath.ConsoleLogPath:
                    return Application.consoleLogPath + "\\";
                case RootSavePath.StreamingAssetsPath:
                    return Application.streamingAssetsPath + "\\";
                case RootSavePath.TemporaryCachePath:
                    return Application.temporaryCachePath + "\\";
                case RootSavePath.Costum:
                    return "";
                default:
                    return Application.persistentDataPath + "\\";
            }
        }

        private static string GetFileType(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Txt:
                    return ".txt";
                case FileType.Json:
                    return ".json";
                case FileType.Cs:
                    return ".cs";
                case FileType.Costum:
                    return "";
                default:
                    return ".txt";
            }
        }

    }
}
