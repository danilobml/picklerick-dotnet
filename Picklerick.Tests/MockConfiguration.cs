using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Picklerick.Tests.Configuration
{
    public class MockConfiguration : IConfiguration
    {
        public string this[string key]
        {
            get
            {
                if (key == "DefaultConnection")
                {
                    return "Server=localhost;Port=5001;Database=postgrespicklerick;Username=postgres;Password=postgres";
                }
                return null; // Handle other configuration keys if needed
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>();
        }

        public IConfigurationSection GetSection(string key)
        {
            // Implement GetSection to return an empty IConfigurationSection
            return new MockConfigurationSection();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }
    }

    public class MockConfigurationSection : IConfigurationSection
    {
        public string this[string key]
        {
            get
            {
                return null; // Implement if needed
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Key => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        private string _value; // Private backing field for Value property

        public string Value
        {
            get => _value;
            set => _value = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            return new MockConfigurationSection();
        }
    }  
}
