namespace Star_Properties.Model.ResponseModel
{
    public class PropertyDashboardResponse
    {
        public int Year { get; set; }
        public List<PropertyGraphResponse> GraphData { get; set; } = new List<PropertyGraphResponse>();
        public PropertySummaryResponse Summary { get; set; } = new PropertySummaryResponse();
    }

    public class PropertySummaryResponse
    {
        public int TotalActive { get; set; }
        public int TotalSoldOut { get; set; }
    }

    public class DashboardSummaryResponse
    {
        public int TotalProperties { get; set; }
        public int ActiveProperties { get; set; }
        public int SoldProperties { get; set; }
        public int TotalLeads { get; set; }

        public List<PropertyTypeDistribution> DistributionByProperties { get; set; } = new();
    }

    public class PropertyTypeDistribution
    {
        public string PropertyType { get; set; }

        public int Count { get; set; }

        public decimal Percentage { get; set; }
    }
}
