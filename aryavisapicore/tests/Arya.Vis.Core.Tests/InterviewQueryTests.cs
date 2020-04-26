using Arya.Vis.Api;
using Arya.Vis.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Arya.Vis.Core.Tests
{
    public class InterviewQueryTests
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        public InterviewQueryTests()
        {
            testServer = new TestServer(new WebHostBuilder()
                    .UseStartup<Startup>());
            testServer.AllowSynchronousIO = true;

            testClient = testServer.CreateClient();
            testClient.DefaultRequestHeaders.Accept.Clear();
            testClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        [Fact]
        public async void GetInterviews_VerifyStatusCode_ShouldPass()
        {
            //Arrange
            var graphQLQuery = @"{
	                            ""Query"":""{ interviews { interviewGuid interviewCode  interviewTitle interviewOwnerGuid interviewStartDate interviewEndDate} }""
                            }";
            //Act
            var response = await testClient.PostAsync("/graphql", new StringContent(graphQLQuery, Encoding.UTF8, "application/json"));
            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetInterviews_VerifyContent_ShouldPass()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/");
            var graphQLQuery = @"{
	                            ""Query"":""{ interviews { interviewGuid interviewCode  interviewTitle interviewOwnerGuid interviewStartDate interviewEndDate} }""
                            }";
            //Act
            var response = await testClient.PostAsync("/graphql", new StringContent(graphQLQuery, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            var interviews = JsonConvert.DeserializeObject<Interview>(content);
            //Assert
            Assert.NotNull(interviews);
        }


        [Fact]
        public async void GetInterview_WithVariable_VerifyContent_ShouldPass()
        {
            //Arrange
            var request = new HttpRequestMessage(new HttpMethod("POST"), "/");
            var graphQLQuery = @"{
	                            ""Query"":""query interview($id:ID!){ interview(interviewGuid: $id) { 
                                                    interviewGuid 
                                                    interviewCode  
                                                    interviewTitle 
                                                    interviewOwnerGuid 
                                                    interviewStartDate 
                                                    interviewEndDate} } "",
                                ""Variables"":{ ""id"":""725000ae-ba39-45a5-a10f-79e5b27fe747""}
                                    }
                            ";
            //Act
            var response = await testClient.PostAsync("/graphql", new StringContent(graphQLQuery, Encoding.UTF8, "application/json"));

            var content = await response.Content.ReadAsStringAsync();
            var interviews = JsonConvert.DeserializeObject<Interview>(content);
            //Assert
            Assert.NotNull(interviews);
        }

    }
}
