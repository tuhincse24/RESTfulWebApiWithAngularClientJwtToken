using System;
using System.Collections.Generic;
namespace RestServerApi.Entities
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IndustryType { get; set; }
        public string RegionalCountry { get; set; }
        public string Address { get; set; }
        public int ProjectCount { get; set; }
        public string LogoLocation { get; set; }

        public static List<Client> Clients => new List<Client>
        {
            new Client{ Id=Guid.NewGuid().ToString() , Name="Microsoft", RegionalCountry="Bangladesh", Address="Dhaka", ProjectCount=20},
            new Client{ Id=Guid.NewGuid().ToString(), Name="Dell", RegionalCountry="Bangladesh", Address="Dhaka", ProjectCount=10},
            new Client{ Id=Guid.NewGuid().ToString(), Name="Nestle", RegionalCountry="Bangladesh", Address="Dhaka", ProjectCount=5},
            new Client{ Id=Guid.NewGuid().ToString(), Name="Walmart", RegionalCountry="India", Address="Dhaka", ProjectCount=8},
            new Client{ Id=Guid.NewGuid().ToString(), Name="MetLife", RegionalCountry="Bangladesh", Address="Dhaka", ProjectCount=3}
        };
    }
}