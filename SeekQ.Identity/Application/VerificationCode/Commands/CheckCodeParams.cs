using System;

namespace SeekQ.Identity.Application.VerificationCode.Commands
{
    public class CheckCodeParams
    {
        public Guid UserId { get; set; }
        public string PhoneOrEmail { get; set; }
        public string CodeToVerify { get; set; }
    }
}
