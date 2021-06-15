using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace com.lexus.th
{
    public class Utility
    {
        static string WS_DATETIME_FORMAT = "dd/MM/yyyy HH:mm:ss";
        static string WS_TIME_FORMAT = "HH:mm:ss";
        static string WS_TIME_FORMAT2 = "HH:mm";
        static string WS_DATE_FORMAT = "dd/MM/yyyy";

        public static string SAMPLE_KEY = "gCjK+DZ/GCYbKIGiAt1qCA==";
        public static string SAMPLE_IV = "47l5QsSe1POo31adQ/u7nQ==";
        //public static string SAMPLE_KEY = "MYUZV1SSQphTEOAc";
        //public static string SAMPLE_IV = "MYUZV1SSQphTEOAc";

        public static string convertToDateTimeServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
            {
                return "";
            }
            else
            {
                return result.ToString(WS_DATETIME_FORMAT);
            }
        }

        public static string convertToTimeServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
            {
                return "";
            }
            else
            {
                return result.ToString(WS_TIME_FORMAT);
            }
        }
        public static string convertToTime2ServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
            {
                return "";
            }
            else
            {
                return result.ToString(WS_TIME_FORMAT2);
            }
        }
        public static string convertToDateServiceFormatString(string input)
        {
            DateTime result;

            bool isPass = DateTime.TryParse(input, out result);

            if (!isPass)
            {
                return "";
            }
            else
            {
                return result.ToString(WS_DATE_FORMAT);
            }
        }

        private static Random random = new Random();
        public static string RandomCredential(int length, string charater)
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(charater, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }


        public static string Sha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //public static String ConvertImageURLToBase64(String url)
        //{
        //    string aa = string.Empty;

        //    StringBuilder _sb = new StringBuilder();

        //    Byte[] _byte = GetImage(url);

        //    _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
        //    aa = _sb.ToString();

        //    return aa;
        //}

        //public static String ConvertImageURLToBase64Path(String url)
        //{
        //    try
        //    {
        //        string aa = string.Empty;

        //        StringBuilder _sb = new StringBuilder();

        //        Byte[] _byte = GetImagePath(url);
        //        if (_byte != null)
        //        {
        //            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));
        //            aa = _sb.ToString();
        //        }
        //        else
        //        {
        //            aa = "fail";
        //        }
        //        return aa;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.ToString();
        //    }
        //}

        //public static byte[] GetImage(string url)
        //{
        //    Stream stream = null;
        //    byte[] buf;

        //    try
        //    {
        //        WebProxy myProxy = new WebProxy();
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

        //        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
        //        stream = response.GetResponseStream();

        //        using (BinaryReader br = new BinaryReader(stream))
        //        {
        //            int len = (int)(response.ContentLength);
        //            buf = br.ReadBytes(len);
        //            br.Close();
        //        }

        //        stream.Close();
        //        response.Close();
        //    }
        //    catch (Exception exp)
        //    {
        //        buf = null;
        //    }

        //    return (buf);
        //}

        //public static byte[] GetImagePath(string url)
        //{
        //    //Stream stream = null;
        //    byte[] buf = new byte[0];
        //    Aes aes = new Aes(Utility.SAMPLE_KEY, Utility.SAMPLE_IV);
        //    try
        //    {
        //        //string username = WebConfigurationManager.AppSettings["usernamePath"];
        //        //string password = WebConfigurationManager.AppSettings["passwordPath"];
        //        string username = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["usernamePath"]);
        //        string password = aes.DecryptFromBase64String(WebConfigurationManager.AppSettings["passwordPath"]);
        //        var credentials = new UserCredentials("mbk-center", username, password);
        //        Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
        //        {
        //            // do whatever you want as this user.
        //            //File.Copy(@"D:\mbklogo2.png", @"\\mbk-dfsappsrv1\Inbound_Tour\mbklogo2.png", true);

        //            Bitmap bitmap = new Bitmap(url);
        //            buf = BitmapToByteArray(bitmap);
        //        });

        //        //Bitmap bitmap = (Bitmap)Image.FromFile(url);

        //        //Bitmap bitmap = new Bitmap(File.OpenRead(url));
        //        //buf = BitmapToByteArray(bitmap);
        //    }
        //    catch (Exception exp)
        //    {
        //        buf = null;
        //    }

        //    return (buf);
        //}

        //public static byte[] BitmapToByteArray(Bitmap bitmap)
        //{
        //    MemoryStream ms = null;
        //    byte[] byteImage = new byte[0];
        //    try
        //    {
        //        ms = new MemoryStream();
        //        bitmap.Save(ms, bitmap.RawFormat);

        //        byteImage = new Byte[ms.Length];
        //        byteImage = ms.ToArray();


        //        //bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
        //        //int numbytes = bmpdata.Stride * bitmap.Height;
        //        //byte[] bytedata = new byte[numbytes];
        //        //IntPtr ptr = bmpdata.Scan0;

        //        //Marshal.Copy(ptr, bytedata, 0, numbytes);

        //        //return bytedata;
        //    }
        //    catch (Exception ex)
        //    {
        //        byteImage = new byte[0];
        //    }
        //    finally
        //    {
        //        //if (bmpdata != null)
        //        //    bitmap.UnlockBits(bmpdata);
        //    }
        //    return byteImage;
        //}

        //public static byte[] ImageToByte(Image img)
        //{
        //    ImageConverter converter = new ImageConverter();
        //    return (byte[])converter.ConvertTo(img, typeof(byte[]));
        //}

        //public static string ChangeLogTableName(string input)
        //{
        //    string new_name = string.Empty;

        //    return new_name;
        //}

        //public static string GetMiMeType(string url)
        //{
        //    string mime = string.Empty;

        //    //var split = url.Split('.');
        //    //mime = split.Last();
        //    if (url != "")
        //    {
        //        mime = MimeMapping.GetMimeMapping(url);
        //    }
        //    else
        //    {
        //        mime = "";
        //    }

        //    return mime;
        //}
    }
}