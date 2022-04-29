using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Interfaces;
using Dnet.Blazor.Components.AdminDashboard.Infrastructure.Services;
using Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Interfaces;
using Dnet.Blazor.Components.ConnectedPanel.Infrastructure.Services;
using Dnet.Blazor.Components.Dialog.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Dialog.Infrastructure.Services;
using Dnet.Blazor.Components.FloatingPanel.Infrastructure.Interfaces;
using Dnet.Blazor.Components.FloatingPanel.Infrastructure.Services;
using Dnet.Blazor.Components.Grid.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Grid.Infrastructure.Services;
using Dnet.Blazor.Components.ImageEditor.Infrastructure.Services;
using Dnet.Blazor.Components.List;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Paginator;
using Dnet.Blazor.Components.Spinner.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Spinner.Infrastructure.Services;
using Dnet.Blazor.Components.Toast.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Toast.Infrastructure.Services;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Dnet.Blazor.Infrastructure.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDnetBlazor(this IServiceCollection services)
        {
            services.AddScoped(typeof(IOverlayService), typeof(OverlayService));

            services.AddScoped(typeof(IBlGridMessageService<>), typeof(BlGridMessageService<>));

            services.AddScoped(typeof(ISpinnerService), typeof(SpinnerService));

            services.AddScoped(typeof(IToastService), typeof(ToastService));

            services.AddScoped(typeof(DnetListDragAndDropService<>), typeof(DnetListDragAndDropService<>));

            services.AddScoped(typeof(IThemeMessageService<>), typeof(ThemeMessageService<>));

            services.AddTransient<IViewportRuler, ViewportRuler>();

            services.AddTransient<DnetOverlayInterop, DnetOverlayInterop>();
            
            services.AddTransient(typeof(IConnectedPanelService), typeof(ConnectedPanelService));
            
            services.AddTransient(typeof(IDialogService), typeof(DialogService));
            
            services.AddTransient(typeof(IFloatingPanelService), typeof(FloatingPanelService));
            
            services.AddTransient(typeof(IGrouping<>), typeof(Grouping<>));

            services.AddTransient(typeof(ISorting<>), typeof(Sorting<>));

            services.AddTransient(typeof(IFiltering<>), typeof(Filtering<>));

            services.AddTransient(typeof(IPaginator), typeof(Paginator));
            
            services.AddTransient(typeof(IAdvancedFiltering<>), typeof(AdvancedFiltering<>));

            services.AddTransient<IStyleService, StyleService>();

            services.AddTransient<IImageEditorService, ImageEditorService>();
            
            services.AddTransient(typeof(ITooltipService), typeof(TooltipService));

            return services;
        }
    }
}
