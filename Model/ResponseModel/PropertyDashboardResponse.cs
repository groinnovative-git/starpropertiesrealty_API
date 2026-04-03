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
}
