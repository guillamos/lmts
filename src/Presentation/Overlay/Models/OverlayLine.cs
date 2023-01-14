using System;
using Godot;
using LMTS.Presentation.Overlay.Abstract;

namespace LMTS.Presentation.Overlay.Models;

public class OverlayLine: BaseOverlayItem
{
    public OverlayLine(string reference, bool isClickable, Vector3 from, Vector3 to, Color color, decimal width) : base(reference, isClickable)
    {
        From = from;
        To = to;
        Color = color;
        Width = width;
    }

    public Vector3 From { get; set; }
    public Vector3 To { get; set; }
    public Color Color { get; set; }
    
    public decimal Width { get; set; }
}