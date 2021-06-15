using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppLibrary.Utility
{
    public class UTLList
    {
        public static List<List<T>> SeperateList<T>(List<T> objList, int interval)
        {
            List<List<T>> list = new List<List<T>>();
            try
            {
                if (objList != null)
                {
                    int loop = (int)Math.Ceiling((objList.Count / (interval * 1.00)));
                    int startIdx = 0;
                    for (int i = 0; i < loop; i++)
                    {
                        List<T> _list = new List<T>();
                        int endIdx = startIdx + interval;
                        if (endIdx > objList.Count)
                            endIdx = objList.Count;
                        for (int j = startIdx; j < endIdx; j++)
                        {
                            _list.Add(objList[j]);
                        }
                        startIdx = endIdx;
                        list.Add(_list);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
    }
}
