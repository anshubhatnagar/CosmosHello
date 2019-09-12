using Cosmos.Hello.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosmos.Hello.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        private DbContext _dbContext;

        public ProductsController()
        {
            _dbContext = new DbContext(new DbSettings());
        }

        [HttpGet]
        [ActionName("Get")]
        public async Task<IEnumerable<PlController>> GetAllAsync()
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            return await _dbContext.GetItemsAsync();
        }

        [HttpGet]
        public string Get(string id)
        {
            return "Not Implemented. Use Get without Parameters instead.";
        }

        [HttpPost]
        [ActionName("Post")]
        public async Task Update([FromBody]PlController value)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.UpdateItemAsync(value, value.Id, value.Name);
        }

        [HttpPut]
        [ActionName("Put")]
        public async Task Insert([FromBody]PlController value)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.AddItemToContainerAsync(value);
        }

        // HTTP Verb Delete not enabled in IIS Express
        [HttpPost]
        [Route("api/Products/delete/{id}/{name}")]
        [ActionName("Delete")]
        public async Task Delete(string id, string name)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.DeleteItemAsync(id, name);
        }
    }
}