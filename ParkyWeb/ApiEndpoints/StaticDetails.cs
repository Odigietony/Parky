using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.ApiEndpoints
{
    public static class StaticDetails
    {
        public static string ApiBaseUrl = "https://localhost:44306/";
        public static string NationalParkApiUrl = ApiBaseUrl + "api/v1/nationalparks";
        public static string TarilsApiUrl = ApiBaseUrl + "api/v1/trails";
    }
}
