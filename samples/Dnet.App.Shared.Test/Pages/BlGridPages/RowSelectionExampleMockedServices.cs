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
using NSubstitute;

namespace Dnet.App.Shared.Test.Pages.BlGridPages;

public class RowSelectionExampleMockedServices
{
    public void RegisterMockedServices(TestContext ctx)
    {
        var mockIBlGridMessageService = Substitute.For<IBlGridMessageService<List<RowNode<Person>>>>();

        var mockAdvancedFiltering = Substitute.For<IAdvancedFiltering<Person>>();

        var mockPaginationService = Substitute.For<IPaginator>();

        var mockGroupingService = Substitute.For<IGrouping<Person>>();

        var mockFilteringService = Substitute.For<IFiltering<Person>>();

        var mockSortingService = Substitute.For<ISorting<Person>>();

        var mockPersonService = Substitute.For<IPersonService>();

        var mockBlGridInteropService = Substitute.For<IBlGridInterop<Person>>();

        List<Person> mockPersons = new() { new Person { Name = "Test", Age = 30 } };

        mockPersonService.GetPersons().Returns(mockPersons);

        ctx.Services.AddSingleton(mockPersonService);

        ctx.Services.AddSingleton(mockIBlGridMessageService);

        ctx.Services.AddTransient(typeof(IGrouping<>), typeof(Grouping<>));

        ctx.Services.AddTransient(typeof(ISorting<>), typeof(Sorting<>));

        ctx.Services.AddTransient(typeof(IFiltering<>), typeof(Filtering<>));

        ctx.Services.AddTransient(typeof(IPaginator), typeof(Paginator));

        ctx.Services.AddTransient(typeof(IAdvancedFiltering<>), typeof(AdvancedFiltering<>));

        ctx.Services.AddTransient<DnetOverlayInterop, DnetOverlayInterop>();

        ctx.Services.AddScoped(typeof(IOverlayService), typeof(OverlayService));

        ctx.Services.AddSingleton(mockBlGridInteropService);

        ctx.JSInterop.SetupVoid("blginterop.init", _ => true);

        ctx.JSInterop.SetupVoid("blginterop.dispose", _ => true);

        var gridConfigServiceMock = Substitute.For<IGridConfigurationService>();

        gridConfigServiceMock.GetGridOptions().Returns(new GridOptions<Person>()
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

        var height = 40;
        var width = 100;
        var canGrow = 1;

        gridConfigServiceMock.GetGridColumns().Returns(new List<GridColumn<Person>>
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
        });

        ctx.Services.AddSingleton(gridConfigServiceMock);
    }
}