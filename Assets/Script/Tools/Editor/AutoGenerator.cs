using System.IO;
using UnityEditor;
using UnityEngine;

namespace TsingPigSDK
{

    public class AutoGenerator : Editor
    {
        [MenuItem("�ҵĹ���/AAд��Str_Def #W")]
        public static void AddressableAutoGen()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject != null && selectedObject is GameObject)
            {
                GameObject prefabObj = (GameObject)selectedObject;
                string objectName = prefabObj.name;

                string codeLine = $"public const string {objectName}_DATA_PATH = \"{objectName}\";";

                string scriptPath = "Assets/Script/Configs/Str_Def.cs";

                bool exist = true;

                if (!File.Exists(scriptPath))
                {
                    exist = false;
                }

                if (!exist)
                {
                    using (StreamWriter writer = File.CreateText(scriptPath))
                    {
                        writer.WriteLine("public static class Str_Def");
                        writer.WriteLine("{");
                    }
                }

                using (StreamWriter writer = File.AppendText(scriptPath))
                {
                    writer.WriteLine($"    {codeLine}");
                }

                if (!exist)
                {
                    using (StreamWriter writer = File.AppendText(scriptPath))
                    {
                        writer.WriteLine("}");
                    }
                }

                Log.Info($"���Ӵ���: {codeLine.ToUpper()} to {scriptPath}");

                AssetDatabase.Refresh();

            }
            else
            {
                Log.Warning("��ѡ��һ��Ԥ���������ɴ���");
            }
        }

    }
}
