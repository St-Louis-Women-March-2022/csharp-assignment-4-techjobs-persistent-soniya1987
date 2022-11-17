using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechJobsPersistentAutograded.Controllers;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;
using Xunit;

namespace TaskTwo.Tests
{
    public class EmployerViewsTests
    {
        public EmployerViewsTests() { }
        

// ------- Index Method Tests --------------------------------

        [Fact]
        public void IndexAction_Executes_ReturnsViewForIndex()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);

            //act
            var result = controller.Index();

            //assert
            Assert.IsType<ViewResult>(result);
            Assert.NotNull(result);
        }



        // --------  About View Tests

        [Fact]
        public void AboutAction_Executes_ReturnsViewForAboutWithId()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            mockRepo
                .Setup(r => r.GetAllEmployers())
                .Returns(GetTestEmployers());
            var controller = new EmployerController(mockRepo.Object);
            //act
            var result1 = controller.About(1);
            var result2 = controller.About(2);
            var viewResult1 = Assert.IsType<ViewResult>(result1);
            var viewResult2 = Assert.IsType<ViewResult>(result2);

            //assert
            Assert.NotSame(result1, result2);
            Assert.Same(result1, result1);
            Assert.IsType<ViewResult>(result1);
            Assert.IsType<ViewResult>(result2);
            Assert.Same(viewResult1, viewResult1);
            Assert.NotSame(viewResult1, viewResult2);
        }

        // ---------- Add View Tests

        [Fact]
        public void Add_ActionExecutes_ReturnsViewForAdd()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);

            //act
            var result = controller.Add();

            //assert
            Assert.IsType<ViewResult>(result);
        }


        // ----------- ProcessAddEmployerForm View Tests

        [Fact]
        public void ProcessAddEmployerForm_InvalidModelState_ReturnsView()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);
            controller.ModelState.AddModelError("Name", "Name is Required");

            var employer = new AddEmployerViewModel { Location = "Testland" };

            // act
            var result = controller.ProcessAddEmployerForm(employer);
            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployer = Assert.IsType<AddEmployerViewModel>(viewResult.Model);

            //assert
            Assert.Equal(employer.Location, testEmployer.Location);
        }

        [Fact]
        public void ProcessAddEmployerFormMethod_ReturnsViewForNewEmployer()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new EmployerController(mockRepo.Object);
            AddEmployerViewModel addEmployerViewModel = new AddEmployerViewModel()
            {
                Name = "Lead Testerina",
                Location = "Testandria"
            };

            //act
            var result = controller.ProcessAddEmployerForm(addEmployerViewModel);
            var redirectToActionResult = Assert.IsType<RedirectResult>(result);

            //assert
            Assert.IsType<RedirectResult>(result);
            Assert.NotNull(result);
            Assert.Equal("/Employer", redirectToActionResult.Url);
        }


        // ------- test employers used for the Index and About tests

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
