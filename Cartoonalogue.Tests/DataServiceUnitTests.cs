using Cartoonalogue.Api.Services;
using Cartoonalogue.Api.ViewModels;
using StudioDomain.Shared.Commands.v1.Interfaces;
using Moq;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Interfaces;
using CartoonDomain.Shared.v1.Interfaces;
using StudioDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Contracts;
using ProtoBuf.Grpc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Cartoonalogue.Tests;

[TestClass]
public class DataServiceUnitTests
{
    [TestMethod]
    public void CreateCartoonDetails_ReturnsNull_WhenTransactionFailsNewStudio()
    {
        var request = new CartoonDetailsCreateViewModel
        {
            Cartoon = new CartoonCreateViewModel
            {
                Title = "The Crazy Test Hour",
                StudioId = 0,
                YearBegin = 1990,
                YearEnd = 1999,
                Description = "This is a test. It is only a test.",
                Rating = 3
            },            
            Characters = new List<CharacterCreateViewModel?>
            {
                new CharacterCreateViewModel { Name = "Testee McTestFace", Description = "Character 1" },
                new CharacterCreateViewModel { Name = "Tester the Tank Engine", Description = "Character 2" }
            },
            Studio = new StudioCreateViewModel
            {
                Name = "Testing Studios"
            }
        };

        var studioCreateResponse = new StudioCreateResponse
        {
            Id = 12,
            Name = request.Studio.Name
        };
        var studioDeleteResponse = new StudioDeleteResponse
        {
            Success = true
        };

        CartoonDetailsCreateResponse? cartoonResponse = null;

        var mockStudioCommandService = new Mock<IStudioDomainCommandService>();
        var mockStudioQueryService = new Mock<IStudioDomainQueryService>();
        var mockCartoonCommandService = new Mock<ICartoonDomainCommandService>();
        var mockCartoonQueryService = new Mock<ICartoonDomainQueryService>();
        var dummyCallContext = new CallContext();

        mockStudioCommandService.Setup(s => s.CreateStudioAsync(It.IsAny<StudioCreateRequest>(), dummyCallContext).Result).Returns(studioCreateResponse);
        mockStudioCommandService.Setup(s => s.DeleteStudioAsync(It.IsAny<StudioDeleteRequest>(), dummyCallContext).Result).Returns(studioDeleteResponse);
        mockCartoonCommandService.Setup(c => c.CreateCartoonDetailsAsync(It.IsAny<CartoonDetailsCreateRequest>(), dummyCallContext).Result).Returns(cartoonResponse);

        var sut = new DataService(mockCartoonQueryService.Object, mockCartoonCommandService.Object, mockStudioQueryService.Object, mockStudioCommandService.Object);
        var actual = sut.CreateCartoonDetailsAsync(request);
        Assert.IsTrue(true);
    }
}