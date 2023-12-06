using JobApplicationLibrary.Models;
using JobApplicationLibrary.Services.Abstract;
using Moq;
using FluentAssertions; 

namespace JobApplicationLibrary.UnitTest
{
    public class JobApplicationEvaluteUnitTest
    {
        [Test]  //Basic Test
        public void Application_WithUnderAge_AutoRejected()
        {
            //Arrange
            ApplicationEvulator evulator = new ApplicationEvulator(null);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 15
                }
            };

            //Action
             ApplicationResult result = evulator.Evalute(form);

            //Assert
           // Assert.AreEqual(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
        }

        [Test] //using Moq for mock datas.
        public void Application_WithNoTechStack_AutoRejected()
        {
            //Arrange

            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c=>c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Azerbaijan");

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age= 20
                },
                TechStackList = new List<string>() { "" },
                YearsOfExperience = 20
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            // Assert.AreEqual(ApplicationResult.AutoRejected, result);
            result.Should().Be(ApplicationResult.AutoRejected);
        }
        [Test] //Using moq for mock data.
        public void Application_WithTechStackOver75_AutoAccepted()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Azerbaijan");

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age=20
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            //Assert.AreEqual(ApplicationResult.AutoAccepted, result);
            result.Should().Be(ApplicationResult.AutoAccepted);
        }
        [Test] //Using moq for false mock datas.
        public void Application_WithInvalidIdentityNumber_TransferredToHR()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(false);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Azerbaijan");

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 20
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            //Assert.AreEqual(ApplicationResult.TransferredToHR, result);
            result.Should().Be(ApplicationResult.TransferredToHR);
        }

        // if class has a new property which not in moq.

        /*
        [Test] //If class has a ney property, nothing changes, simply add.
        public void Application_WithDifferentLocation_TransferredToCTO()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 20
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20,
               // OfficeLocation="Ganja"
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            Assert.AreEqual(ApplicationResult.TransferredToCTO, result);
        }

        */

        //if interface has a new property on moq.
        /*
        [Test] //If moq validator has a new propert(services), setup this property
        public void Application_WithDifferentCountry_TransferredToCTO()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.Country).Returns("Turkey");

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 20
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20,  
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            Assert.AreEqual(ApplicationResult.TransferredToCTO, result);
        }
        */
        [Test] //If moq validator has a new propert(services), setup this property
        //If interface has hierchial structure. 
        //Moq allows to act every interface as property of another interface.
        public void Application_WithDifferentCountry_TransferredToCTO()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Turkey");
            ApplicationEvulator evaluator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 20
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20,
            };

            //Action
            ApplicationResult result = evaluator.Evalute(form);

            //Assert
            //Assert.AreEqual(ApplicationResult.TransferredToCTO, result);
            result.Should().Be(ApplicationResult.TransferredToCTO);
        }

        /*
        [Test] //Remembering data in class
        public void Application_WithOver_ValidationModeToDetailed()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Turkey");
            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 51
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20,
            };

            //Action
            ApplicationResult result = evulator.Evalute(form);

            //Assert
            Assert.AreEqual(ValidationMode.Detailed, form.ValidationMode);
        }
        */
        [Test] //Remembering data in interface
        public void Application_WithOver_ValidationModeToDetailed()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.Setup(c => c.IsValid(It.IsAny<string>())).Returns(true);
            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Turkey");
            
            //remembering data from interface(Moq doesnt remember data by default, and gave <default> values
            mockValidator.SetupProperty(c => c.ValidationMode);

            ///<summary>
            ///if interface has 2 or more property to setup use SetupAllProperties.
            ///Consider [Setup] method must use later from SetupAllProperties for overriding spesific method.
            ///</summary>

            ApplicationEvulator evaluator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 51
                },
                TechStackList = new List<string>() { "C#", "RabbitMq", "MicroService", "Visual Studio" },
                YearsOfExperience = 20,
            };

            //Action
            ApplicationResult result = evaluator.Evalute(form);

            //Assert
           // Assert.AreEqual(ValidationMode.Detailed, mockValidator.Object.ValidationMode);
            mockValidator.Object.ValidationMode.Should().Be(ValidationMode.Detailed);
        }
        [Test] //Testing  throwing exceptions.
        public void Application_WithNullAppicant_ThrowsArgumentNullException()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();

            ApplicationEvulator evulator = new ApplicationEvulator(mockValidator.Object);

            JobApplication form = new JobApplication();

            //Action
            Action appResultAction = () => evulator.Evalute(form);

            //Assert
            appResultAction.Should().Throw<ArgumentNullException>();
        }

        [Test] //Check if method called with any/specisifir value
        public void Application_WithDefaultValue_IsValidCalled()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Azerbaijan");

            ApplicationEvulator evaluator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 20,
                    IdentityNumber="123"
                }
            };

            //Action
            var appResult = evaluator.Evalute(form);

            //Assert


            //test success if IsValid method has IdentityNumber=123
            // mockValidator.Verify(c=>c.IsValid("123"), "IsValid Method should be called with value 123");


            //test success if IdentityNumber has any value
            mockValidator.Verify(c=>c.IsValid(It.IsAny<string>()));
        }

        [Test] //Check if 
        public void Application_WithYoungAge_IsValidNeverCalled()
        {
            //Arrange
            Mock<IIdentityValidator> mockValidator = new Mock<IIdentityValidator>();
            mockValidator.DefaultValue = DefaultValue.Mock;

            mockValidator.Setup(c => c.CountryDataProvider.CountryData.Country).Returns("Azerbaijan");

            ApplicationEvulator evaluator = new ApplicationEvulator(mockValidator.Object);
            JobApplication form = new JobApplication()
            {
                Applicant = new Applicant()
                {
                    Age = 15,
                    IdentityNumber = "123"
                }
            };

            //Action
            var appResult = evaluator.Evalute(form);

            //Assert
            

            //Times.Never() => IsValid method never called.  Times.Exactly(number) => how many times.
            mockValidator.Verify(c=>c.IsValid("123"), Times.Never());

        }
    }
}