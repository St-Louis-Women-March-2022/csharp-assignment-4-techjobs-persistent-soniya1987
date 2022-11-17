using System;
using Xunit;
using TechJobsPersistentAutograded.ViewModels;
using TechJobsPersistentAutograded.Models;
using TechJobsPersistentAutograded.Controllers;
using TechJobsPersistentAutograded.Data;
using System.Reflection;
using System.Collections.Generic;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace TaskThree.Tests
{
    public class TestTaskThree
    {

        public TestTaskThree()
        {
        }

        [Fact]
        public void TestSkillsPropertyAdded()
        {
            Type testClass = typeof(AddJobViewModel);
            PropertyInfo[] properties = testClass.GetProperties();
            bool foundSkillsByType = false;
            bool foundSkillsByName = false;
            Type testListType = typeof(List<Skill>);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                if (property.Name == "Skills")
                {
                    foundSkillsByName = true;
                }

                if (property.PropertyType == testListType)
                {
                    foundSkillsByType = true;
                }
            }

            Assert.True(foundSkillsByName);
            Assert.True(foundSkillsByType);
        }

        [Fact]
        public void TestUpdatedConstructor()
        {
            Type[] types = new Type[2];
            types[0] = typeof(List<Employer>);
            types[1] = typeof(List<Skill>);
            ConstructorInfo constructorInfo = typeof(AddJobViewModel).GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, types, null);
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal("System.Collections.Generic.List`1[[TechJobsPersistentAutograded.Models.Skill, TechJobsPersistentAutograded, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]]", parameters[1].ParameterType.FullName);
            Assert.Equal("skills", parameters[1].Name);
        }

        [Fact]
        public void TestUpdatedProcessAddJobFormParameters()
        {
            MethodInfo methodInfo = typeof(HomeController).GetMethod("ProcessAddJobForm");
            ParameterInfo[] parameters = methodInfo.GetParameters();

            Assert.Equal(2, parameters.Length);
            Assert.Equal("System.String[]", parameters[1].ParameterType.FullName);
            Assert.Equal("selectedSkills", parameters[1].Name);
        }

        [Fact]
        public void TestLocalVariableUpdatesToProcessAddJobFormBody()
        {
            MethodInfo methodInfo = typeof(HomeController).GetMethod("ProcessAddJobForm");
            var locals = methodInfo.GetMethodBody().LocalVariables;
            Assert.True(locals.Count > 1);
        }
    }
}
