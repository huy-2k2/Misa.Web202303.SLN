using AutoMapper;
using Castle.Core.Configuration;
using Misa.Web202303.SLN.BL.Service.FixedAsset;
using Misa.Web202303.SLN.Common.Exceptions;
using Misa.Web202303.SLN.DL.Entity;
using Misa.Web202303.SLN.DL.Repository.FixedAsset;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.SLN.UnitTests.Service
{
    [TestFixture]
    public class FixedAssetServiceTests
    {
        //[Test]
        //public async Task GetAsync_NotFound_ReturnsException()
        //{
        //    var id = Guid.Parse("adbdd61d-fb0d-11ed-921d-002b673830bb");
        //    var fixedAssetRepository = Substitute.For<IFixedAssetRepository>();
        //    fixedAssetRepository.GetAsync(id).ReturnsNull();
        //    var mapper = Substitute.For<IMapper>();

        //    var fixedAssetService = new FixedAssetService(fixedAssetRepository, mapper);

        //    var ex = Assert.ThrowsAsync<NotFoundException>(async () => await fixedAssetService.GetAsync(id));
        //    Assert.That(ex.UserMessage, Is.EqualTo("Tài nguyên cần tìm không tồn tại."));
        //}

        //[Test]
        //public async Task GetAsync_Valid_ReturnFixedAssetDto()
        //{
        //    var id = Guid.Parse("adbdd61d-fb0d-11ed-921d-002b673830bb");
        //    var fixedAssetRepository = Substitute.For<IFixedAssetRepository>();
        //    var mapper = Substitute.For<IMapper>();
        //    var fixedAssetService = new FixedAssetService(fixedAssetRepository, mapper);

        //    var fixedAsset = new FixedAsset();

        //    var fixedAssetDto = new FixedAssetDto();
           

        //    fixedAssetRepository.GetAsync(id).Returns(fixedAsset);
        //    mapper.Map<FixedAssetDto>(fixedAsset).Returns(fixedAssetDto);
        //    var actualResult = await fixedAssetService.GetAsync(id);
        //    Assert.That(actualResult, Is.EqualTo(fixedAssetDto));
        //}

        //[Test]
        //public async Task DeleteAsync_Valid_ReturnBool()
        //{
        //    // Arrange
        //    var id = Guid.Parse("adbdd61d-fb0d-11ed-921d-002b673830bb");
        //    var fixedAssetRepository = Substitute.For<IFixedAssetRepository>();
        //    var mapper = Substitute.For<IMapper>();
        //    var fixedAssetService = new FixedAssetService(fixedAssetRepository, mapper);
        //    var fixedAsset = new FixedAsset();
        //    fixedAssetRepository.GetAsync(id).Returns(fixedAsset);
        //    fixedAssetRepository.DeleteAsync(id).Returns(true);
            
        //    // Act
        //    var actualResult = await fixedAssetService.DeleteAsync(id);

        //    // Assert
        //    await fixedAssetRepository.Received(1).DeleteAsync(id);
        //    Assert.That(actualResult, Is.EqualTo(true));

        //}

        //[Test]
        //public async Task DeleteAsync_NotFound_ReturnException()
        //{
        //    var id = Guid.Parse("adbdd61d-fb0d-11ed-921d-002b673830bb");
        //    var fixedAssetRepository = Substitute.For<IFixedAssetRepository>();
        //    var mapper = Substitute.For<IMapper>();
        //    var fixedAssetService = new FixedAssetService(fixedAssetRepository, mapper);
        //    fixedAssetRepository.GetAsync(id).ReturnsNull();
        //    var ex = Assert.ThrowsAsync<NotFoundException>(async () => await fixedAssetService.DeleteAsync(id));
        //    await fixedAssetRepository.Received(0).DeleteAsync(id);
        //    Assert.That(ex.UserMessage, Is.EqualTo("Không thể xóa vì tài nguyên không tồn tại."));
        //}
    }
}
