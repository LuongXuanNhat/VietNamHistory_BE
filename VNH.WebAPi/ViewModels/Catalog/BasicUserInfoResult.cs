namespace VNH.WebAPi.ViewModels.Catalog
{
    public class BasicUserInfoResult
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool IsSuccessful { get; set; }
        public string? ErrorMessage { get; set; }
    }
}