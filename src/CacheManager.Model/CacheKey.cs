namespace CacheManager.Model
{
    public class CacheKey:BaseEntity
    {

        //key name
        public string Name { set; get; }

        //key detail info
        public string Description { set; get; }

        //app id
        public int AppId{set;get;}

    }
}