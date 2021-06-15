using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th.web
{
    public class ImageService
    {
        public static string Split1(string imagePath)
        {
            string value = "";
            try
            {
                List<string> split = imagePath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (split.Count > 0)
                {
                    value = split[split.Count - 1];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public static string Split2(string imagePath)
        {
            string value = "";
            try
            {
                List<string> split = imagePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (split.Count > 1)
                {
                    value = split[split.Count - 2] + '/' + split[split.Count - 1];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public static string Split3(string imagePath)
        {
            string value = "";
            try
            {
                List<string> split = imagePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (split.Count > 1)
                {
                    value = split[split.Count - 3] + '/' + split[split.Count - 2] + '/' + split[split.Count - 1];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
        public static string Split4(string imagePath)
        {
            string value = "";
            try
            {
                List<string> split = imagePath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (split.Count > 1)
                {
                    value = split[split.Count - 2] + '/' + Split1(split[split.Count - 1]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return value;
        }
    }
}