namespace SeekQ.Identity.Application.VerificationCode.Commands
{
    public class CheckCodeParams
    {
        public string PhoneOrEmail { get; set; }
        public string CodeToVerify { get; set; }
    }
}
