using System.Collections.Generic;
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

        public static string SerializeJson(RedisData data)
        {
            return JsonSerializer.Serialize(data);
        }

        public static RedisData DeserializeJson(string json)
        {
            return JsonSerializer.Deserialize<RedisData>(json);
        }
    }
}
