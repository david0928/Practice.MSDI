namespace EX01_MultipleInterfaces
{
    public interface IPayment
    {
        public string PaymentType();
        public void Credit();
    }
    public interface IPaymentAAA
    {
        public void Refund();
    }

    public class AAAPayment : IPayment, IPaymentAAA
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
