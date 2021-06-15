using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public class ServiceAllMasterDataModel
    {
        public bool success { get; set; }
        public MsgModel msg { get; set; }
        public string ts { get; set; }
        public _ServiceAllMasterData data { get; set; }
    }
    public class _ServiceAllMasterData
    {
        public List<Config> config { get; set; }
        public string survey_url { get; set; }
        public string term_cond_url { get; set; }
        public string term_cond_booking_url { get; set; }
        public string policy_url { get; set; }
        public string default_landmark_lat { get; set; }
        public string default_landmark_lng { get; set; }
        public string pickup_url { get; set; }
        public string dealer_appointment_url { get; set; }
    }
    public class Config
    {
        public int id { get; set; }
        public string name { get; set; }
        public string data_config { get; set; }

        public void loadDataConfig(DataRow dr)
        {
            id = int.Parse(dr["id"].ToString());
            name = dr["name"].ToString();
            data_config = dr["data_config"].ToString();

            //if (dr["name"].ToString() == "has_survey")
            //{
            //    MasterDataService masterData = new MasterDataService();
            //    data_config = masterData.checkHasSurvey().ToString();
            //}
            //else
            //{
                
            //}  
        }
    }
}