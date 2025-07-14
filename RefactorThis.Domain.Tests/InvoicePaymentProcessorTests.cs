using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RefactorThis.Domain.Constants;
using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Entities.Models;
using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.Models;

namespace RefactorThis.Domain.Tests
{
    [TestFixture]
    public class InvoicePaymentProcessorTests
    {
        [Test]
        public async Task ProcessPayment_Should_ThrowException_When_NoInvoiceFoundForPaymentReference()
        {
            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(null as Invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);
            var payment = new Payment();
            var failureMessage = "";

            try
            {
                var result = await paymentProcessor.ProcessPayment("test", payment);
            }
            catch (InvalidOperationException e)
            {
                failureMessage = e.Message;
            }

            Assert.AreEqual(ValidationMessage.NO_INVOICE_FOUND_MESSAGE, failureMessage);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
        {
            var invoice = new Invoice()
            {
                Amount = 0,
                Reference = "test",
                Payments = null
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);
            var payment = new Payment();

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.NO_PAYMENT_NEEDED_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 10
                    }
                }
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);
            var payment = new Payment();

            var result = await paymentProcessor.ProcessPayment("test", payment);

            Assert.AreEqual(ReturnMessage.INVOICE_ALREADY_FULLY_PAID_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);
            var payment = new Payment()
            {
                Amount = 6
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.PAYMENT_GREATER_THAN_PARTIAL_AMOUNT_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
        {
            var invoice = new Invoice()
            {
                Amount = 5,
                Reference = "test",
                Payments = new List<Payment>()
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);

            var payment = new Payment()
            {
                Amount = 6
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.PAYMENT_GREATER_THAN_INVOICE_AMOUNT_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);

            var payment = new Payment()
            {
                Amount = 5
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.FINAL_PARTIAL_PAYMENT_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>() { new Payment() { Amount = 10 } }
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);

            var payment = new Payment()
            {
                Amount = 10
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.INVOICE_ALREADY_FULLY_PAID_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);

            var payment = new Payment()
            {
                Amount = 1
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.NEW_PARTIAL_PAYMENT_BUT_NOT_FULLY_PAID_MESSAGE, result);
        }

        [Test]
        public async Task ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
        {
            var invoice = new Invoice()
            {
                Amount = 10,
                Reference = "test",
                Payments = new List<Payment>()
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var paymentProcessor = new InvoiceService(mockRepo.Object);

            var payment = new Payment()
            {
                Amount = 1
            };

            var result = await paymentProcessor.ProcessPayment(invoice.Reference, payment);

            Assert.AreEqual(ReturnMessage.INVOICE_NOW_PARTIALLY_PAID_MESSAGE, result);
        }

        [Test]
        public async Task ProcessRefund_Should_ReturnForCommercialInvoiceMessage_When_RefundProcessedOnNonCommercialInvoice()
        {
            var invoice = new Invoice()
            {
                Reference = "test",
                Type = InvoiceType.Standard
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var refundProcessor = new InvoiceService(mockRepo.Object);

            var result = await refundProcessor.ProcessRefund(invoice.Reference, new Refund());

            Assert.AreEqual(ReturnMessage.REFUNDS_ONLY_FOR_COMMERCIAL_INVOICES_MESSAGE, result);
        }

        [Test]
        public async Task ProcessRefund_Should_ReturnRefundProcessedSuccessfullyMessage_When_RefundProcessedOnCommercialInvoice()
        {
            var invoice = new Invoice()
            {
                Reference = "test",
                Type = InvoiceType.Commercial,
                Amount = 100,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 150,
                    }
                },
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var refundProcessor = new InvoiceService(mockRepo.Object);

            var refund = new Refund()
            {
                Amount = 50
            };

            var result = await refundProcessor.ProcessRefund(invoice.Reference, refund);

            Assert.AreEqual(ReturnMessage.REFUND_PROCESSED_SUCCESSFULLY_MESSAGE, result);
        }

        [Test]
        public async Task ProcessRefund_Should_ReturnRefundAmountExceedsTotalPaidMessage_When_RefundAmountExceedsTotalPaid()
        {
            var invoice = new Invoice()
            {
                Reference = "test",
                Type = InvoiceType.Commercial,
                Amount = 150,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 50,
                    }
                },
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var refundProcessor = new InvoiceService(mockRepo.Object);

            var refund = new Refund()
            {
                Amount = 250
            };

            var result = await refundProcessor.ProcessRefund(invoice.Reference, refund);

            Assert.AreEqual(ReturnMessage.REFUND_AMOUNT_EXCEEDS_TOTAL_PAID_MESSAGE, result);
        }

        [Test]
        public async Task ProcessRefund_Should_ReturnRefundCannotBeProcessedNoPaymentsMadeOnInvoiceMessage_When_NoPaymentsMadeOnInvoice()
        {
            var invoice = new Invoice()
            {
                Reference = "test",
                Type = InvoiceType.Commercial,
                Amount = 100,
                Payments = new List<Payment>()
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var refundProcessor = new InvoiceService(mockRepo.Object);

            var refund = new Refund()
            {
                Amount = 50
            };
            var result = await refundProcessor.ProcessRefund(invoice.Reference, refund);

            Assert.AreEqual(ReturnMessage.REFUND_CANNOT_BE_PROCESSED_NO_PAYMENTS_MADE_ON_INVOICE_MESSAGE, result);
        }

        [Test]
        public async Task ProcessRefund_Refund_Is_Added_To_Invoice_When_RefundProcessedOnCommercialInvoice()
        {
            var invoice = new Invoice()
            {
                Reference = "test",
                Type = InvoiceType.Commercial,
                Amount = 200,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 150,
                    }
                },
            };

            var mockRepo = new Mock<IInvoiceRepository>();
            mockRepo.Setup(r => r.GetInvoiceByReferenceAsync(It.IsAny<string>())).ReturnsAsync(invoice);

            var refundProcessor = new InvoiceService(mockRepo.Object);

            var refund = new Refund()
            {
                Amount = 50
            };
            await refundProcessor.ProcessRefund(invoice.Reference, refund);

            Assert.IsTrue(invoice.Refunds.Contains(refund));
        }
    }
}