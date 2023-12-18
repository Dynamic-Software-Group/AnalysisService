using System.Collections.Generic;
using System.Diagnostics;

namespace AnalysisService;

public class Server
{
    protected int update_frequency = 10000; //In milliseconds
    internal Communication comm_service;
    
    public Server(int update_freq)
    {
        update_frequency = update_freq;
        comm_service = new Communication();
    }

    public void start_thread()
    {
        List<string> users_jsons = comm_service.GetCategory("idx:users");
        List<string> business_jsons = comm_service.GetCategory("idx.businesses");

        List<Datamanager.RedisData> user_data = Datamanager.DeserializeJsons<Datamanager.RedisData>(users_jsons);
        List<Datamanager.business> business_data = Datamanager.DeserializeJsons<Datamanager.business>(business_jsons);

        Dictionary<float, Datamanager.business> id_business = Datamanager.GetIDtoBusinesses(business_data);
        
        Dictionary<Datamanager.business, List<Datamanager.RedisData>> business_oriented_data = Datamanager.SortByBusiness(user_data, id_business);


        List<Calculations.AnalysisInfo> ainfo = Calculations.CreateAnalysisInfo(business_oriented_data);

        List<string> serialized_ainfo = Datamanager.SerializeJsons<Calculations.AnalysisInfo>(ainfo);
        
        comm_service.AddToCategory(serialized_ainfo);
        
        System.Threading.Thread.Sleep(update_frequency);
    }
}