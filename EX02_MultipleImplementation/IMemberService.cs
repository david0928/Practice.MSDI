namespace EX02_MultipleImplementation
{
    interface IMemberService
    {
        public string GetPaidType();
    }

    public class MemberService : IMemberService
    {
        public string GetPaidType()
        {
            return "AAA";
        }
    }
}
