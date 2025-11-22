namespace Gateway_Yarp_Solution.Models
{
    public class RedisJwtInfo
    {
        public string Token { get; set; }
        public string Permission { get; set; }
        public string Device { get; set; }
        public DateTime LoginTime { get; set; }
        public string IpAddress { get; set; }
        public string ConnectionStringDB { get; set; }
    }
}
