using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApertureScience.Model
{
    public class SAPFixedAsset
    {
        public string ItemCode { get; set; } //= "AFPR0003";
        public string ItemName { get; set; } //= "TELÉFONO SAMSUNG 4G A 13 NEGRO";
        public int ItemsGroupCode { get; set; } //= 140;
        public string VatLiable { get; set; } //= "tYES";
        public string PurchaseItem { get; set; } //= "tYES";
        public string SalesItem { get; set; } //= "tYES";
        public string InventoryItem { get; set; } //= "tNO";
        public string AssetItem { get; set; } //= "tNO";
        public string GLMethod { get; set; } //= "glm_WH";
        public string ItemType { get; set; } //= "itFixedAssets";
        public string AssetClass { get; set; } //= "MOBI20GDL";
        public List<SAPItemDepreciationParameters> ItemDepreciationParameters { get; set; }
    }
}
