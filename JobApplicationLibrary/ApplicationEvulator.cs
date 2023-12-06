using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services.Abstract;

namespace JobApplicationLibrary
{
    public class ApplicationEvulator
    {
        private const int minAge = 18;
        private const int autoAcceptedYearsOfExperience = 10;
        private List<string> techStackList = new(){ "C#", "RabbitMq","MicroService", "Visual Studio" };
        private IIdentityValidator identityValidator;
        public ApplicationEvulator(IIdentityValidator identityValidator)
        {
            this.identityValidator = identityValidator;
        }
        public ApplicationResult Evalute(JobApplication form)
        {

            if(form.Applicant is null)
                throw new ArgumentNullException();
            

            if (form.Applicant.Age < minAge)
                return ApplicationResult.AutoRejected;

            //  form.ValidationMode = form.Applicant.Age > 50 ? ValidationMode.Detailed : ValidationMode.Quick;

            identityValidator.ValidationMode = form.Applicant.Age > 50 ? ValidationMode.Detailed : ValidationMode.Quick;

            /* class with new property
            if(form.OfficeLocation!="Baku")
                return ApplicationResult.TransferredToCTO;
            */

            /* interface with new property
            if(identityValidator.Country!="Azerbaijan")
                return ApplicationResult.TransferredToCTO;
            */

            //interface with hierchial property
            if (identityValidator.CountryDataProvider.CountryData.Country!="Azerbaijan")
                return ApplicationResult.TransferredToCTO;

            bool validIdentity = identityValidator.IsValid(form.Applicant.IdentityNumber);
            if(!validIdentity)
                return ApplicationResult.TransferredToHR;

            int similarityRate = GetTechStackSimilarityRate(form.TechStackList);
            if(similarityRate < 25)
                return ApplicationResult.AutoRejected;

            if(similarityRate >= 75 && form.YearsOfExperience>=autoAcceptedYearsOfExperience)
                return ApplicationResult.AutoAccepted;

            return ApplicationResult.AutoAccepted;
        }
        private int GetTechStackSimilarityRate(List<string> techStacks) 
        {
            var matchedCount = techStacks.Where(i => techStackList.Contains(i, StringComparer.OrdinalIgnoreCase))
                                         .Count();

            return (int)((double)matchedCount/techStackList.Count)*100;
        }
    }

    public enum ApplicationResult
    {
        AutoRejected,
        TransferredToHR,
        TransferredToLead,
        TransferredToCTO,
        AutoAccepted
    }
}
