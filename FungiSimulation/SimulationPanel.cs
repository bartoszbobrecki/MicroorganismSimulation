using FungiSimulation;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Windows.Forms;

public class SimulationPanel : Panel
{
    private int BaseCellSize = 20;
    private int CellSize; // Size of each cell in the grid
    private int gridWidth; // Number of cells in width
    private int gridHeight; // Number of cells in height
    private bool[,] gridState; // 2D array to hold the state of each cell
    private Point dragStart; // Start point for dragging
    private bool isDragging = false;
    private SimulationMode simulationMode;
    public SimulationMode SimulationMode
    {
        get => simulationMode;
        set
        {
            if (simulationMode != value)
            {
                form.ChangeButtonHighlights(simulationMode,value);
                simulationMode = value;
            }
        }
    }

    private Point lastMousePosition;
    private int offsetX;
    private int offsetY;
    private readonly FungiSimulationForm form;
    private Bitmap gridBitmap;
    private float zoomFactor = 1.0f;


    public SimulationPanel(FungiSimulationForm _form)
    {
        CellSize = BaseCellSize;
        gridHeight = 200;
        gridWidth = gridHeight * (Width/Height);

        offsetX = -(gridWidth * CellSize + 1) / 2;
        offsetY = -(gridHeight * CellSize + 1) / 2;
        // Initialize the grid state
        gridState = new bool[gridWidth, gridHeight];

        gridBitmap = new Bitmap(gridWidth * CellSize+1, gridHeight * CellSize+1);

        RenderGridToBitmap();

        // Set the panel to enable double buffering for smoother rendering
        this.DoubleBuffered = true;

        // Subscribe to mouse events
        this.MouseDown += SimulationPanel_MouseDown;
        this.MouseMove += SimulationGrowthPanel_MouseMove;
        this.MouseUp += SimulationGrowthPanel_MouseUp;
        this.Paint += SimulationGrowthPanel_Paint;

        this.MouseDown += SimulationPanel_MouseDownFungiGrowth;
        this.MouseMove += SimulationPanel_MouseMoveFungiGrowth;
        SimulationMode = SimulationMode.Cursor;
        form = _form;
    }

    private void SimulationPanel_MouseMoveFungiGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Organisms)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                gridState[cellX, cellY] = true;
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });



        }
    }

    private void SimulationPanel_MouseDownFungiGrowth(object? sender, MouseEventArgs e)
    {
        if(e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Organisms)
        {

            int cellX = (e.X-offsetX) / CellSize;
            int cellY = (e.Y-offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                gridState[cellX, cellY] = true; 
            }

            RenderChangeOnClick(cellX, cellY);

            Invalidate(new Rectangle(0, 0, Width, Height));

        }
    }

    private void RenderChangeOnClick(int cellX,int cellY)
    {
        using Graphics g = Graphics.FromImage(gridBitmap);

        Color cellColor = ColorTranslator.FromHtml("#1544ed");

        using Brush brush = new SolidBrush(cellColor);

        g.FillRectangle(brush, cellX*BaseCellSize+1, cellY * BaseCellSize+1, BaseCellSize-1, BaseCellSize-1);

    }

    private void RenderGridToBitmap()
    {
        using Graphics g = Graphics.FromImage(gridBitmap);


        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(Color.FromArgb(30, 30, 30)))
                {
                    g.FillRectangle(brush, x * CellSize, y * CellSize, CellSize, CellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * CellSize, y * CellSize, CellSize, CellSize);
            }
        }
    }

    private void SimulationGrowthPanel_Paint(object sender, PaintEventArgs e)
    {

        e.Graphics.DrawImage(gridBitmap, new Rectangle(offsetX, offsetY, (int)(gridBitmap.Width * zoomFactor), (int)(gridBitmap.Height * zoomFactor)));
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if ((gridHeight * (CellSize - 1) + 1 <= Height && e.Delta < 0) || (CellSize >= 20 && e.Delta > 0))
        {
            return;
        }

        var scrollAmount = e.Delta > 0 ? 1 : -1;

        var currentZoomFactor = (CellSize + scrollAmount) / (float)CellSize;

        zoomFactor *= currentZoomFactor;


        offsetX = (int)((offsetX - e.X) * currentZoomFactor + e.X);
        offsetY = (int)((offsetY - e.Y) * currentZoomFactor + e.Y);


        CellSize += scrollAmount;

        offsetX = Math.Max(Math.Min(offsetX, 0), -(gridWidth * CellSize + 1 - Width));
        offsetY = Math.Max(Math.Min(offsetY, 0), -(gridHeight * CellSize + 1 - Height));

        Invalidate(new Rectangle(0, 0, Width, Height));
    }

    private void SimulationPanel_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Cursor)
        {
            isDragging = true; // Start dragging
            lastMousePosition = e.Location; // Record the last mouse position
        }
    }

    private void SimulationGrowthPanel_MouseMove(object sender, MouseEventArgs e)
    {


        if (isDragging)
        {
            // Calculate the difference in mouse position
            int deltaX = e.X - lastMousePosition.X;
            int deltaY = e.Y - lastMousePosition.Y;

            if (Math.Abs(deltaX) > 1 || Math.Abs(deltaY) > 1)
            {
                offsetX += deltaX;
                offsetY += deltaY;

                offsetX = Math.Max(Math.Min(offsetX, 0), -(gridWidth * CellSize+1 - Width));
                offsetY = Math.Max(Math.Min(offsetY, 0), -(gridHeight * CellSize+1 - Height));

                lastMousePosition = e.Location;

                // Only invalidate the portion that has moved
                Invalidate(new Rectangle(0, 0, Width, Height));
            }

        }
        
    }

    private void SimulationGrowthPanel_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Cursor)
        {
            isDragging = false; // Stop dragging
        }
    }
}