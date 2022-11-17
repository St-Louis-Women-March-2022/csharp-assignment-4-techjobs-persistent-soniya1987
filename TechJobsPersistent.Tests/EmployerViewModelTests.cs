using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TechJobsPersistentAutograded.ViewModels;
using Xunit;

namespace TaskTwo.Tests
{
    public class EmployerViewModelTests
    {
        //1 -- has viewmodel properties been added
        [Fact]
        public void AddEmployerViewModel_ContainsCorrectProperties()
        {
            //Arrange
            Type testEVM = typeof(AddEmployerViewModel);
            PropertyInfo[] infos = testEVM.GetProperties();
            List<string> infoName = new List<string>();

            //Act
            for (int i = 0; i < infos.Length; i++)
            {
                PropertyInfo info = infos[i];
                infoName.Add(info.Name);
            }

            //Assert
            Assert.True(infos.Length > 1 && infos.Length < 3);
            Assert.Contains("Name", infoName);
            Assert.Contains("Location", infoName);
        }

        //2 -- have custom attributes been added
        [Fact]
        public void AddEmployerViewModel_ContainsCorrectAttributes()
        {
            //Arrange
            Type testEVM = typeof(AddEmployerViewModel);
            PropertyInfo[] infos = testEVM.GetProperties();
            bool nameAttributes = false;
            bool locAttributes = false;

            //Act
            for (int i = 0; i < infos.Length; i++)
            {
                foreach (var info in infos)
                {
                    if (info.Name == "Name")
                    {
                        var attributes = info.CustomAttributes.ToList();
                        foreach (var attribute in attributes)
                        {
                            if (attribute.NamedArguments.Count > 0)
                            {
                                nameAttributes = true;
                            }
                        }
                    }

                    if (info.Name == "Location")
                    {
                        var attributes = info.CustomAttributes.ToList();
                        foreach (var attribute in attributes)
                        {
                            if (attribute.NamedArguments.Count > 0)
                            {
                                locAttributes = true;
                            }
                        }
                    }
                }

            }

            //Assert
            Assert.True(nameAttributes);
            Assert.True(locAttributes);
           
        }

    }
}
