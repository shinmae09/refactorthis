using RefactorThis.Domain.Constants;
using RefactorThis.Persistence.Entities.Enums;
using RefactorThis.Persistence.Models;
using System;

namespace RefactorThis.Domain.Factories
{
    public class InvoiceHandlerFactory
    {
        public static InvoiceHandler CreateInvoiceHandler(Invoice invoice, Payment payment)
        {
            switch (invoice.Type)
            {
                case InvoiceType.Standard:
                    return new StandardInvoiceHandler(invoice, payment);
                case InvoiceType.Commercial:
                    return new CommercialInvoiceHandler(invoice, payment);
                default:
                    throw new ArgumentException(ValidationMessage.INVALID_INVOICE_TYPE_MESSAGE, nameof(invoice.Type));
            }
        }
    }
}