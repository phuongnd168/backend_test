using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLS.BHL.Infra.App.Domain.Entities
{
    [Table("ForgotPassword")]

    public class ForgotPasswordEntity : BaseEntity
    {
        public string Otp { get; set; }
        public DateTime ExpiredOtpAt { get; set; }
        public string? resetToken { get; set; }
        public DateTime? ExpiredResetTokenAt { get; set; }
        public UserEntity User { get; set; }
        public int UserId { get; set; }
    }
}
