using System;
using CSharpMath.Structures;
using Typography.OpenFont;

namespace CSharpMath.Rendering {
  public interface ICanvas<TPathWrapper> where TPathWrapper : IPath {
    float Width { get; }
    float Height { get; }
    Color DefaultColor { get; set; }
    Color? CurrentColor { get; set; }
    PaintStyle CurrentStyle { get; set; }

    TPathWrapper GetPath();
    void DrawLine(float x1, float y1, float x2, float y2, float lineThickness);
    void StrokeRect(float left, float top, float width, float height);
    void FillRect(float left, float top, float width, float height);
    void FillText(string text, float x, float y, float pointSize);
    void Save();
    void Translate(float dx, float dy);
    void Scale(float sx, float sy);
    void Restore();
  }
  public static class CanvasExtensions {
    public static void StrokeLineOutline<TPathWrapper>(this ICanvas<TPathWrapper> c, float x1, float y1, float x2, float y2, float lineThickness)
      where TPathWrapper : IPath {
      var dx = Math.Abs(x2 - x1);
      var dy = Math.Abs(y2 - y1);
      var length = (float)Math.Sqrt((double)dx * dx + (double)dy * dy);
      var halfThickness = lineThickness / 2;
      var px = dx / length * halfThickness;
      var py = dy / length * halfThickness;
      using (var p = c.GetPath()) {
        p.BeginRead(0);
        p.MoveTo(x1 - py, y1 + px);
        p.LineTo(x1 + py, y1 - px);
        p.LineTo(x2 + py, y2 - px);
        p.LineTo(x2 - py, y2 + px);
        p.CloseContour();
        p.EndRead();
      }
    }
  }
}