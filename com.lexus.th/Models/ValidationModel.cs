using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace com.lexus.th
{
    public class ValidationModel
    {
        public bool Success { get; set; }
        public int InvalidCode { get; set; }
        public string InvalidMessage { get; set; }
        public string InvalidStoreLink { get; set; }
        public string InvalidVersion { get; set; }

        public enum InvalidState
        {
            E301, E302, E303, E304, E305, E306, E307, E308, E309, 
            E401, E402, E403, E404, E405, E406, E407, E408, E409, E410, E411, E412, E413, E414, E415, E416,
            E501, E502, E503, E504, E505, E506, E507, E508,
            E601,
            E701, E702, E703, E704, E705, E706, E707, E708,
            E801, E802, E803, E804, E805, E806, E807, E808, E809, E810, E811, E812, E813, E814, E815, E816, E817, E818, E819, E820,
            E901, E902, E903, E904, E905 , E906
        }
        public static int GetInvalidCode(InvalidState state)
        {
            switch (state)
            {
                #region Test Drive
                case InvalidState.E301:
                    return 301;
                case InvalidState.E302:
                    return 302;
                case InvalidState.E303:
                    return 303;
                case InvalidState.E304:
                    return 304;
                case InvalidState.E305:
                    return 305;
                case InvalidState.E306:
                    return 306;
                case InvalidState.E307:
                    return 307;
                case InvalidState.E308:
                    return 308;
                case InvalidState.E309:
                    return 309;
                #endregion

                #region Service Appointment
                case InvalidState.E401:
                    return 401;
                case InvalidState.E402:
                    return 402;
                case InvalidState.E403:
                    return 403;
                case InvalidState.E404:
                    return 404;
                case InvalidState.E405:
                    return 405;
                case InvalidState.E406:
                    return 406;
                case InvalidState.E407:
                    return 407;
                case InvalidState.E408:
                    return 408;
                case InvalidState.E409:
                    return 409;
                case InvalidState.E410:
                    return 410;
                case InvalidState.E411:
                    return 411;
                case InvalidState.E412:
                    return 412;
                case InvalidState.E413:
                    return 413;
                case InvalidState.E414:
                    return 414;
                #endregion




                case InvalidState.E501:
                    return 501;
                case InvalidState.E502:
                    return 502;
                case InvalidState.E503:
                    return 503;
                case InvalidState.E504:
                    return 504;
                case InvalidState.E505:
                    return 505;
                case InvalidState.E506:
                    return 506;
                case InvalidState.E507:
                    return 507;
                case InvalidState.E508:
                    return 508;
                case InvalidState.E601:
                    return 601;
                case InvalidState.E701:
                    return 701;
                case InvalidState.E702:
                    return 702;
                case InvalidState.E703:
                    return 703;
                case InvalidState.E704:
                    return 704;
                case InvalidState.E705:
                    return 705;
                case InvalidState.E706:
                    return 706;
                case InvalidState.E801:
                    return 801;
                case InvalidState.E802:
                    return 802;
                case InvalidState.E803:
                    return 803;
                case InvalidState.E804:
                    return 804;
                case InvalidState.E805:
                    return 805;
                case InvalidState.E806:
                    return 806;
                case InvalidState.E807:
                    return 807;
                case InvalidState.E808:
                    return 808;
                case InvalidState.E809:
                    return 809;
                case InvalidState.E810:
                    return 810;
                case InvalidState.E811:
                    return 811;
                case InvalidState.E812:
                    return 812;
                case InvalidState.E813:
                    return 813;
                case InvalidState.E814:
                    return 814;
                case InvalidState.E815:
                    return 815;
                case InvalidState.E816:
                    return 816;
                case InvalidState.E817:
                    return 817;
                case InvalidState.E818:
                    return 818;
                case InvalidState.E819:
                    return 819;
                case InvalidState.E820:
                    return 820;

                case InvalidState.E901:
                    return 901;
                case InvalidState.E902:
                    return 902;
                case InvalidState.E903:
                    return 903;
                case InvalidState.E904:
                    return 904;
                case InvalidState.E905:
                    return 905;
                case InvalidState.E906:
                    return 906;
                default:
                    return 0;
            }
        }
        public static string GetInvalidMessage(InvalidState state)
        {
            switch (state)
            {
                case InvalidState.E501:
                    return "Not enough privilege to redeem";
                case InvalidState.E502:
                    return "Your member is expired";
                case InvalidState.E503:
                    return "Item is out of stock";
                case InvalidState.E504:
                    return "You've already redeemed this item";
                case InvalidState.E505:
                    return "Privilege is expired";
                case InvalidState.E506:
                    return "Privilege not found";
                case InvalidState.E507:
                    return "Event not found";
                case InvalidState.E508:
                    return "News not found";
                //506 Privilege not found
                case InvalidState.E601:
                    return "Invalid shop verified code";
                case InvalidState.E701:
                    return "Event registration ends";
                case InvalidState.E702:
                    return "Participants reach maximum number.";
                case InvalidState.E703:
                    return "You've already register this event";
                case InvalidState.E704:
                    return "Your member is expired";
                case InvalidState.E801:
                    return "Member Card ID not found";
                case InvalidState.E802:
                    return "Phone number is incorrect";
                case InvalidState.E803:
                    return "";
                case InvalidState.E901:
                    return "OTP is incorrect";
                case InvalidState.E902:
                    return "OTP is expired";
                case InvalidState.E903:
                    return "service maintenance";
                case InvalidState.E904:
                    return "App have new version. You must update app before";
                case InvalidState.E905:
                    return "App have new version.";

                default:
                    return "";
            }
        }

        public static async Task<string> GetInvalidMessageNew(InvalidState state, string lang)
        {
            string message_text = string.Empty;
            int message_code = 0;
            MessageService ms = new MessageService();
            switch (state)
            {

                #region Test Drive
                case InvalidState.E301:
                    message_code = 21039001;// return Please insert name.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E302:
                    message_code = 21039002;// return Please insert surname.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E303:
                    message_code = 21039003;// return Please insert contact number.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E304:
                    message_code = 21039004;// return Please insert contact number 10 digit.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E305:
                    message_code = 21039005;// return Please insert email.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E306:
                    message_code = 21039006;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E307:
                    message_code = 21039007;// return Please insert preferred model.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E308:
                    message_code = 21039008;// return Please insert preferred dealer.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E309:
                    message_code = 21039009;// return Please insert car purchase plan.
                    return await ms.GetMessagetext(message_code, lang);
                #endregion

                #region Service Appointment
                case InvalidState.E401:
                    message_code = 21036001;// return Please insert name.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E402:
                    message_code = 21036002;// return Please insert surname.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E403:
                    message_code = 21036003;// return Please insert contact number.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E404:
                    message_code = 21036004;// return Please insert contact number 10 digit.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E405:
                    message_code = 21036005;// return Please insert email.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E406:
                    message_code = 21036006;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E407:
                    message_code = 21036007;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E408:
                    message_code = 21036008;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E409:
                    message_code = 21036009;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E410:
                    message_code = 21036010;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E411:
                    message_code = 21036011;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E412:
                    message_code = 21036012;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E413:
                    message_code = 21036013;// return Please insert correct email format.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E414:
                    message_code = 21036014;// return please share location or insert location detail.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E415:
                    message_code = 21036015;// return please choose pickup date.
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E416:
                    message_code = 21036016;// return please choose pickup time.
                    return await ms.GetMessagetext(message_code, lang);

                #endregion


                case InvalidState.E501:
                    message_code = 300501; //return "Not enough privilege to redeem";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E502:
                    message_code = 300502; //return "Your member is expired";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E503:
                    message_code = 300503; //return "Item is out of stock";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E504:
                    message_code = 300504; //return "You've already redeemed this item";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E505:
                    message_code = 300505; //return "Privilege is expired";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E506:
                    message_code = 300506; //return "Privilege not found";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E507:
                    message_code = 300507; //return "Event not found";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E508:
                    message_code = 300508; //return "News not found";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E601:
                    message_code = 300601; //return "Invalid shop verified code";
                    return await ms.GetMessagetext(message_code, lang);

                
                case InvalidState.E701:
                    message_code = 300701; //return "Event registration ends";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E702:
                    message_code = 300702; //return "Participants reach maximum number.";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E703:
                    message_code = 300703; //return "Not enough VIN for register";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E704:
                    message_code = 300704; //return "Your member is expired";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E705:
                    message_code = 21023001; //return "Please Login";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E706:
                    message_code = 21023002; //return "VIN not enough for register this event.";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E707:
                    message_code = 21023003; //return "Sorry, your membership is not eligible for this event.";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E708:
                    message_code = 21023004; //return "Please complete your answer.";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E801:
                    message_code = 300801; //return "Member Card ID not found";
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E802:
                    message_code = 21002001; //ไม่พบหมายเลขโทรศัพท์นี้ในระบบ กรุณาตรวจสอบหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                
                case InvalidState.E803:
                    message_code = 21007001; //หมายเลขโทรศัพท์นี้ได้รับการลงทะเบียนแล้ว กรุณาลงทะเบียนด้วยหมายเลขอื่นหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E804:
                    message_code = 21007002; //ไม่พบหมายเลขตัวถังรถยนต์ในระบบ กรุณาตรวจสอบหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E805:
                    message_code = 21007003; //หมายเลขตัวถังนี้ได้รับการลงทะเบียนแล้ว กรุณาติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E806:
                    message_code = 21007004; //ไม่พบหมายเลขบัตรประชาชนของท่านในระบบ กรุณาตรวจสอบหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E807:
                    message_code = 21007005; //หมายเลขบัตรประชาชนนี้ได้รับการลงทะเบียนแล้ว กรุณาตรวจสอบหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E808:
                    message_code = 21007006; //ไม่สามารถลงทะเบียนได้ เนื่องจากรถยนต์เลกซัสของท่านมีอายุเกิน 10 ปี
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E809:
                    message_code = 21008001; //หมายเลขโทรศัพท์นี้ได้รับการลงทะเบียนแล้ว กรุณาลงทะเบียนด้วยหมายเลขอื่นหรือติดต่อ LEXUS Elite Club call center 02-305-6799
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E810:
                    message_code = 21007001; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E811:
                    message_code = 21007002; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E812:
                    message_code = 21007003; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E813:
                    message_code = 21007004; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E814:
                    message_code = 21007005; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E815:
                    message_code = 21007006; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E816:
                    message_code = 21007007; //
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E817:
                    message_code = 21007008; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E818:
                    message_code = 21007009; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E819:
                    message_code = 21007010; //
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E820:
                    message_code = 21007011; //
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E901:
                    message_code = 21003001; //รหัส OTP ไม่ถูกต้อง กรุณาตรวจสอบอีกครั้ง
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E902:
                    message_code = 21003002; //รหัส OTP หมดอายุ กรุณากดขอรหัสใหม่อีกครั้ง
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E903:
                    message_code = 300903; //return "service maintenance";
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E904:
                    message_code = 300904; //return "App have new version. You must update app before";
                    return await ms.GetMessagetext(message_code, lang);
                case InvalidState.E905:
                    message_code = 300905; //return "App have new version.";
                    return await ms.GetMessagetext(message_code, lang);

                case InvalidState.E906:
                    message_code = 21039010; //return "App have new version. You must update app before";
                    return await ms.GetMessagetext(message_code, lang);

                default:
                    return "";
            }
        }

        public static string GetInvalidMessageNewReplace(InvalidState state, string lang, string platform)
        {
            string message_text = string.Empty;
            int message_code = 0;
            MessageService ms = new MessageService();
            switch (state)
            {
                case InvalidState.E904:
                    message_code = 300904; //return "App have new version. You must update app before";
                    return ms.GetMessagetextReplace(message_code, lang, platform.ToLower());

                default:
                    return "";
            }
        }
    }
}