using Bunit;
using Dnet.App.Shared.Infrastructure.Entities;
using Dnet.App.Shared.Pages.BlGridPages;
using Dnet.Blazor.Components.Grid.BlgGrid;
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
    public void GridCreateColumnsCountCorrectly()
    {
        var cut = RenderComponent();

        cut.Instance.GridOptions.UseVirtualization = false;

        var grid = cut.FindComponent<BlgGrid<Person>>();

        var gridCells = grid.FindAll("span.blg-header-cell-text");

        var firstCellText = gridCells.First().TextContent;

        gridCells.Count.ShouldBe(8);

        firstCellText.ShouldBe("Name");
    }
}
