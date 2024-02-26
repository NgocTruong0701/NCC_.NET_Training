using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.WebRequestMethods;

namespace Timesheet.Constants
{
    public class ConstantFaceId
    {
        public static string ImageCheckInPath { get; set; } = "https://checkin.nccsoft.vn/v1/upload-image?pathValue=";
        public static string baseAddress { get; set; } = "https://checkin.nccsoft.vn/v1/";
        public static string securityCode { get; set; } = "GHstHchTdpn9L83e";
    }
}
