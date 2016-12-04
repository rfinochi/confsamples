using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Xunit;

using TodoApi.Controllers;
using TodoApi.Models;

namespace TodoApi.Tests
{
    public class TodoControllerFixture : Controller
    {
        [Fact]

        public void GetAllOk()
        {
            var controller = new TodoController(new MemoryTodoRepository());
            
            Assert.Equal(1, controller.GetAll().Count());
        }

        [Fact]

        public void GetByIdOk()
        {
            var controller = new TodoController(new MemoryTodoRepository());

            var result = (Item)((ObjectResult)controller.GetById(1)).Value;

            Assert.Equal(1, result.Id);
            Assert.Equal("In Memory Task 1", result.Title);
            Assert.Equal(true, result.IsDone);
        }
            
        [Fact]

        public void DeleteOk()
        {
            var controller = new TodoController(new MemoryTodoRepository());

            controller.DeleteItem(1);    
            Assert.Equal(0, controller.GetAll().Count());
        }     
    }
}