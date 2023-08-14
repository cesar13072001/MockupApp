using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockupApp.Models
{
    public partial class PaypalOrderCapture
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("payment_source")]
        public PaymentSource PaymentSource { get; set; }

        [JsonProperty("purchase_units")]
        public List<PurchaseUnitOC> PurchaseUnits { get; set; }

        [JsonProperty("payer")]
        public Payer Payer { get; set; }

        [JsonProperty("create_time")]
        public DateTimeOffset CreateTime { get; set; }

        [JsonProperty("update_time")]
        public DateTimeOffset UpdateTime { get; set; }

        [JsonProperty("links")]
        public List<LinkOC> Links { get; set; }
    }

    public partial class LinkOC
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public partial class Payer
    {
        [JsonProperty("name")]
        public PayerName Name { get; set; }

        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("payer_id")]
        public string PayerId { get; set; }

        [JsonProperty("address")]
        public PayerAddress Address { get; set; }
    }

    public partial class PayerAddress
    {
        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }

    public partial class PayerName
    {
        [JsonProperty("given_name")]
        public string GivenName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }
    }

    public partial class PaymentSource
    {
        [JsonProperty("paypal")]
        public Paypal Paypal { get; set; }
    }

    public partial class Paypal
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("account_status")]
        public string AccountStatus { get; set; }

        [JsonProperty("name")]
        public PayerName Name { get; set; }

        [JsonProperty("address")]
        public PayerAddress Address { get; set; }
    }

    public partial class PurchaseUnitOC
    {
        [JsonProperty("reference_id")]
        public string ReferenceId { get; set; }

        [JsonProperty("amount")]
        public AmountOC Amount { get; set; }

        [JsonProperty("payee")]
        public Payee Payee { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("items")]
        public List<ItemOC> Items { get; set; }

        [JsonProperty("shipping")]
        public Shipping Shipping { get; set; }

        [JsonProperty("payments")]
        public Payments Payments { get; set; }
    }

    public partial class AmountOC
    {
        [JsonProperty("currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("breakdown")]
        public Breakdown Breakdown { get; set; }
    }

    public partial class BreakdownOC
    {
        [JsonProperty("item_total")]
        public Handling ItemTotal { get; set; }

        [JsonProperty("shipping")]
        public Handling Shipping { get; set; }

        [JsonProperty("handling")]
        public Handling Handling { get; set; }

        [JsonProperty("insurance")]
        public Handling Insurance { get; set; }

        [JsonProperty("shipping_discount")]
        public Handling ShippingDiscount { get; set; }
    }

    public partial class Handling
    {
        [JsonProperty("currency_code")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class ItemOC
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("unit_amount")]
        public Handling UnitAmount { get; set; }

        [JsonProperty("tax")]
        public Handling Tax { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

     

        
    }

    public partial class Payee
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; set; }

        [JsonProperty("merchant_id")]
        public string MerchantId { get; set; }
    }

    public partial class Payments
    {
        [JsonProperty("captures")]
        public List<Capture> Captures { get; set; }
    }

    public partial class Capture
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public Handling Amount { get; set; }

        [JsonProperty("final_capture")]
        public bool FinalCapture { get; set; }

        [JsonProperty("seller_protection")]
        public SellerProtection SellerProtection { get; set; }

        [JsonProperty("seller_receivable_breakdown")]
        public SellerReceivableBreakdown SellerReceivableBreakdown { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }

        [JsonProperty("create_time")]
        public DateTimeOffset CreateTime { get; set; }

        [JsonProperty("update_time")]
        public DateTimeOffset UpdateTime { get; set; }
    }

    public partial class SellerProtection
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("dispute_categories")]
        public List<string> DisputeCategories { get; set; }
    }

    public partial class SellerReceivableBreakdown
    {
        [JsonProperty("gross_amount")]
        public Handling GrossAmount { get; set; }

        [JsonProperty("paypal_fee")]
        public Handling PaypalFee { get; set; }

        [JsonProperty("net_amount")]
        public Handling NetAmount { get; set; }
    }

    public partial class Shipping
    {
        [JsonProperty("name")]
        public ShippingName Name { get; set; }

        [JsonProperty("address")]
        public ShippingAddress Address { get; set; }
    }

    public partial class ShippingAddress
    {
        [JsonProperty("address_line_1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("admin_area_2")]
        public string AdminArea2 { get; set; }

        [JsonProperty("admin_area_1")]
        public string AdminArea1 { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }
    }

    public partial class ShippingName
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }
    }

    public enum CurrencyCode { Usd };

}