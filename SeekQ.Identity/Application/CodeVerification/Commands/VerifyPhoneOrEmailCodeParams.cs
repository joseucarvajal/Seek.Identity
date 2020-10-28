namespace SeekQ.Identity.Application.CodeVerification.Commands
{
    public class VerifyPhoneOrEmailCodeParams
    {
        public string PhoneOrEmail { get; set; }
        public string CodeToVerify { get; set; }
    }
}
