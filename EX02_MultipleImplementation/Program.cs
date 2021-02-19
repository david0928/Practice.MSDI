using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EX02_MultipleImplementation
{
    [TestFixture]
    public class Program
    {
        /// <summary>
        /// 1. 一個 Interface 多個 Class 實作
        /// 2. 使用 IEnumerable 方式取得各別實作
        /// Test: 一個 Interface 多個 Class 實作，不同的實作要不相等
        /// </summary>
        [Test]
        public void MultipleImplementation_IEnumerable_InstancesAreNotSame()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddSingleton<IPayment, AAAPayment>(); // 註冊 AAAPayment
            services.AddSingleton<IPayment, BBBPayment>(); // 註冊 BBBPayment

            var provider = services.BuildServiceProvider();

            // Act
            var payments = provider.GetService<IEnumerable<IPayment>>();

            var paymentAAA = payments.Where(w => w.PaymentType() == "AAA").FirstOrDefault();
            var paymentBBB = payments.Where(w => w.PaymentType() == "BBB").FirstOrDefault();

            // AssertInstance
            Assert.AreNotSame(paymentAAA, paymentBBB); // 不同的 Instance
        }

        /// <summary>
        /// 1. 一個 Interface 多個 Class 實作
        /// 2. 使用 FactoryFunction 方式取得實作
        /// Test: 一個 Interface 多個 Class 實作，不同的實作要不相等
        /// </summary>
        [Test]
        public void MultipleImplementation_FactoryFunction_PaymentTypeAreEqual()
        {
            // Arrange
            var expected = "AAA";
            var services = new ServiceCollection();

            services.AddSingleton<IMemberService, MemberService>(); // 先註冊 MemberService

            services.AddSingleton<IPayment>(x =>
            {
                var memberService = x.GetRequiredService<IMemberService>();
                if (memberService.GetPaidType() == expected)
                {
                    return ActivatorUtilities.CreateInstance<AAAPayment>(x);
                }
                else
                {
                    return ActivatorUtilities.CreateInstance<BBBPayment>(x);
                }
            });

            var provider = services.BuildServiceProvider();

            // Act
            var payment = provider.GetService<IPayment>();

            // Assert
            Assert.AreEqual(payment.PaymentType(), expected);
        }
    }
}
