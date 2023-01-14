namespace LMTS.Presentation.Overlay.Abstract;

public class BaseOverlayItem: IOverlayItem
{
    public BaseOverlayItem(string reference, bool isClickable)
    {
        Reference = reference;
        IsClickable = isClickable;
    }

    public string Reference { get; set; }
    public bool IsClickable { get; set; }
}