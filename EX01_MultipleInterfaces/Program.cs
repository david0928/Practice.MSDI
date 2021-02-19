using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace EX01_MultipleInterfaces
{
    [TestFixture]
    public class Program
    {

        /// <summary>
        /// 一個 Class 繼承多個 Interface
        /// Test: IPayment 和 IPaymentAAA 產生各別的 instance，造成資源浪費
        /// </summary>
        [Test]
        public void WhenRegistered_SeparateSingleton_InstancesAreNotSame()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddSingleton<IPayment, AAAPayment>();
            services.AddSingleton<IPaymentAAA, AAAPayment>();

            var provider = services.BuildServiceProvider();

            // Act
            var payment1 = provider.GetService<IPayment>(); // An instance of Foo
            var payment2 = provider.GetService<IPaymentAAA>(); // An instance of Foo

            // Assert
            Assert.AreNotSame(payment1, payment2); // 不同的 Instance
        }

        /// <summary>
        /// 一個 Class 繼承多個 Interface
        /// Test: 先註冊 Class，再把 Class 的 Instance 給 Interface，才會產生相同的 instance，而不是 IPayment 和 IPaymentAAA 產生各別的 instance
        /// </summary>
        [Test]
        public void WhenRegistered_SeparateSingleton_InstancesAreSame()
        {
            // Arrange
            var services = new ServiceCollection(); 

            services.AddSingleton<AAAPayment>(); // 先註冊 AAAPayment
            services.AddSingleton<IPayment>(x => x.GetRequiredService<AAAPayment>()); // 把 AAAPayment 的 instance 給 IPayment
            services.AddSingleton<IPaymentAAA>(x => x.GetRequiredService<AAAPayment>()); // 把 AAAPayment 的 instance 給 IPaymentAAA

            var provider = services.BuildServiceProvider();

            // Act
            var payment = provider.GetService<AAAPayment>();
            var payment1 = provider.GetService<IPayment>();
            var payment2 = provider.GetService<IPaymentAAA>();

            // Assert
            Assert.AreSame(payment, payment1); // 相同的 Instance
            Assert.AreSame(payment, payment2); // 相同的 Instance
            Assert.AreSame(payment1, payment2); // 相同的 Instance
        }

        /// <summary>
        /// 1. 一個 Interface 多個 Class 實作
        /// 2. 一個 Class 繼承多個 Interface
        /// Test: 一個 Interface 多個 Class 實作，不同的實作要不相等
        /// </summary>
        [Test]
        public void WhenRegistered_MultipleImplementation_InstancesAreNotSame()
        {
            var (payment, paymentAAA, paymentBBB) = MultipleImplementationArrange();

            // AssertInstance
            Assert.AreNotSame(paymentAAA, paymentBBB); // 不同的 Instance
        }

        /// <summary>
        /// 1. 一個 Interface 多個 Class 實作
        /// 2. 一個 Class 繼承多個 Interface
        /// Test: 一個 Class 繼承多個 Interface，各別的 Interface 是否為相同 Instance
        /// </summary>
        [Test]
        public void WhenRegistered_MultipleImplementation_InstancesAreSame()
        {
            var (payment, paymentAAA, paymentBBB) = MultipleImplementationArrange();

            // Assert
            Assert.AreSame(payment, paymentAAA); // 相同的 Instance
        }

        private (AAAPayment payment, IPayment paymentAAA, IPayment paymentBBB) MultipleImplementationArrange()
        {
            // Arrange
            var services = new ServiceCollection();

            services.AddSingleton<AAAPayment>(); // 先註冊 AAAPayment
            services.AddSingleton<IPayment>(x => x.GetRequiredService<AAAPayment>()); // 把 AAAPayment 的 instance 給 IPayment
            services.AddSingleton<IPaymentAAA>(x => x.GetRequiredService<AAAPayment>()); // 把 AAAPayment 的 instance 給 IPaymentAAA

            services.AddSingleton<IPayment, BBBPayment>(); // 註冊 BBBPayment

            var provider = services.BuildServiceProvider();

            // Act
            var payment = provider.GetService<AAAPayment>();
            var payments = provider.GetService<IEnumerable<IPayment>>();

            var paymentAAA = payments.Where(w => w.PaymentType() == "AAA").FirstOrDefault();
            var paymentBBB = payments.Where(w => w.PaymentType() == "BBB").FirstOrDefault();

            return (payment, paymentAAA, paymentBBB);
        }
    }
}