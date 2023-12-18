using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;

namespace AnalysisService
{
    class Datamanager
    {

        public record class RedisData
        (
            user userinfo,
            List<business> businesses,
            List<pointholder> points
        );
        public record class user
        (
            string name,
            string email,
            string password,
            bool isBusinessOwner,
            List<string> authorities,
            string jwt
        );
        public record class business
        (
            string name,
            float id
        //TODO: Finish adding full defining characteristics of business
        );
        public record class pointholder
        (
            float business_id,
            float points
        );

        public static string SerializeJson<T>(T data)
        {
            return JsonSerializer.Serialize(data);
        }

        public static List<string> SerializeJsons<T>(List<T> data)
        {
            List<string> jsonstrings = new List<string>();
            foreach (T t in data)
            {
                jsonstrings.Add(SerializeJson<T>(t));
            }

            return jsonstrings;
        }

        public static T DeserializeJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }

        public static List<T> DeserializeJsons<T>(List<string> jsons)
        {
            List<T> data = new List<T>();
            foreach (string json in jsons)
            {
                data.Add(DeserializeJson<T>(json));
            }

            return data;
        }

        public static Dictionary<business, List<RedisData>> SortByBusiness(List<RedisData> users, Dictionary<float, business> id_businesses)
        {
            Dictionary<business, List<RedisData>> businessdata = new Dictionary<business, List<RedisData>>();
            foreach (KeyValuePair<float,business> b in id_businesses)
            {
                businessdata.Add(b.Value, new List<RedisData>());
            }
            foreach (RedisData user in users)
            {
                foreach (pointholder p in user.points)
                {
                    businessdata[id_businesses[p.business_id]].Add(user);
                }
            }

            return businessdata;
        }

        public static Dictionary<float, business> GetIDtoBusinesses(List<business> businesses)
        {
            Dictionary<float, business> id_business = new Dictionary<float, business>();
            foreach (business b in businesses)
            {
                id_business.Add(b.id, b);
            }

            return id_business;
        }
    }
}
