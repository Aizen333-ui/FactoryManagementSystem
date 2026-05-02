using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryDashboard
{
    public static class GlobalStorage
    {
        public static List<RawMaterialEntry> RawMaterials = new List<RawMaterialEntry>();
        public static List<ProductionEntry> Productions = new List<ProductionEntry>();
    }

    public class RawMaterialEntry
    {
        public string? MaterialName { get; set; }
        public double Quantity { get; set; }
        public string? Unit { get; set; }
        public DateTime Date { get; set; }
    }

    public class ProductionEntry
    {
        public string? ProductName { get; set; }
        public double Quantity { get; set; }
        public string? Unit { get; set; }

        public DateTime Date { get; set; }
    }
}