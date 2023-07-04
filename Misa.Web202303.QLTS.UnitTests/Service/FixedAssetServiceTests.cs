using AutoMapper;
using Misa.Web202303.QLTS.BL.Service.FixedAsset;
using Misa.Web202303.QLTS.DL.Repository.FixedAsset;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Web202303.QLTS.UnitTests.Service
{
    [TestFixture]
    public class FixedAssetServiceTests
    {
        [Test]
        public async Task GetAsync_NotFound_ReturnsException()
        {
            var id = Guid.Parse("adbdd61d-fb0d-11ed-921d-002b673830bb");
            var fixedAssetRepository = Substitute.For<IFixedAssetRepository>();
            fixedAssetRepository.GetAsync(id).ReturnsNull();
            var mapper = Substitute.For<IMapper>();

            var fixedAssetService = new FixedAssetService(fixedAssetRepository, mapper);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await fixedAssetService.GetAsync(id));
            Assert.That(ex.UserMessage, Is.EqualTo("Tài nguyên cần tìm không tồn tại."));
        }
    }
}
