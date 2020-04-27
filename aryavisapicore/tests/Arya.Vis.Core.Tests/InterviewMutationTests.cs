using Arya.Vis.Api;
using Arya.Vis.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Arya.Vis.Core.Tests
{
    public class InterviewMutationTests
    {
        private readonly TestServer testServer;
        private readonly HttpClient testClient;

        public InterviewMutationTests()
        {
            testServer = new TestServer(new WebHostBuilder()
                    .UseStartup<Startup>());
            testServer.AllowSynchronousIO = true;

            testClient = testServer.CreateClient();
            testClient.DefaultRequestHeaders.Accept.Clear();
            testClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        [Fact]
        public async void CreateInterview_VerifyStatusCode_ShouldPass()
        {
            //Arrange
            var graphQLQuery = @"{
	                            ""Query"":""mutation createInterview($interview: InterviewInput!){  createInterview(interview: $interview){    interviewGuid    interviewTitle interviewStartDate    interviewEndDate    companyLocation  }} "",
	                            ""Variables"":{
	                              ""interview"": {
	                                ""interviewCode"": ""Test Order"",
	                                ""interviewTitle"": ""This is a dummy order"",    
	                                ""interviewStartDate"": ""01-01-2020 05:30pm"",
	                                ""interviewEndDate"": ""01-01-2020 05:30pm"",
	                                ""orgGuid"": ""2D542572-EF99-4786-AEB5-C997D82E57C7"",
	                                ""interviewStatusGuid"": ""2D542572-EF99-4786-AEB5-C997D82E57C7"",
	                                ""interviewOwnerGuid"": ""2D542572-EF99-4786-AEB5-C997D82E57C7"",
	                                ""interviewCreatedDate"": ""01-01-2020 05:30pm"",
	                                ""companyGuid"": ""2D542572-EF99-4786-AEB5-C997D82E57C7"",
	                                ""companyLocation"": ""Company Location - Hyderabad"",
	                                ""jobPostingUrl"": ""http://www.jobpostingurl.com"",
	                                ""jobDesc"": ""Job Description"",
	                                ""jobSummaryVisible"": 1,
	                                ""publishId"": 1,
	                                ""comments"": ""Comments"",
	                                ""emailDesc"": ""Dear candidate, EMAL description"",
	                                ""sendReminderEmail"": 1,
	                                ""reminderEmailDesc"": ""Dear candidate, EMAL Reminder"",
	                                ""smsDesc"": ""Dear Candidate, SMS description"",
	                                ""sendReminderSms"": 1,
	                                ""reminderSmsDesc"": ""Dear Candidate, SMS reminder"",
	                                ""interviewsharableLink"": ""https://www.google.com"",
	                                ""notifyOnSubmission"":1
	                              }
	                            }
                            }";
            //Act
            var response = await testClient.PostAsync("/graphql", new StringContent(graphQLQuery, Encoding.UTF8, "application/json"));
			//Assert
			var content = await response.Content.ReadAsStringAsync();
			

			JObject joResponse = JObject.Parse(content);
			JObject ojObject = (JObject)joResponse["data"]["createInterview"];
			var interview = JsonConvert.DeserializeObject<Interview>(ojObject.ToString());

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.NotNull(interview.interview_guid );
        }

       
       
    }
}
