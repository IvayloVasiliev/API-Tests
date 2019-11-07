using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace POMHomework.WebServices
{
    public static class Serialize
    {
        public static string ToJson(this User self) => JsonConvert
            .SerializeObject(self, Converter.Settings);

        public static string ToJson(this Household self) => JsonConvert
            .SerializeObject(self, Converter.Settings);

        public static string ToJson(this Book self) => JsonConvert
            .SerializeObject(self, Converter.Settings);

        public static string ToJson(this Wishlist self) => JsonConvert
            .SerializeObject(self, Converter.Settings);
    }
}
