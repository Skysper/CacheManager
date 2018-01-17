namespace CacheManager.Model
{
    public class CacheKey : BaseEntity
    {
        public CacheKey() { }

        public CacheKey(int id, string name, string description, int appId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.AppId = appId;
        }

        //key name
        public string Name { set; get; }

        //key detail info
        public string Description { set; get; }

        //app id
        public int AppId { set; get; }

    }
}