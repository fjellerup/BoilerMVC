namespace BoilerMVC.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Role
    {
        public Role()
        {
            this.InRoles = new HashSet<InRole>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<InRole> InRoles { get; set; }
    
    }
}
