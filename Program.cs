 AccountController:
     public ActionResult LogOn(LogOnModel model, string returnUrl)

-->
	 var auth = new AuthenticateUser();

               bool hasAccess = auth.getUserAccess(model.UserName);

                if(!hasAccess)
                    return Redirect("http://www.google.com");
					
					
	~~~~
AuthenticateUser.cs
-->	

     public bool getUserAccess(string username)
        {
            CloudTable tbl = AuthTable();

            var entities = tbl.ExecuteQuery(new TableQuery<Users>()).ToList().Where(x=> x.mail == username).FirstOrDefault();
            
            if(entities != null)
                return entities.accessMercury;

            return false;
           
        }

        private static CloudTable AuthTable()
        {
            string accountName = "initialdbstorage";
            string accountKey = "OlJgQ4W/J3aW91v9v1weXs6mq8Pmigl2RfWxqlczcK2VUhiOzCge9VOL+NNe4dvp/O5cyY/FwpP0mkCQ==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("initaltable");

                return table;
            }
            catch
            {
                return null;
            }
        }
		
		
		
				

--> Users.cs

  public class Users : TableEntity
    {
        public Users() { }
        public bool accessBackOffice { get; set; }
        public bool accessGlide { get; set; }
        public bool accessManagerAnalytics { get; set; }
        public bool accessPeerComparison { get; set; }
        public bool accessPortal { get; set; }
        public bool accessPortfolioAnalytics { get; set; }
        public bool accessPreRegister { get; set; }
        public bool accessRadias { get; set; }
        public bool accessUserDocs { get; set; }
        public string familyName { get; set; }
        public string givenName { get; set; }
        public bool is_admin { get; set; }
        public string mail { get; set; }
        public bool accessPreviewPortfolioAnalytics { get; set; }
        public bool accessMercury { get; set; }
       
    }

	
