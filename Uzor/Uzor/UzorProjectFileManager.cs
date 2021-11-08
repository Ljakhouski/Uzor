using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Uzor.Data;

namespace Uzor
{
    class UzorProjectFileManager
    {
        public static string SaveInInternalStorage(UzorData data) // returns the save path
        {
            return saveObjectInInternalStorage(data, data.Name, ".ubf");
           /* string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string fileName = data.Name + ".ubf";

            if (File.Exists(Path.Combine(folderPath, data.Name + ".ubf")))
                for (int i = 0; i < 999; i++)
                    if (File.Exists(Path.Combine(folderPath, data.Name + i.ToString() + ".ubf")))
                        continue;
                    else
                    {
                        fileName = data.Name + i.ToString() + ".ubf";
                        //data.Name = data.Name + i.ToString();
                        break;
                    }


            var savedFilePath = folderPath + "/" + fileName;

            FileStream fs = new FileStream(savedFilePath, FileMode.OpenOrCreate);
            new BinaryFormatter().Serialize(fs, data);
            fs.Dispose();

            return savedFilePath;*/
        }

        public static string SaveInInternalStorage(LongUzorData data) // returns the save path
        {
            return saveObjectInInternalStorage(data, data.Name, ".lubf");
        }

        private static string saveObjectInInternalStorage(object data, string name, string extension)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string fileName = name + extension;

            if (File.Exists(Path.Combine(folderPath, fileName)))
                for (int i = 0; i < 999; i++)
                    if (File.Exists(Path.Combine(folderPath, name + i.ToString() + extension)))
                        continue;
                    else
                    {
                        fileName = name + i.ToString() + extension;
                        //data.Name = data.Name + i.ToString();
                        break;
                    }


            var savedFilePath = folderPath + "/" + fileName;

            FileStream fs = new FileStream(savedFilePath, FileMode.OpenOrCreate);
            new BinaryFormatter().Serialize(fs, data);
            fs.Dispose();

            return savedFilePath;
        }

        public static void ReSave(UzorData data, string path)
        {
            FileStream fsr = new FileStream(path, FileMode.Truncate);
            new BinaryFormatter().Serialize(fsr, data);
            fsr.Dispose();
        }
        public static void ReSave(LongUzorData data, string path)
        {
            FileStream fsr = new FileStream(path, FileMode.Truncate);
            new BinaryFormatter().Serialize(fsr, data);
            fsr.Dispose();
        }

        public static UzorData LoadUzorData(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            UzorData d = null;
            try
            {
                d = (UzorData)formatter.Deserialize(fs);
            }
            catch(System.Runtime.Serialization.SerializationException e)
            {
                return new UzorData("SERIALIZATION ERROR", System.DateTime.Now, 20);
            }
            
            fs.Dispose();
            return d;
        }

        public static LongUzorData LoadLongUzorData(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            var d = (LongUzorData)formatter.Deserialize(fs);
            fs.Dispose();
            return d;
        }

        public static void DeleteByNameFromInternalStorage(string fileName)
        {
            File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/" + fileName);
        }

        public static void Delete(string path)
        {
            File.Delete(path);
        }
    }
}
