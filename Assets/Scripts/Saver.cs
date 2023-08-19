using System;
using System.IO;
using UnityEngine;

namespace TowerDefense
{
    [Serializable]
    internal class Saver<T>
    {
        public static bool IsSave;
        public static void TryLoad(string filename, ref T data)
        {
            var path = FileHandler.Path(filename);

            if (File.Exists(path))
            {
                var dataString = File.ReadAllText(path);
                var saver = JsonUtility.FromJson<Saver<T>>(dataString);
                data = saver.data;
                IsSave = true;
            }
            else
            { 
                IsSave = false; 
            }
        }
        internal static void Save(string filename, T data)
        {
            //Debug.Log($"saving to {FileHandler.Path(filename)}");
            var wrapper = new Saver<T> { data = data };
            var dataString = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FileHandler.Path(filename), dataString);
        }
        public T data;
    }
    public static class FileHandler
    {
        public static string Path(string filename)
        {
            return $"{Application.persistentDataPath}/{filename}";
        }
        public static void Reset(string filename)
        {
            var path = Path(filename);
            if (File.Exists(path))
            {
                File.Delete(path);
                if (MapCompletion.Instance != null) MapCompletion.Instance.ResetData();
            }
        }

        internal static bool HasFile(string filename)
        {
            return File.Exists(Path(filename));
        }
    }
}