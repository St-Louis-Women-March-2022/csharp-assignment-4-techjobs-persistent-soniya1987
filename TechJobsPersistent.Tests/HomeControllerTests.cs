using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TechJobsPersistentAutograded.Controllers;
using TechJobsPersistentAutograded.Data;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.ViewModels;
using Xunit;

namespace TaskTwo.Tests
{
    public class HomeControllerTests
    {
        public HomeControllerTests() { }

// ------- Index Method Tests

        [Fact]
        public void IndexAction_ReturnsViewResultForAllJobs()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            mockRepo
                .Setup(r => r.GetAllJobs())
                .Returns(GetTestJobs());
            var controller = new HomeController(mockRepo.Object);

            //act
            var result = controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Job>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        

// ------ AddJob Tests

        [Fact]
        public void AddJobMethod_ContainsAppropriateLocalVarialbes()
        {
            //arrange
            Type testType = typeof(HomeController);
            MethodBody mb = testType.GetMethod("AddJob").GetMethodBody();
            var locals = mb.LocalVariables;

            //assert
            Assert.True(locals.Count > 1);
        }

// ProcessAddJobForm -----------

        [Fact]
        public void ProcessAddJobFormMethod_TakesAddJobViewModelAndSelectedSkillParameters()
        {
            //arrange
            Type testType = typeof(HomeController);
            MethodInfo mi = testType.GetMethod("ProcessAddJobForm");
            ParameterInfo[] pi = mi.GetParameters();

            //assert
            Assert.Equal(2, pi.Length);
            Assert.Equal("addJobViewModel", pi[0].Name);
            Assert.Equal("selectedSkills", pi[1].Name);
        }

        [Fact]
        public void ProcessAddJobFormMethod_CreatesValidJobObjects()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);

            Job? job = null;

            mockRepo.Setup(r => r.AddNewJob(It.IsAny<Job>()))
                .Callback<Job>(x => job = x);

            var newTestJobVM = new AddJobViewModel
            {
                Name = "Tests"
            };

            string[] testSelectedSkills = { "1", "2" };

            //act
            controller.ProcessAddJobForm(newTestJobVM, testSelectedSkills);
            mockRepo.Verify(x => x.AddNewJob(It.IsAny<Job>()), Times.Once);

            //assert
            Assert.Equal(job.Name, newTestJobVM.Name);
            Assert.NotNull(newTestJobVM.Name);

        }

        [Fact]
        public void ProcessJobForm_InvalidModelState_NeverAddsNewJob()
        {
            //arrange
            var mockRepo = new Mock<JobRepository>();
            var controller = new HomeController(mockRepo.Object);
            controller.ModelState.AddModelError("JobId", "Job is required");

            var newTestJobVM = new AddJobViewModel { Name = "Tests" };

            string[] testSelectedSkills = {  };

            //act
            controller.ProcessAddJobForm(newTestJobVM, testSelectedSkills);

            //assert
            mockRepo.Verify(r => r.AddNewJob(It.IsAny<Job>()), Times.Never);
        }

        //using this for index test
        private IEnumerable<Job> GetTestJobs()
        {
            return new List<Job>()
            {
                new Job { Id = 1, Name = "Unit Tester" },
                new Job { Id = 2, Name = "Quality Tester" }
            };  
        }
    }
}
