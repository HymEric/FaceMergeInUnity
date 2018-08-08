using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace SCY
{

    public class Utility
    {
        public static Texture2D Base64Img(string Base64String)
        {
            Texture2D pic = new Texture2D(500, 500);
            byte[] data = System.Convert.FromBase64String(Base64String);
            pic.LoadImage(data);
            return pic;
        }

        public static string ByteArrToStr(byte[] byteArray) {
            return Encoding.UTF8.GetString(byteArray);
        }

        //public static Bitmap GetWebImage(string url)
        //{
        //    Bitmap bitmap = null;
        //    HttpWebResponse response = null;
        //    try
        //    {
        //        Uri requestUri = new Uri(url);
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
        //        request.Timeout = 0x2bf20;
        //        request.Method = "GET";
        //        response = (HttpWebResponse)request.GetResponse();
        //        bitmap = new Bitmap(response.GetResponseStream());
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception("图片读取出错!");
        //    }
        //    finally
        //    {
        //        response.Close();
        //    }
        //    return bitmap;
        //}

        public static byte[] JoinByteArr(byte[] byte1, byte[] byte2)
        {
            byte[] buffer = new byte[byte1.Length + byte2.Length];
            Stream stream = new MemoryStream();
            stream.Write(byte1, 0, byte1.Length);
            stream.Write(byte2, 0, byte2.Length);
            stream.Position = 0L;
            if (stream.Read(buffer, 0, buffer.Length) <= 0)
            {
                throw new Exception("读取错误!");
            }
            return buffer;
        }

        public static byte[] StrToByteArr(string str) {
        return    Encoding.UTF8.GetBytes(str);
        }

        public static string UnixTime(double expired = 0.0)
        {
            long num = (DateTime.Now.AddSeconds(expired).ToUniversalTime().Ticks - 0x89f7ff5f7b58000L) / 0x989680L;
            return num.ToString();
        }
        public static string ImgBase64(string path, bool isWebImg = false)
        {
            string base64Info = "";
            //if (!System.IO.File.Exists(path))
            //{
            //    throw new Exception("文件不存在!");
            //}
            if (Application.platform == RuntimePlatform.Android)
            {
                base64Info = Convert.ToBase64String(GetPictureData(path.Replace("//jar:file://", "jar:file:///").Replace("!","//!")));
               
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Debug.Log("下载开始0");
                base64Info = Convert.ToBase64String(GetPictureData(path));
                // Debug.Log("hym" + path.Replace("//jar:file://", "jar:file:///").Replace("!", "//!"));
                //Debug.Log("hym" + GetPictureData(path.Replace("//jar:file://", "jar:file:///").Replace("!", "//!")));
                //CreateOrOPenFile(Application.dataPath, "data.txt", path.Replace("//jar:file://", "jar:file:///").Replace("!", "//!") + '\n');
                //CreateOrOPenFile(Application.dataPath, "data.txt", GetPictureData(path.Replace("//jar:file://", "jar:file:///").Replace("!", "//!")).ToString() + '\n');
            }
            return base64Info;
        }

      
        public static string MyImgBase64FromBytes(byte[] bytes)
        {
            string base64Info = "";
            base64Info = Convert.ToBase64String(bytes);
            return base64Info;
        }

        //参数是图片的路径  
        public static byte[] GetPictureData(string imagePath)
        {

            //FileStream fs = new FileStream(imagePath, FileMode.Open);
            //byte[] byteData = new byte[fs.Length];
            //fs.Read(byteData, 0, byteData.Length);
            Debug.Log("下载开始1");
            byte[] mybytes = null;

            //fs.Close();
            return mybytes;
        }

        public static IEnumerator loadWWW(string url,byte[] mybyte)
        {
            using (WWW www = new WWW(url))
            {
                yield return www;
                Debug.Log("下载开始2");
                mybyte = www.bytes;
            }
                
            
           
            //if (www.isDone)
            //{
            //    Debug.Log("下载完成");
                
            //}

        }

            public static IEnumerator LoadTextureFromPath(string path , RawImage raw)
        {
                WWW www = new WWW(path);
                yield return www;
                raw.texture = www.texture;
        }

        public static IEnumerator LoadTextureFromBase64(string bese64, RawImage raw)
        {
            WWW www = new WWW(bese64);
            yield return www;
            raw.texture = www.texture;
        }

        //打开文件并写入
        public static void CreateOrOPenFile(string path, string name, string info)
        {          //路径、文件名、写入内容
            StreamWriter sw;
            FileInfo fi = new FileInfo(path + "//" + name);
            sw = fi.AppendText();        //直接重新写入，如果要在原文件后面追加内容，应用fi.AppendText()
            sw.WriteLine(info);
            sw.Close();
            sw.Dispose();
        }
    }
}
