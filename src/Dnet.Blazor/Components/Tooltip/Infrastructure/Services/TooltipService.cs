using Dnet.Blazor.Components.Overlay.Infrastructure.Enums;
using Dnet.Blazor.Components.Overlay.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Overlay.Infrastructure.Models;
using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Interfaces;
using Dnet.Blazor.Components.Tooltip.Infrastructure.Models;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Tooltip.Infrastructure.Services
{
    public class TooltipService : ITooltipService, IDisposable
    {
        private readonly IOverlayService _overlayService;
        private readonly Dictionary<int, OverlayReference> _activeTooltips = new();
        private readonly Dictionary<int, System.Threading.Timer> _showTimers = new();
        private readonly Dictionary<int, System.Threading.Timer> _hideTimers = new();
        private readonly Dictionary<int, TooltipConfig> _tooltipConfigs = new();
        private readonly Dictionary<int, int> _placeholderToRealIdMap = new(); // Mapeo de placeholder ID a overlay real ID
        private readonly object _lock = new object();

        public TooltipService(IOverlayService overlayService)
        {
            _overlayService = overlayService;
        }

        public OverlayReference Show(TooltipConfig tooltipConfig, ElementReference elementReference)
        {
            return ShowInternal(null, null, tooltipConfig, elementReference);
        }

        public OverlayReference Show<TComponent>(TooltipConfig tooltipConfig, IDictionary<string, object> parameters, ElementReference elementReference) where TComponent : ComponentBase
        {
            return ShowInternal(typeof(TComponent), parameters, tooltipConfig, elementReference);
        }

        private OverlayReference ShowInternal(Type? componentType, IDictionary<string, object>? parameters, TooltipConfig tooltipConfig, ElementReference elementReference)
        {
            lock (_lock)
            {
                // Generar un ID único para este tooltip
                var placeholderId = GenerateUniqueId();
                
                // Crear una referencia placeholder que usaremos como retorno
                var placeholderRef = new OverlayReference(placeholderId);
                
                // Guardar la configuración
                _tooltipConfigs[placeholderId] = tooltipConfig;
                
                // Cancelar cualquier timer de hide pendiente
                if (_hideTimers.TryGetValue(placeholderId, out var existingHideTimer))
                {
                    existingHideTimer?.Dispose();
                    _hideTimers.Remove(placeholderId);
                }

                // Cancelar cualquier timer de show pendiente
                if (_showTimers.TryGetValue(placeholderId, out var existingShowTimer))
                {
                    existingShowTimer?.Dispose();
                    _showTimers.Remove(placeholderId);
                }

                if (tooltipConfig.ShowDelay > 0)
                {
                    // Guardar el placeholder
                    _activeTooltips[placeholderId] = placeholderRef;

                    // Programar la creación del tooltip real
                    var timer = new System.Threading.Timer(_ =>
                    {
                        lock (_lock)
                        {
                            // Solo crear si aún está en la lista (no fue cancelado)
                            if (_activeTooltips.ContainsKey(placeholderId))
                            {
                                var actualRef = Open(componentType, parameters, tooltipConfig, elementReference);
                                var realId = actualRef.GetOverlayReferenceId();
                                
                                // Mapear placeholder ID a real ID
                                _placeholderToRealIdMap[placeholderId] = realId;
                                _activeTooltips[realId] = actualRef;
                                
                                // Mover la configuración al ID real
                                _tooltipConfigs[realId] = tooltipConfig;
                                _tooltipConfigs.Remove(placeholderId);
                                
                                // Remover el placeholder
                                _activeTooltips.Remove(placeholderId);
                            }
                            _showTimers.Remove(placeholderId);
                        }
                    }, null, tooltipConfig.ShowDelay, System.Threading.Timeout.Infinite);

                    _showTimers[placeholderId] = timer;
                    return placeholderRef;
                }

                // Sin delay, crear inmediatamente
                var result = Open(componentType, parameters, tooltipConfig, elementReference);
                var resultId = result.GetOverlayReferenceId();
                
                _placeholderToRealIdMap[placeholderId] = resultId;
                _activeTooltips[resultId] = result;
                _tooltipConfigs[resultId] = tooltipConfig;
                _tooltipConfigs.Remove(placeholderId);
                
                return result;
            }
        }

        private int GenerateUniqueId()
        {
            // Usar timestamp + random para generar IDs únicos
            return Math.Abs(DateTime.UtcNow.Ticks.GetHashCode() ^ new Random().Next());
        }

        private OverlayReference Open(Type? componentType, IDictionary<string, object>? parameters, TooltipConfig tooltipConfig, ElementReference elementReference)
        {
            if (!typeof(ComponentBase).IsAssignableFrom(componentType) && componentType != null)
            {
                throw new ArgumentException($"{componentType.FullName} must be a Blazor Component");
            }

            var positions = new List<ConnectedPosition>
            {
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Center,
                    OverlayX = HorizontalConnectionPos.End,
                    OverlayY = VerticalConnectionPos.Center
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Center,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Center
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Center,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Center,
                    OverlayY = VerticalConnectionPos.Bottom
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Center,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Center,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.Start,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Bottom
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Bottom,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Top
                },
                new ConnectedPosition
                {
                    OriginX = HorizontalConnectionPos.End,
                    OriginY = VerticalConnectionPos.Top,
                    OverlayX = HorizontalConnectionPos.Start,
                    OverlayY = VerticalConnectionPos.Bottom
                }
            };

            var flexibleConnectedPositionStrategyBuilder = new FlexibleConnectedPositionStrategyBuilder()
                .WithViewportMargin(8)
                .WithFlexibleDimensions(false)
                .SetOrigin(elementReference)
                .WithPositions(positions);

            var overlayConfig = new OverlayConfig
            {
                HasBackdrop = false,
                HasTransparentBackdrop = true,
                PositionStrategy = PositionStrategy.FlexibleConnectedTo,
                FlexibleConnectedPositionStrategyBuilder = flexibleConnectedPositionStrategyBuilder,
                PanelClass = "dnet-tooltip-panel",
                Width = tooltipConfig.Width,
                Height = tooltipConfig.Height,
                MinHeight = tooltipConfig.MinHeight,
                MinWidth = tooltipConfig.MinWidth,
                MaxHeight = tooltipConfig.MaxHeight,
                MaxWidth = tooltipConfig.MaxWidth,
                ComponentType = ComponentType.ToolTip
            };

            var tooltip = new RenderFragment(x =>
            {
                x.OpenComponent(0, typeof(DnetTooltipPanel));
                x.AddAttribute(1, "Text", tooltipConfig.Text);
                x.AddAttribute(2, "TooltipClass", tooltipConfig.TooltipClass);
                x.AddAttribute(3, "TooltipColor", tooltipConfig.TooltipColor);
                x.AddAttribute(4, "MaxWidth", tooltipConfig.MaxWidth);
                x.AddAttribute(5, "MaxHeight", tooltipConfig.MaxHeight);
                x.AddAttribute(6, "ComponentType", componentType);
                x.AddAttribute(7, "Parameters", parameters);
                x.CloseComponent();
            });

            var overlayReference = _overlayService.Attach(tooltip, overlayConfig);

            return overlayReference;
        }

        public void Close(OverlayResult overlayDataResult)
        {
            if (overlayDataResult == null) return;

            var requestedId = overlayDataResult.OverlayReferenceId;

            lock (_lock)
            {
                // Resolver el ID real si es un placeholder
                var tooltipId = _placeholderToRealIdMap.TryGetValue(requestedId, out var realId) ? realId : requestedId;

                // Cancelar el timer de show si existe (usando el ID solicitado, que puede ser el placeholder)
                if (_showTimers.TryGetValue(requestedId, out var showTimer))
                {
                    showTimer?.Dispose();
                    _showTimers.Remove(requestedId);
                    _activeTooltips.Remove(requestedId);
                    _tooltipConfigs.Remove(requestedId);
                    _placeholderToRealIdMap.Remove(requestedId);
                    return; // Si aún no se mostró, solo cancelamos y salimos
                }

                // Verificar si el tooltip existe (buscar por ID real)
                if (!_activeTooltips.ContainsKey(tooltipId))
                {
                    return;
                }

                // Obtener la configuración del HideDelay
                var hideDelay = 0;
                if (_tooltipConfigs.TryGetValue(tooltipId, out var config))
                {
                    hideDelay = config.HideDelay;
                }

                if (hideDelay > 0)
                {
                    // Si ya hay un timer de hide pendiente, no hacer nada
                    if (_hideTimers.ContainsKey(tooltipId))
                    {
                        return;
                    }

                    // Crear timer para cerrar después del delay
                    var timer = new System.Threading.Timer(_ =>
                    {
                        CloseImmediate(new OverlayResult { OverlayReferenceId = tooltipId });
                    }, null, hideDelay, System.Threading.Timeout.Infinite);

                    _hideTimers[tooltipId] = timer;
                }
                else
                {
                    // Sin delay, cerrar inmediatamente
                    CloseImmediate(new OverlayResult { OverlayReferenceId = tooltipId });
                }
            }
        }

        private void CloseImmediate(OverlayResult overlayDataResult)
        {
            if (overlayDataResult == null) return;

            var tooltipId = overlayDataResult.OverlayReferenceId;

            lock (_lock)
            {
                // Cancelar cualquier timer de show pendiente
                if (_showTimers.TryGetValue(tooltipId, out var showTimer))
                {
                    showTimer?.Dispose();
                    _showTimers.Remove(tooltipId);
                }

                // Cancelar cualquier timer de hide pendiente
                if (_hideTimers.TryGetValue(tooltipId, out var hideTimer))
                {
                    hideTimer?.Dispose();
                    _hideTimers.Remove(tooltipId);
                }

                // Solo detach si el tooltip realmente existe
                if (_activeTooltips.ContainsKey(tooltipId))
                {
                    _overlayService.Detach(overlayDataResult);
                    _activeTooltips.Remove(tooltipId);
                    _tooltipConfigs.Remove(tooltipId);
                    
                    // Remover mapeo si existe
                    var placeholderId = _placeholderToRealIdMap.FirstOrDefault(x => x.Value == tooltipId).Key;
                    if (placeholderId != 0)
                    {
                        _placeholderToRealIdMap.Remove(placeholderId);
                    }
                }
            }
        }

        /// <summary>
        /// Closes all active tooltips.
        /// </summary>
        public void CloseAll()
        {
            lock (_lock)
            {
                foreach (var tooltip in _activeTooltips.Values.ToList())
                {
                    if (tooltip != null)
                    {
                        CloseImmediate(new OverlayResult { OverlayReferenceId = tooltip.GetOverlayReferenceId() });
                    }
                }

                // Limpiar todos los timers
                foreach (var timer in _showTimers.Values)
                {
                    timer?.Dispose();
                }
                _showTimers.Clear();

                foreach (var timer in _hideTimers.Values)
                {
                    timer?.Dispose();
                }
                _hideTimers.Clear();

                _activeTooltips.Clear();
                _tooltipConfigs.Clear();
                _placeholderToRealIdMap.Clear();
            }
        }

        public void Dispose()
        {
            CloseAll();
        }
    }
}
