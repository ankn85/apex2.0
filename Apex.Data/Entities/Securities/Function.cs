using System.Collections.Generic;

namespace Apex.Data.Entities.Securities
{
    public class Function : BaseEntity
    {
        public string Name { get; set; }

        public int Priority { get; set; }

        public bool Enabled { get; set; }

        public virtual ICollection<Function> SubFunctions { get; set; }

        public int? ParentId { get; set; }

        public virtual Function ParentFunction { get; set; }

         //public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}
