namespace BoilerMVC.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.InRoles = new HashSet<InRole>();
        }
    
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string LastLoginIP { get; set; }
        public Nullable<System.DateTime> LastLogout { get; set; }
    
        public virtual ICollection<InRole> InRoles { get; set; }
        public virtual PasswordResetRequest PasswordResetRequest { get; set; }
    
    }
}
