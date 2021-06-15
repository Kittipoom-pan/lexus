using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.lexus.th
{
    public enum EventQuestionUserType
    {
        car_owner = 1,
        representative = 2,
        follower_car_owner = 3,
        follower_representative = 4
    }

    public enum AnswerType
    {
        choice = 1,
        text = 2
    }

    public enum ProcessStatus
    {
        waiting_confirm = 1,
        confirm = 2,
        cancel = 3,
        completed = 4
    }

    public enum SystemMessage
    {
        Message_Register = 20023030,
        Message_Registered = 20023031,
        Message_Comming_Soon = 20023032,
        Message_Expire_Event = 20023033,
        Message_Register_With_New_Win = 20023034
    }

    public enum Company
    {
        Lexus = 1,
        Toyota = 2
    }

    public enum EmailStatus
    {
        success = 1,
        error = 2
    }

    public enum BookingType
    {
        repurchase = 1,
        referral = 2,
        car_booking = 3
    }
}