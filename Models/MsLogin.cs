namespace Jeremiah_SupermarketOnline.Models
{

    public class MsLogin
    {
        public string odatacontext { get; set; }
        public string userPrincipalName { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public string surname { get; set; }
        public string givenName { get; set; }
        public string preferredLanguage { get; set; }
        public string mail { get; set; }
        public object mobilePhone { get; set; }
        public object jobTitle { get; set; }
        public object officeLocation { get; set; }
        public object[] businessPhones { get; set; }
    }

}
