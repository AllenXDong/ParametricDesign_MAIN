using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Utils
{
    class FileUtil
    {
        public static void Write(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write("Hello World!!!!");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        public static void CopyDirectory(string sourcePath, string targetPath)
        {
            //创建所有新目录
            targetPath = targetPath + "\\" + Path.GetFileName(sourcePath);
            Directory.CreateDirectory(targetPath);
            //创建所有新目录
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }

                //复制所有文件 & 保持文件名和路径一致
                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                }
        }
    }
}
