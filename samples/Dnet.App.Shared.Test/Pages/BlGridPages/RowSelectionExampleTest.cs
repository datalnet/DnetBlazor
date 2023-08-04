using Bunit;
using Dnet.App.Shared.Infrastructure.Entities;
using Dnet.App.Shared.Pages.BlGridPages;
using Dnet.Blazor.Components.Grid.BlgGrid;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Dnet.App.Shared.Test.Pages.BlGridPages;

public class RowSelectionExampleTest
{
    private TestContext ctx;
    private RowSelectionExampleMockedServices rowSelectionExampleMockedServices;

    public RowSelectionExampleTest()
    {
        ctx = new TestContext();

        rowSelectionExampleMockedServices = new RowSelectionExampleMockedServices();
    }

    private IRenderedComponent<RowSelectionExample> RenderComponent()
    {
        rowSelectionExampleMockedServices.RegisterMockedServices(ctx);

        return ctx.RenderComponent<RowSelectionExample>();
    }

    [Fact]
    public void GridRendersCorrectly()
    {
        var cut = RenderComponent();

        var grid = cut.FindComponent<BlgGrid<Person>>();

        //grid.MarkupMatches(GlobalMarkups.BLGRID_MARKUP);
    }

    [Fact]
    public void GridCreateColumnsCountCorrectly()
    {
        var cut = RenderComponent();

        //ctx.RenderTree.Add<BlgGrid<VesselTypeDto>>(parameters =>
        //{
        //    parameters.Add(p => p.GridOptions, cut.Instance.GridOptions);

        //    parameters.Add(p => p.GridColumns, cut.Instance.GridColumns);

        //    parameters.Add(p => p.GroupGridColumn, cut.Instance.GroupGridColumn);
        //});

        //var grid = ctx.RenderComponent<BlgGrid<Person>>(parameters =>
        //{
        //    parameters.Add(p => p.GridOptions, cut.Instance.GridOptions);

        //    parameters.Add(p => p.GridColumns, cut.Instance.GridColumns);

        //    parameters.Add(p => p.GroupGridColumn, cut.Instance.GroupGridColumn);
        //});

        var grid = cut.FindComponent<BlgGrid<Person>>();

        var instance = grid.Instance;

        var gridCells = grid.FindAll("span.blg-header-cell-text");

        var firstCellText = gridCells.First().TextContent;

        gridCells.Count.ShouldBe(2);

        firstCellText.ShouldBe("Id");
    }
}
