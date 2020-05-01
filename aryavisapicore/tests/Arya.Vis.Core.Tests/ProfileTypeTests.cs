using Arya.Vis.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Arya.Vis.Core.Tests
{

    public class ProfileTypeTests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private string currentMethod = string.Empty;
        public ProfileTypeTests(ITestOutputHelper output)
        {
            _output = output;
        }
        public static List<string> profileTypes()
        {            
            List<string> profiles = new List<string>();
            foreach (var type in Enum.GetValues(typeof(ProfileType)))
            {
                profiles.Add(type.ToString());
            }
            return profiles;
        }

        [Fact]
        [Trait("Category", "AryaVis")]
        public void HaveVISProfile_ShouldPass()
        {
            currentMethod = MethodBase.GetCurrentMethod().Name;
            _output.WriteLine("Executing Test case HaveVISProfile_ShouldPass");
            var lstProfiles = profileTypes();

            Assert.Contains(ProfileType.AryaVis.ToString(), lstProfiles);
        }

        [Fact(Skip ="Enum is having value")]
        public void DoesNotHaveVISProfile_ShouldFail()
        {
            var lstProfiles = profileTypes();

            Assert.DoesNotContain(ProfileType.AryaVis.ToString(), lstProfiles);
        }

        [Fact]
        [Trait("Category", "AryaVis")]
        public void ContainHaveVISProfile_ShouldPass()
        {
            currentMethod = MethodBase.GetCurrentMethod().Name;
            var lstProfiles = profileTypes();

            Assert.Contains(lstProfiles, profile=> profile.Contains(ProfileType.AryaVis.ToString()));
        }

        public void Dispose()
        {
            _output.WriteLine("Disposing function : " + currentMethod);
        }
    }
}
