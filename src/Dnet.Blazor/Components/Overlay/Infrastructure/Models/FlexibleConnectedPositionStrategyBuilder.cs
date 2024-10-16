using Dnet.Blazor.Components.Overlay.Infrastructure.Services;
using Microsoft.AspNetCore.Components;

namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class FlexibleConnectedPositionStrategyBuilder
    {

        private bool _isPushed;

        private bool _canPush;

        private bool _growAfterOpen;

        private bool _hasFlexibleDimensions;

        private bool _positionLocked;

        private int _viewportMargin;

        private List<CdkScrollable> _scrollables;

        private List<ConnectedPosition> _preferredPositions;

        private FlexibleConnectedPositionStrategyOrigin _origin;

        private ElementReference _originElementReference;

        private ConnectedPosition _lastPosition;

        private int _offsetX;

        private int _offsetY;

        private string _transformOriginSelector;

        private string _appliedPanelClasses;

        private Point _previousPushAmount;

        private bool _isRtl;

        public List<ConnectedPosition> Positions()
        {
            return _preferredPositions;
        }

        public int ViewportMargin()
        {
            return _viewportMargin;
        }

        public bool GrowAfterOpen()
        {
            return _growAfterOpen;
        }

        public bool HasFlexibleDimensions()
        {
            return _hasFlexibleDimensions;
        }

        public bool IsPushed()
        {
            return _isPushed;
        }

        public int GetOffsetX()
        {
            return _offsetX;
        }

        public int GetOffsetY()
        {
            return _offsetY;
        }

        public FlexibleConnectedPositionStrategyBuilder FlexibleConnectedTo(FlexibleConnectedPositionStrategyOrigin origin)
        {
            _origin = origin; ;
            return this;
        }

        /*
         * Sets the list of Scrollable containers that host the origin element so that
         * on reposition we can evaluate if it or the overlay has been clipped or outside view. Every
         * Scrollable must be an ancestor element of the strategy's origin element.
         */
        public FlexibleConnectedPositionStrategyBuilder WithScrollableContainers(List<CdkScrollable> scrollables)
        {
            _scrollables = scrollables;
            return this;
        }

        /*
         * Adds new preferred positions.
         * @param positions List of positions options for this overlay.
         */
        public FlexibleConnectedPositionStrategyBuilder WithPositions(List<ConnectedPosition> connectedPositions)
        {
            _preferredPositions = connectedPositions;

            var isOnPositions = connectedPositions.Contains(_lastPosition);

            if (!isOnPositions) _lastPosition = null;

            ValidatePositions();

            return this;
        }

        /*
         * Sets a minimum distance the overlay may be positioned to the edge of the viewport.
         * @param margin Required margin between the overlay and the viewport edge in pixels.
         */
        public FlexibleConnectedPositionStrategyBuilder WithViewportMargin(int margin)
        {
            _viewportMargin = margin;
            return this;
        }

        /* Sets whether the overlay's width and height can be constrained to fit within the viewport. */
        public FlexibleConnectedPositionStrategyBuilder WithFlexibleDimensions(bool flexibleDimensions = true)
        {
            _hasFlexibleDimensions = flexibleDimensions;
            return this;
        }

        /* Sets whether the overlay can be pushed on-screen if none of the provided positions fit. */
        public FlexibleConnectedPositionStrategyBuilder WithGrowAfterOpen(bool growAfterOpen = true)
        {
            this._growAfterOpen = growAfterOpen;
            return this;
        }

        /* Sets whether the overlay can be pushed on-screen if none of the provided positions fit. */
        public FlexibleConnectedPositionStrategyBuilder withPush(bool canPush = true)
        {
            _canPush = canPush;
            return this;
        }

        /*
         * Sets whether the overlay's position should be locked in after it is positioned
         * initially. When an overlay is locked in, it won't attempt to reposition itself
         * when the position is re-applied (e.g. when the user scrolls away).
         * @param isLocked Whether the overlay should locked in.
         */
        public FlexibleConnectedPositionStrategyBuilder WithLockedPosition(bool isLocked = true)
        {
            _positionLocked = isLocked;
            return this;
        }

        /*
         * Sets the default offset for the overlay's connection point on the x-axis.
         * @param offset New offset in the X axis.
         */
        public FlexibleConnectedPositionStrategyBuilder WithDefaultOffsetX(int offset = 0)
        {
            _offsetX = offset;
            return this;
        }

        /*
         * Sets the default offset for the overlay's connection point on the y-axis.
         * @param offset New offset in the Y axis.
         */
        public FlexibleConnectedPositionStrategyBuilder WithDefaultOffsetY(int offset = 0)
        {
            _offsetY = offset;
            return this;
        }

        /*
         * Configures that the position strategy should set a `transform-origin` on some elements
         * inside the overlay, depending on the current position that is being applied. This is
         * useful for the cases where the origin of an animation can change depending on the
         * alignment of the overlay.
         * @param selector CSS selector that will be used to find the target
         *    elements onto which to set the transform origin.
         */
        public FlexibleConnectedPositionStrategyBuilder WithTransformOriginOn(string selector)
        {
            _transformOriginSelector = selector;
            return this;
        }

        /*
         * Sets the origin, relative to which to position the overlay.
         * Using an element origin is useful for building components that need to be positioned
         * relatively to a trigger (e.g. dropdown menus or tooltips), whereas using a point can be
         * used for cases like contextual menus which open relative to the user's pointer.
         * @param origin Reference to the new origin.
         */
        public FlexibleConnectedPositionStrategyBuilder SetOrigin(ElementReference originElementReference)
        {
            _originElementReference = originElementReference;
            return this;
        }

        public ElementReference Origin(ElementReference originElementReference)
        {
            return _originElementReference;
        }

        private void ValidatePositions()
        {
            var preferredPositions = this._preferredPositions;

            if (!preferredPositions.Any())
            {
                throw new Exception("FlexibleConnectedPositionStrategy At least one position is required");
            }
        }

        /* Returns the ClientRect of the current origin. */
        public async Task<ClientRect> GetOriginRect(DnetOverlayInterop dnetOverlayInterop)
        {
            if (string.IsNullOrEmpty(_originElementReference.Id)) return null;

            _origin = await dnetOverlayInterop.GetBoundingClientRect(_originElementReference);

            var originRect = new ClientRect
            {
                Top = _origin.Top,
                Left = _origin.Left,
                Right = _origin.Left + _origin.Width,
                Bottom = _origin.Top + _origin.Height,
                Width = _origin.Width,
                Height = _origin.Height
            };

            return originRect;
        }

        /* Gets how well an overlay at the given point will fit within the viewport. */
        public OverlayFit GetOverlayFit(Point point, ClientRect overlay, ClientRect viewport, ConnectedPosition position)
        {

            var x = point.X;
            var y = point.Y;

            var offsetX = GetOffset(position, "x");
            var offsetY = GetOffset(position, "y");

            // Account for the offsets since they could push the overlay out of the viewport.
            if (offsetX != null) x += (double)offsetX;

            if (offsetY != null) y += (double)offsetY;

            // How much the overlay would overflow at this position, on each side.
            var leftOverflow = 0 - x;

            var rightOverflow = (x + overlay.Width) - viewport.Width;

            var topOverflow = 0 - y;

            var bottomOverflow = (y + overlay.Height) - viewport.Height;

            // Visible parts of the element on each axis.
            var visibleWidth = this.SubtractOverflows(overlay.Width, new List<double> { leftOverflow, rightOverflow });

            var visibleHeight = this.SubtractOverflows(overlay.Height, new List<double> { topOverflow, bottomOverflow });

            var visibleArea = visibleWidth * visibleHeight;

            return new OverlayFit()
            {
                VisibleArea = visibleArea,
                IsCompletelyWithinViewport = (overlay.Width * overlay.Height) == visibleArea,
                FitsInViewportVertically = visibleHeight == overlay.Height,
                FitsInViewportHorizontally = visibleWidth == overlay.Width,
            };
        }

        /* Determines whether the overlay uses exact or flexible positioning. */
        public bool HasExactPosition()
        {
            return !_hasFlexibleDimensions || _isPushed;
        }

        /* Whether the we're dealing with an RTL context */
        public bool IsRtl()
        {
            //return _overlayRef.getDirection() === 'rtl';
            return _isRtl;
        }

        public double? GetOffset(ConnectedPosition position, string axis)
        {
            if (axis == "x")
            {
                // We don't do something like `position['offset' + axis]` in
                // order to avoid breking minifiers that rename properties.
                return position.OffsetX ?? _offsetX;
            }

            return position.OffsetY ?? _offsetY;
        }

        /** Subtracts the amount that an element is overflowing on an axis from its length. */
        public double SubtractOverflows(double length, List<double> overflows)
        {

            var amount = overflows.Aggregate(length, (acc, x) => acc - Math.Max(x, 0));

            return amount;
        }

    }
}
