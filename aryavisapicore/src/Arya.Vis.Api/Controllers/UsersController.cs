using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arya.Vis.Container.Web.Security;
using Arya.Vis.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Arya.Vis.Api.Controllers
{
    [Route("api/v3/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthConstants.AuthSchemes)]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger,
                               IUserService userService)
                        
        {
            _logger = logger;
            _userService = userService;
        }
    }
}