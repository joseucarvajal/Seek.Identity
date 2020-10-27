namespace SeekQ.Identity.Application.Commands.CodeVerification
{
    public class VerifyPhoneOrEmailCodeParams
    {
        public string PhoneOrEmail { get; set; }
        public string CodeToVerify { get; set; }
    }
}
