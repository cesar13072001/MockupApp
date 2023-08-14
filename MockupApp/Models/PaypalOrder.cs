using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockupApp.Models
{
    public class PaypalOrder
    {
        [JsonProperty("intent")]
        public string Intent { get; set; }

        [JsonProperty("purchase_units")]
        public List<PurchaseUnit> PurchaseUnits { get; set; }

        [JsonProperty("application_context")]
        public ApplicationContext ApplicationContext { get; set; }
    }

    public class ApplicationContext
    {
        [JsonProperty("brand_name")]
        public string BrandName { get; set; }

        [JsonProperty("user_action")]
        public string UserAction { get; set; }

        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("cancel_url")]
        public string CancelUrl { get; set; }
    }

    public class PurchaseUnit
    {
        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("amount")]
        public Amount Amount { get; set; }
    }

    public class Amount
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("breakdown")]
        public Breakdown Breakdown { get; set; }
    }

    public class Breakdown
    {
        [JsonProperty("item_total")]
        public ItemTotal ItemTotal { get; set; }
    }

    public class ItemTotal
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

       
    }

    public class Item
    {
        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unit_amount")]
        public ItemTotal UnitAmount { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

    }

   

}