using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.Services.Abstract
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);
       // bool CheckConnectionToRemoteServer();
       // string Country { get; }

        public ICountryDataProvider CountryDataProvider { get; }
        public ValidationMode ValidationMode { get; set; }
    }

    public interface ICountryData
    {
        string Country { get; }
    }

    public interface ICountryDataProvider
    {
        public ICountryData CountryData { get; }
    }
}