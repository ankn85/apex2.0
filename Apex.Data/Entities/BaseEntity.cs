namespace Apex.Data.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public string GetObjectFullName()
        {
            return this.GetType().FullName;
        }
    }
}