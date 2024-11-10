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
    private int[,,] gridState; // 2D array to hold the state of each cell
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
                if ( (int)value != 0 && currentGrid != (int)value - 1)
                {

                    currentGrid = (int)value - 1;

                    Task.Run( () => {
                        RenderGridToBitmap();
                    } );
                    

                }

                simulationMode = value;
                
            }
        }
    }

    private int currentGrid = 0;

    private Point lastMousePosition;
    private int offsetX;
    private int offsetY;
    private readonly FungiSimulationForm form;
    private Bitmap[] gridBitmap;
    private float zoomFactor = 1.0f;




    public SimulationPanel(FungiSimulationForm _form)
    {
        CellSize = BaseCellSize;
        gridHeight = 200;
        gridWidth = gridHeight * (Width/Height);

        offsetX = -(gridWidth * CellSize + 1) / 2;
        offsetY = -(gridHeight * CellSize + 1) / 2;
        // Initialize the grid state
        gridState = new int[gridWidth, gridHeight,5];

        for(int i = 0; i < gridWidth; i++)
        {
            for(int j = 0; j < gridHeight; j++)
            {
                gridState[i,j,1] = 128;
                gridState[i, j, 2] = 128;
                gridState[i, j, 3] = 25;
                gridState[i, j, 4] = 10;

            }
        }

        gridBitmap = new Bitmap[5];

        for (int i = 0; i < 5; i++) {
            gridBitmap[i] = new Bitmap(gridWidth * CellSize + 1, gridHeight * CellSize + 1);
        }



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
        this.MouseDown += SimulationPanel_MouseDownFoodGrowth;
        this.MouseMove += SimulationPanel_MouseMoveFoodGrowth;
        this.MouseDown += SimulationPanel_MouseDownWaterGrowth;
        this.MouseMove += SimulationPanel_MouseMoveWaterGrowth;
        this.MouseDown += SimulationPanel_MouseDownTemperatureGrowth;
        this.MouseMove += SimulationPanel_MouseMoveTemperatureGrowth;
        this.MouseDown += SimulationPanel_MouseDownObstacleGrowth;
        this.MouseMove += SimulationPanel_MouseMoveObstacleGrowth;
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
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
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
                if(gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
                
            }

            RenderChangeOnClick(cellX, cellY);

            Invalidate(new Rectangle(0, 0, Width, Height));

        }
    }



    private void SimulationPanel_MouseMoveFoodGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Food)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Food)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }

    }

    private void SimulationPanel_MouseDownFoodGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Food)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }

            }

            RenderChangeOnClick(cellX, cellY);

            Invalidate(new Rectangle(0, 0, Width, Height));

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Food)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }


    private void SimulationPanel_MouseMoveWaterGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Water)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });



        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Water)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }

    private void SimulationPanel_MouseDownWaterGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Water)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }

            }

            RenderChangeOnClick(cellX, cellY);

            Invalidate(new Rectangle(0, 0, Width, Height));

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Water)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }

    private void SimulationPanel_MouseMoveTemperatureGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Temperature)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 70)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Temperature)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > -5)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }


    }

    private void SimulationPanel_MouseDownTemperatureGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Temperature)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 70)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Temperature)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > -5)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }



    private void SimulationPanel_MouseMoveObstacleGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Accessibility)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });



        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Accessibility)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }

    private void SimulationPanel_MouseDownObstacleGrowth(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && SimulationMode == SimulationMode.Accessibility)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] < 255)
                {
                    gridState[cellX, cellY, currentGrid] += 1;
                }

            }

            RenderChangeOnClick(cellX, cellY);

            Invalidate(new Rectangle(0, 0, Width, Height));

        }

        if (e.Button == MouseButtons.Right && SimulationMode == SimulationMode.Accessibility)
        {

            int cellX = (e.X - offsetX) / CellSize;
            int cellY = (e.Y - offsetY) / CellSize;

            if (cellX >= 0 && cellX < gridWidth && cellY >= 0 && cellY < gridHeight)
            {
                if (gridState[cellX, cellY, currentGrid] > 0)
                {
                    gridState[cellX, cellY, currentGrid] -= 1;
                }
            }

            Task.Run(() => {

                RenderChangeOnClick(cellX, cellY);

                Invalidate(new Rectangle(0, 0, Width, Height));

            });

        }
    }


    private void RenderChangeOnClick(int cellX,int cellY)
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        Color cellColor;

        switch (currentGrid)
        {
            case 0:
                cellColor = ColorGenerator.GenerateFungiColor(gridState[cellX, cellY, currentGrid]);
                break;
            case 1:
                cellColor = ColorGenerator.GenerateFoodColor(gridState[cellX, cellY, currentGrid]);
                break;
            case 2:
                cellColor = ColorGenerator.GenerateWaterColor(gridState[cellX, cellY, currentGrid]);
                break;
            case 3:
                cellColor = ColorGenerator.GenerateTemperatureColor(gridState[cellX, cellY, currentGrid]);
                break;
            case 4:
                cellColor = ColorGenerator.GenerateObstacleColor(gridState[cellX, cellY, currentGrid]);
                break;
            default:
                cellColor = Color.FromArgb(30,30,30);
                break;
        }

        using Brush brush = new SolidBrush(cellColor);

        g.FillRectangle(brush, cellX*BaseCellSize+1, cellY * BaseCellSize+1, BaseCellSize-1, BaseCellSize-1);

    }

    private void RenderGridToBitmap()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        switch (currentGrid)
        {
            case 0:
                RenderGridToBitmapOrganisms();
                break;
            case 1:
                RenderGridToBitmapFood();
                break;
            case 2:
                RenderGridToBitmapWater();
                break;
            case 3:
                RenderGridToBitmapTemperature();
                break;
            case 4:
                RenderGridToBitmapAccessibility();
                break;
        }

        Invalidate(new Rectangle(0, 0, Width, Height));
    }

    private void RenderGridToBitmapOrganisms()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(ColorGenerator.GenerateFungiColor(gridState[x, y, currentGrid])))
                {
                    g.FillRectangle(brush, x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
            }
        }
    }

    private void RenderGridToBitmapFood()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(ColorGenerator.GenerateFoodColor(gridState[x, y, currentGrid])))
                {
                    g.FillRectangle(brush, x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
            }
        }
    }

    private void RenderGridToBitmapWater()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(ColorGenerator.GenerateWaterColor(gridState[x, y, currentGrid])))
                {
                    g.FillRectangle(brush, x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
            }
        }
    }

    private void RenderGridToBitmapTemperature()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(ColorGenerator.GenerateTemperatureColor(gridState[x, y, currentGrid])))
                {
                    g.FillRectangle(brush, x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
            }
        }
    }

    private void RenderGridToBitmapAccessibility()
    {
        using Graphics g = Graphics.FromImage(gridBitmap[currentGrid]);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                using (Brush brush = new SolidBrush(ColorGenerator.GenerateObstacleColor(gridState[x, y, currentGrid])))
                {
                    g.FillRectangle(brush, x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
                }

                g.DrawRectangle(new Pen(Color.FromArgb(150, 150, 150)), x * BaseCellSize, y * BaseCellSize, BaseCellSize, BaseCellSize);
            }
        }
    }






    private void SimulationGrowthPanel_Paint(object sender, PaintEventArgs e)
    {

        e.Graphics.DrawImage(gridBitmap[currentGrid], new Rectangle(offsetX, offsetY, (int)(gridBitmap[currentGrid].Width * zoomFactor), (int)(gridBitmap[currentGrid].Height * zoomFactor)));
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