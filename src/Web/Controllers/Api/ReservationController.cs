﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.Api
{
    public class ReservationController : BaseApiController
    {
        [HttpGet]
        public Task<IActionResult> List()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public Task<IAsyncResult> Create()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<IActionResult> Checkout()
        {
            throw new NotImplementedException();
        }
    }
}
