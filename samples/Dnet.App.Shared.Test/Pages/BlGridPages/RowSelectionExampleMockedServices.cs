using Bunit;
using Dnet.App.Shared.Infrastructure.Entities;
using Dnet.App.Shared.Infrastructure.Services;
using Dnet.App.Shared.Pages.BlGridPages;
using Dnet.Blazor.Components.Grid.Infrastructure.Entities;
using Dnet.Blazor.Components.Grid.Infrastructure.Enums;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Grid.Infrastructure.Services;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Paginator;
using Dnet.Blazor.Infrastructure.Models.SearchModels;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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

        var gridConfigServiceMock = new Mock<IGridConfigurationService>();

        gridConfigServiceMock.Setup(service => service.GetGridOptions()).Returns(new GridOptions<Person>()
        {
            HeaderHeight = 60,
            RowHeight = 40,
            GridClass = "cvs-bl-grid",
            CheckboxSelectionColumn = true,
            SuppressRowClickSelection = true,
            RowMultiSelectWithClick = false,
            SuppressRowDeselection = false,
            RowSelectionType = RowSelectionType.Multiple,
            EnableGrouping = true,
            EnableAdvancedFilter = true,
            GroupDefaultExpanded = false,
            UseVirtualization = false
        });

        gridConfigServiceMock.Setup(service => service.GetGridColumns()).Returns(() =>
        {
            var height = 40;
            var width = 100;
            var canGrow = 1;

            return new List<GridColumn<Person>>
            {
                 new GridColumn<Person> {
                            ColumnId = 1,
                            ColumnOrder = 1,
                            HeaderName = "Name",
                            DataField = "Name",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Name,
                            EnableAdvancedFilter = true,
                            CellDataType = CellDataType.Text,
                        },
                        new GridColumn<Person> {
                            ColumnId = 2,
                             ColumnOrder = 2,
                            HeaderName = "Gender",
                            DataField = "Gender",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Gender
                        },
                        new GridColumn<Person> {
                            ColumnId = 3,
                            ColumnOrder = 3,
                            HeaderName = "IsActive",
                            DataField = "IsActive",
                            Width= 150,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.IsActive,
                            AlingContent = AlingContent.Center
                        },
                        new GridColumn<Person> {
                            ColumnId = 4,
                            ColumnOrder = 4,
                            HeaderName = "Age",
                            DataField = "Age",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Age,
                            AlingContent = AlingContent.Center,
                            CellDataType = CellDataType.Number,
                        },
                        new GridColumn<Person> {
                            ColumnId = 5,
                            ColumnOrder = 5,
                            HeaderName = "Company",
                            DataField = "Company",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Company,
                        },
                        new GridColumn<Person> {
                            ColumnId = 6,
                            ColumnOrder = 6,
                            HeaderName = "Email",
                            DataField = "Email",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Email,
                        },
                          new GridColumn<Person> {
                            ColumnId = 7,
                            ColumnOrder = 7,
                            HeaderName = "Phone",
                            DataField = "Phone",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Phone
                        },
                           new GridColumn<Person> {
                            ColumnId = 8,
                            ColumnOrder = 8,
                            HeaderName = "Address",
                            DataField = "Address",
                            Width= width,
                            Height= height,
                            CanGrow = canGrow,
                            CellDataFn = (x) => x.RowData.Address
                        }
                };
        });

        ctx.Services.AddSingleton(gridConfigServiceMock.Object);
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
