namespace BoilerMVC.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class InRole
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int UserId { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    
    }
}
