namespace Dnet.Blazor.Components.Overlay.Infrastructure.Models
{
    public class GlobalPositionStrategyBuilder
    {

        private string _topOffset;

        private string _bottomOffset;

        private string _leftOffset;

        private string _rightOffset;

        private string _alignItems;

        private string _justifyContent;

        private string _width;

        private string _height;

        public GlobalPositionStrategyBuilder Top(string offset)
        {
            _bottomOffset = "";
            _topOffset = offset;
            _alignItems = "flex-start";
            return this;
        }

        public GlobalPositionStrategyBuilder Bottom(string offset)
        {
            _topOffset = "";
            _bottomOffset = offset;
            _alignItems = "flex-end";
            return this;
        }

        public GlobalPositionStrategyBuilder Left(string offset)
        {
            _rightOffset = "";
            _leftOffset = offset;
            _justifyContent = "flex-start";
            return this;
        }

        public GlobalPositionStrategyBuilder Right(string offset)
        {
            _leftOffset = "";
            _rightOffset = offset;
            _justifyContent = "flex-end";
            return this;
        }

        public GlobalPositionStrategyBuilder CenterHorizontally(string offset)
        {
            _rightOffset = "";
            _leftOffset = offset;
            _justifyContent = "center";
            return this;
        }

        public GlobalPositionStrategyBuilder CenterVertically(string offset)
        {
            _bottomOffset = "";
            _topOffset = offset;
            _alignItems = "center";
            return this;
        }

        public string GetJustifyContent()
        {
            return _justifyContent;
        }

        public string GetAlignItems()
        {
            return _alignItems;
        }

        public string GetTopOffset()
        {
            return _topOffset;
        }

        public string GetBottomOffset()
        {
            return _bottomOffset;
        }

        public string GetRightOffset()
        {
            return _rightOffset;
        }

        public string GetLeftOffset()
        {
            return _leftOffset;
        }
        public string GetWidth()
        {
            return _width;
        }

        public string GetHeight()
        {
            return _height;
        }

    }
}
