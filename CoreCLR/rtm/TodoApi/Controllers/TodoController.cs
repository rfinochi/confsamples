using System.Collections.Generic;

﻿using Microsoft.AspNetCore.Mvc;

using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;

        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Item> GetAll()
        {
            return _repository.AllItems;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var item = _repository.GetById(id);
            if (item == null)
                return new NotFoundResult();

            return new ObjectResult(item);
        }

        [HttpPost]
        public void CreateItem([FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                this.Response.StatusCode = 400;
            }
            else
            {
                _repository.Add(item);

                string url = Url.RouteUrl("GetById", new { id = item.Id }, Request.Scheme, Request.Host.ToUriComponent());
                this.Response.StatusCode = 201;
                this.Response.Headers["Location"] = url;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {
            if (_repository.Delete(id))
            {
                return new StatusCodeResult(204); // 201 No Content
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}