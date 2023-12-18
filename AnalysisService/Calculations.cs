using System.Collections.Generic;
using BOD = System.Collections.Generic.Dictionary<AnalysisService.Datamanager.business, System.Collections.Generic.List<AnalysisService.Datamanager.RedisData>>;
using BODPair = System.Collections.Generic.KeyValuePair<AnalysisService.Datamanager.business, System.Collections.Generic.List<AnalysisService.Datamanager.RedisData>>;

namespace AnalysisService
{
    class Calculations
    {

        public record class AnalysisInfo
        (
            Datamanager.business business,
            float average_point_per_user
        );

        public static List<AnalysisInfo> CreateAnalysisInfo(BOD bod)
        {
            List<AnalysisInfo> ainfo = new List<AnalysisInfo>();
            AnalysisInfo info;
            foreach (BODPair bpair in bod)
            {
                info = new AnalysisInfo(
                    bpair.Key,
                    AvgPointsPerUser(bpair)
                );
                ainfo.Add(info);
            }

            return ainfo;
        }
        
        public static float AvgPointsPerUser(BODPair bpair)
        {
            float total_points = 0;
            foreach (Datamanager.RedisData r in bpair.Value)
            {
                foreach (Datamanager.pointholder p in r.points)
                {
                    if (p.business_id == bpair.Key.id)
                    {
                        total_points += p.points;
                        break;
                    }
                }
            }
            if (bpair.Value.Count == 0)
            {
                return 0;
            }
            else
            {
                return total_points / bpair.Value.Count;
            }
        }
            
            
        
    }
}
