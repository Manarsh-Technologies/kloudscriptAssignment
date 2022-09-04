using kloudscript.Test.API.Controllers;
using kloudscript.Test.API.Entity;
using kloudscript.Test.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace kloudScript.NUnit.ApiTest
{
    public class MemCacheCapControllerTest
    {
        
         [SetUp]
        public void Setup()
        {
           
        }

        [Test]
        public void ValidateCacheMemoryEntityTest()
        { 
            CacheMemoryEntity cacheMemoryEntity = new CacheMemoryEntity();
            cacheMemoryEntity.ExpirationTime = 10;
            cacheMemoryEntity.CacheKey = "SV_Memory_Test";
            cacheMemoryEntity.ExpirationTime = 10;
            cacheMemoryEntity.CacheValue = "Shailesh Memory Test";

            var context = new ValidationContext(cacheMemoryEntity, null, null);
            var results = new List<ValidationResult>(); 
            var isModelStateValid = Validator.TryValidateObject(cacheMemoryEntity, context, results, true); 

            Assert.AreEqual(true, isModelStateValid);
        }
        [Test]
        public async Task SetValueInMemoryTest()
        {
            //Arrange
            var mockLogger = new Mock<ILogger<MemCacheCapController>>().Object;   
             var memorymock = new Mock<IMemeoryConfigService>().Object;  
             var controller = new MemCacheCapController(memorymock,mockLogger);


            CacheMemoryEntity cacheMemoryEntity = new CacheMemoryEntity();
            cacheMemoryEntity.ExpirationTime = 10;
            cacheMemoryEntity.CacheKey = "SV_Memory_Test";
            cacheMemoryEntity.ExpirationTime = 10;
            cacheMemoryEntity.CacheValue = "Shailesh Memory Test";
            
            //Act
            var result = await controller.SetValueInMemory(cacheMemoryEntity);
            var viewResult = (ObjectResult)result;

            //Assert
            //Assert.AreEqual(HttpStatusCode.OK, result);
            Assert.AreEqual((int)HttpStatusCode.OK, viewResult.StatusCode);

            //Act
            result = await controller.SetValueInMemory(null);
            viewResult = (ObjectResult)result;
            Assert.AreEqual((int)HttpStatusCode.BadRequest, viewResult.StatusCode);

            //var model = Assert.IsAssignableFrom(typeof(ResponseEntity),viewResult.);
        }
    }
}