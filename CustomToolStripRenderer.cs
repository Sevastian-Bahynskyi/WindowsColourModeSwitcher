using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media;
using DarkModeSwitcher;
using Brushes = System.Drawing.Brushes;

public class CustomToolStripRenderer : ToolStripProfessionalRenderer
{
    private ViewModel _viewModel;
    public CustomToolStripRenderer(ViewModel viewModel)
    {
        _viewModel = viewModel;
    }
    
    protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
    {
        base.OnRenderMenuItemBackground(e);

        // Create rectangle covering the entire menu item
        Rectangle itemBounds = new Rectangle(Point.Empty, e.Item.Size);

        if (e.Item.Selected)
        {
            // Custom background color for hovered item (selected)
            e.Graphics.FillRectangle(new SolidBrush(_viewModel.SelectedItemColor), itemBounds); // Hover color
        }
        else
        {
            // Default background color for non-hovered items
            e.Graphics.FillRectangle(new SolidBrush(_viewModel.BackgroundColor), itemBounds); // Non-selected color
        }
    }
    
    protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
    {
        e.Graphics.FillRectangle(new SolidBrush(_viewModel.BackgroundColor), e.AffectedBounds); // Background color for non-selected items
    }

    protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
    {
        e.Graphics.FillRectangle(new SolidBrush(_viewModel.BackgroundColor), e.AffectedBounds); // Whole background color
    }
    
}