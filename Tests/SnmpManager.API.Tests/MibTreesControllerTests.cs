using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using SnmpManager.API.Controllers;
using Xunit;

namespace SnmpManager.API.Tests
{
    public class MibTreesControllerTests
    {
        private Mock<IMibTreeRepository> _mibTreeRepositoryMock;

        public MibTreesControllerTests()
        {
        }
        
        [Fact]
        public void Get_loaded_mib_tree_success()
        {
            //Arrange
            var fakeMib = "1.2.3.4.5.6.7.8.9";
            
            _mibTreeRepositoryMock = new Mock<IMibTreeRepository>();
            _mibTreeRepositoryMock.Setup(x => x.GetRootTree()).Returns(new MibTree {Mib = fakeMib});
            var controller = new MibTreesController(_mibTreeRepositoryMock.Object);
            
            //Run
            IActionResult result = controller.Get();
            
            //Assert
            Assert.NotNull(result);
            var okResult = result.ShouldBeOfType<OkObjectResult>();
            var mibTree = okResult.Value.ShouldBeOfType<MibTree>();
            mibTree.Mib.ShouldBe(fakeMib);
        }

        [Fact]
        public void Post_mib_file_success()
        {
            //Arrange
            var fakeMib = "1.2.3.4.5.6.7.8.9";
            
            _mibTreeRepositoryMock = new Mock<IMibTreeRepository>();
            _mibTreeRepositoryMock.Setup(x => x.GetRootTree()).Returns(new MibTree {Mib = fakeMib});
            var contextMock = new Mock<ControllerContext>();
            contextMock.Setup(x => x.HttpContext.Request.Form.Files).Returns(new FormFileCollection());
            
            var controller = new MibTreesController(_mibTreeRepositoryMock.Object);
            controller.ControllerContext = contextMock.Object;
            
            IActionResult result = controller.PostFiles();
            
            Assert.NotNull(result);

            var acceptedResult = result.ShouldBeOfType<AcceptedResult>();
            
            Assert.Equal(fakeMib, acceptedResult.Value); 
        }
    }
}