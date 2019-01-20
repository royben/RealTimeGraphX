# RealTimeGraphX
RealTimeGraphX is a data type agnostic, high performance plotting library for WPF, UWP and soon, Xamarin Forms.

The library core components are built using .Net Standard which makes it portable to a range of platforms.

Typical use case is, scientific measurements applications which requires real-time display with large data volumes.

RealTimeGraphX has a number of built-in data point types (axis) like Double, Float, Int32 and TimeSpan, but you can easily implement any kind of custom data type by inheriting and implementing the mathematical logic for that type.

### Features:
- **High performance**
- **Thread safe**
- **MVVM support**
- **Any type of data point**
- **Zooming and panning**

<br/>
<hr/>
<br/>

The solution contains demo projects for WPF and UWP.

**Single Series**

![alt tag](https://github.com/royben/RealTimeGraphX/blob/master/Preview/single.png)

**Multi Series**
 
![alt tag](https://github.com/royben/RealTimeGraphX/blob/master/Preview/multi.PNG)
  
**Gradient Fill**
 
![alt tag](https://github.com/royben/RealTimeGraphX/blob/master/Preview/gradient.png)

<br/>
<hr/>
<br/>
 
The follwing diagrams demonstrates the connections between graph components and how they are implemented on each platform.

**Model**

![alt tag](https://github.com/royben/RealTimeGraphX/blob/master/Preview/schema.png)

The graph controller binds to a renderer and a surface. Data points are pushed to the controller, the controller uses the renderer in orderer to prepare and arrange the points for visual display. Finally, the controller directs the renderer to draw the points on the specific surface.


**WPF Stack Implementation**

![alt tag](https://github.com/royben/RealTimeGraphX/blob/master/Preview/stack.png)

Each platform (WPF/UWP etc.) should implement it's own *IGraphDataSeries* and *IGraphSurface*.

<br/>
<hr/>
<br/>

<h3>Example<h3>
 
 ##### Model.cs
 
 ```csharp
 
    public class ViewModel
    {
        //Graph controller with timespan as X axis and double as Y.
        public WpfGraphController<TimeSpanDataPoint, DoubleDataPoint> Controller { get; set; }

        public ViewModel()
        {
            Controller = new WpfGraphController<TimeSpanDataPoint, DoubleDataPoint>();
            Controller.Renderer = new ScrollingLineRenderer<WpfGraphDataSeries>();
            Controller.DataSeriesCollection.Add(new WpfGraphDataSeries()
            {
                Name = "Series Name",
                Stroke = Colors.Red,
            });

            //We will attach the surface using WPF binding...
            //Controller.Surface = null;
        }

        private void Start()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Thread thread = new Thread(() => 
            {
                while (true)
                {
                    //Get the current elapsed time and mouse position.
                    var y = System.Windows.Forms.Cursor.Position.Y;
                    var x = watch.Elapsed;

                    //Push the x,y to the controller.
                    Controller.PushData(x, y);

                    Thread.Sleep(30);
                }
            });
            thread.Start();
        }
    }
 
 ```

 ##### View.xaml
 
  ```xaml
  
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="1*"/>
            <RowDefinition Height="35"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="70"/>
            <ColumnDefinition Width="1*"/>
        </Grid.ColumnDefinitions>

        <Grid Grid.Column="1">
            <realTimeGraphX:WpfGraphGridLines Controller="{Binding Controller}" />
            <realTimeGraphX:WpfGraphSurface Controller="{Binding Controller}" />
        </Grid>

        <realTimeGraphX:WpfGraphAxisControl Orientation="Vertical" Controller="{Binding Controller}" StringFormat="0.0" />
        <realTimeGraphX:WpfGraphAxisControl Orientation="Horizontal" Controller="{Binding Controller}" Grid.Column="1" Grid.Row="1" StringFormat="hh\:mm\:ss"/>
    </Grid>
  
  ```
