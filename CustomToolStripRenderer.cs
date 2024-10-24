using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using DarkModeSwitcher;
using Brushes = System.Drawing.Brushes;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    private ThemeColor _themeColor;
    public CustomToolStripRenderer(ThemeColor themeColor)
    {
        _themeColor = themeColor;
    }

    // protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
    // {
    //     if (e.Item.Selected)
    //     {
    //         e.Graphics.FillRectangle(new SolidBrush(_themeColor.selectedItemColor), e.Item.Bounds); // Color for selected item
    //     }
    //     else
    //     {
    //         e.Graphics.FillRectangle(new SolidBrush(_themeColor.backgroundColor), e.Item.Bounds); // Background color for non-selected items
    //     }
    // }
    //
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        base.OnRenderMenuItemBackground(e);

        // Create rectangle covering the entire menu item
        Rectangle itemBounds = new Rectangle(Point.Empty, e.Item.Size);

        if (e.Item.Selected)
        {
            // Custom background color for hovered item (selected)
            e.Graphics.FillRectangle(new SolidBrush(_themeColor.selectedItemColor), itemBounds); // Hover color
        }
        else
        {
            // Default background color for non-hovered items
            e.Graphics.FillRectangle(new SolidBrush(_themeColor.backgroundColor), itemBounds); // Non-selected color
        }
    }
    
    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
        e.Graphics.FillRectangle(new SolidBrush(_themeColor.backgroundColor), e.AffectedBounds); // Background color for non-selected items
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        e.Graphics.FillRectangle(new SolidBrush(_themeColor.backgroundColor), e.AffectedBounds); // Whole background color
    }
    
}