using System;
using System.Collections.Generic;

namespace AnalysisService
{
    class Program
    {

        static void Main(string[] args)
        {
            Datamanager.RedisData rdata = new Datamanager.RedisData(
                new Datamanager.user(
                    "testuser",
                    "testuser@gmail.com",
                    "shouldbeencryptedpassword",
                    false,
                    new List<string>() { "ROLE_USER" },
                    "jwtstring"
                ),
                new List<Datamanager.business>()
                {
                            new Datamanager.business(
                                "testbusiness",
                                1
                            )
                },
                new List<Datamanager.pointholder>() {
                            new Datamanager.pointholder(
                                1,
                                0
                            )
                }
            );
            string json = Datamanager.SerializeJson(rdata);
            rdata = Datamanager.DeserializeJson(json);
            Console.WriteLine(json);
        }
    }
}
