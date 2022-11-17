using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using TechJobsPersistentAutograded;
using Xunit;


namespace TaskOne.Tests
{
    public class TaskOne
    {
        private readonly IConfiguration _configuration;

        public TaskOne()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, false)
                .AddEnvironmentVariables()
                .Build();
        }


        [Fact]
        public void TestDefaultConnectionString()
        {
            string mysqlConnectionString = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            Boolean mysqlConnectionURL = mysqlConnectionString.Contains("server=localhost");
            Assert.True(mysqlConnectionURL, "Server is not localhost");

            Boolean mySqlConnectionDatabase = mysqlConnectionString.Contains("database=techjobs");
            Assert.True(mySqlConnectionDatabase, "Database is not named techjobs");

            Boolean mySqlConnectionUsername = mysqlConnectionString.Contains("userid=techjobs");
            Assert.True(mySqlConnectionUsername, "Username is not techjobs");

            Boolean mySqlConnectionPassword = mysqlConnectionString.Contains("password=techjobs");
            Assert.True(mySqlConnectionPassword, "Password is not techjobs");
        }
    }
}
