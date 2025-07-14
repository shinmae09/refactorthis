using RefactorThis.Domain.Constants;
using RefactorThis.Domain.Factories;
using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Entities.Models;
using RefactorThis.Persistence.Extensions;
using RefactorThis.Persistence.Interfaces;
using RefactorThis.Persistence.Models;
using System;
using System.Threading.Tasks;

namespace RefactorThis.Domain
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;  

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public async Task<string> ProcessPayment(string reference, Payment payment)
        {
            var invoice = await _invoiceRepository.GetInvoiceByReferenceAsync(reference);
            if (invoice == null)
            {
                throw new InvalidOperationException(ValidationMessage.NO_INVOICE_FOUND_MESSAGE);
            }

            var validationResponse = ValidateInvoiceAndPayment(invoice, payment);
            if (!string.IsNullOrEmpty(validationResponse))
            {
                return validationResponse;
            }

            var invoiceHandler = InvoiceHandlerFactory.CreateInvoiceHandler(invoice);
            var result = invoiceHandler.HandleInvoiceAndPayment(payment);
            await _invoiceRepository.UpdateAsync(invoice);

            return result;
        }

        public async Task<string> ProcessRefund(string reference, Refund refund)
        {
            var invoice = await _invoiceRepository.GetInvoiceByReferenceAsync(reference);
            if (invoice == null)
            {
                throw new InvalidOperationException(ValidationMessage.NO_INVOICE_FOUND_MESSAGE);
            }

            if (invoice.Type != InvoiceType.Commercial)
            {
                return ReturnMessage.REFUNDS_ONLY_FOR_COMMERCIAL_INVOICES_MESSAGE;
            }

            var invoiceHandler = InvoiceHandlerFactory.CreateInvoiceHandler(invoice);
            var result = invoiceHandler.HandleRefund(refund);

            await _invoiceRepository.UpdateAsync(invoice);

            return result;
        }

        private string ValidateInvoiceAndPayment(Invoice invoice, Payment payment)
        {
            invoice.ThrowIfNull(nameof(invoice));
            payment.ThrowIfNull(nameof(payment));

            if (invoice.Amount <= 0)
            {
                if (invoice.HasPayments())
                {
                    throw new InvalidOperationException(ValidationMessage.INVALID_INVOICE_STATE_MESSAGE);
                }
                return ReturnMessage.NO_PAYMENT_NEEDED_MESSAGE;
            }

            if (invoice.HasPayments())
            {
                var amountDue = invoice.GetAmountDue();
                if (amountDue <= 0)
                {
                    return ReturnMessage.INVOICE_ALREADY_FULLY_PAID_MESSAGE;
                }

                if (payment.Amount > amountDue)
                {
                    return ReturnMessage.PAYMENT_GREATER_THAN_PARTIAL_AMOUNT_MESSAGE;
                }
            }
            else
            {
                if (payment.Amount > invoice.Amount)
                {
                    return ReturnMessage.PAYMENT_GREATER_THAN_INVOICE_AMOUNT_MESSAGE;
                }
            }

            return string.Empty;
        }
    }
}