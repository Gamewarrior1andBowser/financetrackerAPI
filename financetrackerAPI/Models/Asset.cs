using System.ComponentModel.DataAnnotations;

namespace financetrackerAPI.Models
{
    public class Asset
    {
        [Key]
        public int assetID { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string AssetType { get; set; }
        public decimal CurrentPrice { get; set; }
        public string? ExternalApiID { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}