namespace EX02_MultipleImplementation
{
    public interface IPayment
    {
        public string PaymentType();
        public void Credit();
    }

    public class AAAPayment : IPayment
    {
        public string PaymentType()
        {
            return "AAA";
        }

        public void Credit()
        {
        }

        public void Refund()
        {
        }
    }

    public class BBBPayment : IPayment
    {
        public string PaymentType()
        {
            return "BBB";
        }

        public void Credit()
        {
        }
    }
}
