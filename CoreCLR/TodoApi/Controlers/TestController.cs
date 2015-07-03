using System;
using System.Collections.Generic;
using System.Linq;

﻿using Microsoft.AspNet.Mvc;

using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ITodoRepository _repository;

        public TestController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public string GetAll()
        {
            string ret = "OK";
            
            try
            {
                var x = _repository.AllItems;
            }
            catch (Exception e) 
            {
                ret = e.Message ;
            }                          
            
            return ret;
        }
    }
}
