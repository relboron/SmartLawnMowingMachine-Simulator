namespace SLMM.Api
{
    public class Garden
    {
        #region PRIVATE FIELDS
        private readonly uint width;
        private readonly uint length;
        #endregion

        #region CTOR
        public Garden(uint width, uint length)
        {
            this.width = width > 0 ? width : (uint)1;
            this.length = length > 0 ? length : (uint)1;
        }
        #endregion

        #region PUBLIC PROPERTIES
        public virtual bool IsValidPosition(uint x, uint y)
        {
            return x >= 1 && x <= width && y >= 1 && y <= length;
        }
        #endregion
    }
}
