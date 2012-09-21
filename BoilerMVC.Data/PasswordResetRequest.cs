namespace BoilerMVC.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PasswordResetRequest
    {
        public int UserId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.Guid Code { get; set; }
    
        public virtual User User { get; set; }
    
    }
}
