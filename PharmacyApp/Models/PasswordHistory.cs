using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PharmacyApp.Models
{
    public enum PasswordChangeType { SelfReset, SuperAdminReset, AdminReset }
    public class PasswordHistory
    {
        public string ChangedBy { get; set; }
        public PasswordChangeType ChangeType { get; set; }
        public DateTime ChangeDate { get; set; }
        public string RemoteIPAddress { get; set; }
    }

    [Serializable]
    public class PasswordHistories : List<PasswordHistory>
    {
        public PasswordHistories() : base() { }
    }

}