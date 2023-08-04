using Bunit;
using Dnet.App.Shared.Infrastructure.Entities;
using Dnet.App.Shared.Infrastructure.Services;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Grid.Infrastructure.Services;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Paginator;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dnet.App.Shared.Test.Pages.BlGridPages;

public class RowSelectionExampleMockedServices
{
    public void RegisterMockedServices(TestContext ctx)
    {
        var mockIBlGridMessageService = new Mock<IBlGridMessageService<List<RowNode<Person>>>>();

        var mockAdvancedFiltering = new Mock<IAdvancedFiltering<Person>>();

        var mockPaginationService = new Mock<IPaginator>();

        var mockGroupingService = new Mock<IGrouping<Person>>();

        var mockFilteringService = new Mock<IFiltering<Person>>();

        var mockSortingService = new Mock<ISorting<Person>>();

        var mockPersonService = new Mock<IPersonService>();

		var mockBlGridInteropService = new Mock<IBlGridInterop<Person>>();

		List<Person> mockPersons = new() { new Person { Name = "Test", Age = 30 } };

        mockPersonService
            .Setup(service => service.GetPersons())
            .ReturnsAsync(mockPersons);

        ctx.Services.AddSingleton(mockPersonService.Object);

        ctx.Services.AddSingleton(mockIBlGridMessageService.Object);

        ctx.Services.AddTransient(typeof(IGrouping<>), typeof(Grouping<>));

        ctx.Services.AddTransient(typeof(ISorting<>), typeof(Sorting<>));

        ctx.Services.AddTransient(typeof(IFiltering<>), typeof(Filtering<>));

        ctx.Services.AddTransient(typeof(IPaginator), typeof(Paginator));

        ctx.Services.AddTransient(typeof(IAdvancedFiltering<>), typeof(AdvancedFiltering<>));

        ctx.Services.AddTransient<DnetOverlayInterop, DnetOverlayInterop>();

        ctx.Services.AddScoped(typeof(IOverlayService), typeof(OverlayService));

		ctx.Services.AddSingleton(mockBlGridInteropService.Object);

        ctx.JSInterop.SetupVoid("blginterop.init", _ => true);

		ctx.JSInterop.SetupVoid("blginterop.dispose", _ => true);

		//ctx.Services.AddTransient(typeof(IBlGridInterop<>), typeof(BlGridInterop<>));

		//ctx.JSInterop.Setup<int>("blginterop.getHeaderWidth").SetResult(800);

		//ctx.Services.AddSingleton(mockAdvancedFiltering.Object);

		//ctx.Services.AddSingleton(mockPaginationService.Object);

		//ctx.Services.AddSingleton(mockGroupingService.Object);

		//ctx.Services.AddSingleton(mockFilteringService.Object);

		//ctx.Services.AddSingleton(mockSortingService.Object);
	}
}


// Crear un mock de HttpMessageHandler
//var mockHandler = new Mock<HttpMessageHandler>();

//mockHandler
//  .Protected()
//  .Setup<Task<HttpResponseMessage>>(
//     "SendAsync",
//     ItExpr.IsAny<HttpRequestMessage>(),
//     ItExpr.IsAny<CancellationToken>())
//  .ReturnsAsync(new HttpResponseMessage()
//  {
//      StatusCode = HttpStatusCode.OK,
//      Content = new StringContent("[{'id':1,'value':'1'}]"),
//  });

//var httpClient = new HttpClient(mockHandler.Object);

//var mockFactory = new Mock<IHttpClientFactory>();
//mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

//ctx.Services.AddSingleton(mockFactory.Object);
