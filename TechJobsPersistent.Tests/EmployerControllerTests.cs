using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechJobsPersistentAutograded.Controllers;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;
using Xunit;

namespace TaskTwo.Tests
{
    

    public class EmployerControllerTests
    {
        public EmployerControllerTests() { }


// ------- JobRepository Setup Tests  --------------------------------

        [Fact]
        public void EmployerController_ContainsJobRepositoryVariable()
        {
            //arrange
            Type testType = typeof(EmployerController);
            ConstructorInfo[] conInfo = testType.GetConstructors();
            List<string> conNames = new List<string>();
            string conCheck = "";

            //act
            foreach(var name in conInfo)
            {
                conNames.Add(name.ToString());
            }

            foreach (var name in conNames)
            {
                if(name.Contains("Void .ctor(TechJobsPersistentAutograded.Data.JobRepository)"))
                {
                   conCheck += "Found it";
                   break;
                }
            }

            //assert
            Assert.Equal("Found it", conCheck);
        }

// ------- Index Method Tests --------------------------------

        [Fact]
        public void IndexMethod_ReturnsViewResultForAllEmployers()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            mockRepo
                .Setup(r => r.GetAllEmployers())
                .Returns(GetTestEmployers());
            var controller = new EmployerController(mockRepo.Object);

            //act
            var result = controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Employer>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void IndexMethod_ContainsAppropriateLocalVarialbes()
        {
            Type testType = typeof(EmployerController);
            MethodBody mb = testType.GetMethod("Index").GetMethodBody();
            var locals = mb.LocalVariables;

            Assert.True(locals.Count > 1);
        }

        // --------- Add Method Tests

        [Fact]
        public void AddMethod_ContainsAppropriateLocalVarialbes()
        {
            Type testType = typeof(EmployerController);
            MethodBody mb = testType.GetMethod("Add").GetMethodBody();
            var locals = mb.LocalVariables;

            Assert.True(locals.Count > 1);
        }


        // --------- ProcessAddEmployerForm Method Tests --------------------------------

        [Fact]
        public void ProcessAddEmployerFormMethod_TakesAddEmployerViewModelParameter()
        {
            //arrange
            Type testType = typeof(EmployerController);
            MethodInfo mi = testType.GetMethod("ProcessAddEmployerForm");
            ParameterInfo[] pInfo = mi.GetParameters();

            //assert
            Assert.True(pInfo.Length > 0);
            Assert.Equal("addEmployerViewModel", pInfo[0].Name);
        }

        [Fact]
        public void ProcessAddEmployerFormMethod_CreatesValidEmployerObjects()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);

            Employer? emp = null;

            mockRepo.Setup(r => r.AddNewEmployer(It.IsAny<Employer>()))
                .Callback<Employer>(x => emp = x);

            var newTestEmployer = new AddEmployerViewModel()
            {
                Name = "Testnician",
                Location = "Testifax"
            };

            //act

            controller.ProcessAddEmployerForm(newTestEmployer);
            mockRepo.Verify(x => x.AddNewEmployer(It.IsAny<Employer>()), Times.Once);

            //assert
            Assert.Equal(emp.Name, newTestEmployer.Name);
            Assert.Equal(emp.Location, newTestEmployer.Location);
        }




        [Fact]
        public void ProcessAddEmployerForm_InvalidModelState_NeverAddsNewEmployer()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Name is Required");

            //act
            var employer = new AddEmployerViewModel { Location = "Testland" };
            controller.ProcessAddEmployerForm(employer);

            //assert
            mockRepo.Verify(r => r.AddNewEmployer(It.IsAny<Employer>()), Times.Never);
        }

        

// --------- About Method Tests --------------------------------

        [Fact]
        public void AboutMethod_FindsEmployerById()
        {
            //arrange
            Type testType = typeof(EmployerController);
            MethodInfo mi = testType.GetMethod("About");
            ParameterInfo[] pInfo = mi.GetParameters();
            string check = "";

            //act
            foreach(var p in pInfo)
            {
                
                if(p.Name == "id")
                {
                    check += "id";
                }
            }

            //assert
            Assert.True(check == "id");
            
        }

// --- test employers used for the Index and About tests
        private IEnumerable<Employer> GetTestEmployers()
        {
            return new List<Employer>()
            {
                    new Employer() { Id = 1, Name = "Tester", Location = "Testerville" },
                    new Employer() { Id = 2, Name = "Testerista", Location = "Testienna" }
            };
        }

    }

}

//resources:
    //https://code-maze.com/unit-testing-controllers-aspnetcore-moq/
    //https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-6.0
