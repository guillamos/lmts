namespace LMTS.Presentation.Overlay.Abstract
{
    public interface IOverlayItem
    {
        public string Reference { get; set; }
        public bool IsClickable { get; set; }
    }
}