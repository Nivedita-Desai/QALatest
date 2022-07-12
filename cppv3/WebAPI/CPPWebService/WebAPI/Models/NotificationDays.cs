using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    [Table("notificationdays")]
    public class NotificationDays
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public String Id { get; set; }
        public String MailService { get; set; }
        public String Days { get; set; }

    }
}
