using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RealTimeGraphX.WPF
{
  public class WpfGraphControl : Control
  {
    /// <summary>
    /// Gets or sets the graph controller.
    /// </summary>
    public IGraphController Controller
    {
      get { return (IGraphController)GetValue(ControllerProperty); }
      set { SetValue(ControllerProperty, value); }
    }
    public static readonly DependencyProperty ControllerProperty =
        DependencyProperty.Register("Controller", typeof(IGraphController), typeof(WpfGraphControl), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets a value indicating whether to display a tool tip with the current cursor value.
    /// </summary>
    public bool DisplayToolTip
    {
      get { return (bool)GetValue(DisplayToolTipProperty); }
      set { SetValue(DisplayToolTipProperty, value); }
    }
    public static readonly DependencyProperty DisplayToolTipProperty =
        DependencyProperty.Register("DisplayToolTip", typeof(bool), typeof(WpfGraphControl), new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets the string format for the X Axis.
    /// </summary>
    public String StringFormatX
    {
      get { return (String)GetValue(StringFormatXProperty); }
      set { SetValue(StringFormatXProperty, value); }
    }
    public static readonly DependencyProperty StringFormatXProperty =
        DependencyProperty.Register("StringFormatX", typeof(String), typeof(WpfGraphControl), new PropertyMetadata("0.0"));

    /// <summary>
    /// Gets or sets the string format for the Y Axis.
    /// </summary>
    public String StringFormatY
    {
      get { return (String)GetValue(StringFormatYProperty); }
      set { SetValue(StringFormatYProperty, value); }
    }
    public static readonly DependencyProperty StringFormatYProperty =
        DependencyProperty.Register("StringFormatY", typeof(String), typeof(WpfGraphControl), new PropertyMetadata("hh\\:mm\\:ss"));


    /// <summary>
    /// Gets or sets the caption of the Y axis
    /// </summary>
    public String AxisCaptionY
    {
      get { return (String)GetValue(AxisCaptionYProperty); }
      set { SetValue(AxisCaptionYProperty, value); }
    }
    public static readonly DependencyProperty AxisCaptionYProperty =
        DependencyProperty.Register("AxisCaptionY", typeof(String), typeof(WpfGraphControl), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the caption of the X axis
    /// </summary>
    public String AxisCaptionX
    {
      get { return (String)GetValue(AxisCaptionXProperty); }
      set { SetValue(AxisCaptionXProperty, value); }
    }
    public static readonly DependencyProperty AxisCaptionXProperty =
        DependencyProperty.Register("AxisCaptionX", typeof(String), typeof(WpfGraphControl), new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the brush used for the grid lines
    /// </summary>
    public Brush GridLinesBrush
    {
      get { return (Brush)GetValue(GridLinesBrushProperty); }
      set { SetValue(GridLinesBrushProperty, value); }
    }
    public static readonly DependencyProperty GridLinesBrushProperty =
        DependencyProperty.Register( "GridLinesBrush", typeof(Brush), typeof(WpfGraphControl), new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#FF2E2E2E")) ) ;

    /// <summary>
    /// Initializes the <see cref="WpfGraphControl"/> class.
    /// </summary>
    static WpfGraphControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(WpfGraphControl), new FrameworkPropertyMetadata(typeof(WpfGraphControl)));
    }
  }
}
