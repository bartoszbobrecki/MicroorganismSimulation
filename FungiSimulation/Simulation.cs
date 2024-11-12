using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FungiSimulation
{
    public class Simulation
    {

        public List<Point> currentOrganismCells = [];
        public Dictionary<Point, int> repeat = [];

        public void Start(SimulationPanel simulationPanel)
        {

            ShowPanel showPanel = simulationPanel.form.showPanelPanel;

            while (true)
            {
                Task.Run(() => {

                    List<Point> copyCells;

                    lock (currentOrganismCells)
                    {
                        copyCells = currentOrganismCells.ToList();
                    }

                    foreach (Point cell in copyCells)
                    {
                        try
                        {
                            var copy = repeat.ToDictionary();

                            if (simulationPanel.gridState[cell.X, cell.Y, 0] == 0 || copy[cell] > 37)
                            {
                                lock (currentOrganismCells)
                                {
                                    currentOrganismCells.Remove(cell);
                                }
                                lock (repeat)
                                {
                                    repeat.Remove(cell);
                                }

                                continue;
                            }
                        }
                        catch { }

                        if(CheckToSkip(cell, simulationPanel))
                        {
                            continue;
                        }
                        try
                        {
                            if (simulationPanel.gridState[cell.X, cell.Y, 0] == 255)
                            {
                                repeat[cell] += 1;
                            }
                        }
                        catch { }

                        simulationPanel.gridState[cell.X, cell.Y, 0] += CalculationOfGrowth(cell, simulationPanel);
                        Task.Run(() =>
                        {
                            simulationPanel.RenderChangeOnClick(cell.X, cell.Y, 0);
                        });
                        
                    }
                });

                simulationPanel.Invalidate(new Rectangle(0, 0, simulationPanel.Width, simulationPanel.Height));
                Thread.Sleep(5);

            }
        }

        private int CalculationOfGrowth(Point cell, SimulationPanel simulationPanel)
        {
            var gridState = simulationPanel.gridState;
            Random random = new();
            int[] step = { -1, 0, 1 };


            if (gridState[cell.X, cell.Y, 0] > 0 && gridState[cell.X, cell.Y, 0] < 255)
            {
                if (gridState[cell.X, cell.Y, 0] > 128)
                {
                    int randomStepX = step[random.Next(step.Length)];
                    int randomStepY = step[random.Next(step.Length)];

                    if(gridState[cell.X + randomStepX, cell.Y + randomStepY, 0] == 0 && random.Next(37) == 7)
                    {
                        gridState[cell.X + randomStepX, cell.Y + randomStepY, 0] += 1;
                        lock (currentOrganismCells)
                        {
                            currentOrganismCells.Add(new Point(cell.X + randomStepX, cell.Y + randomStepY));
                        }
                        
                        repeat.Add(new Point(cell.X + randomStepX, cell.Y + randomStepY), 0);
                        
                        simulationPanel.RenderChangeOnClick(cell.X + randomStepX, cell.Y + randomStepY, 0);
                    }

                }

                return 1;
            }
            else
            {
                return 0;
            }
        }

        private bool CheckToSkip(Point cell, SimulationPanel simulationPanel)
        {
            var gridState = simulationPanel.gridState;
            return gridState[cell.X, cell.Y, 0] == 255 &&
                gridState[cell.X-1, cell.Y-1, 0] == 255 &&
                gridState[cell.X, cell.Y-1, 0] == 255 &&
                gridState[cell.X+1, cell.Y-1, 0] == 255 &&
                gridState[cell.X-1, cell.Y+1, 0] == 255 &&
                gridState[cell.X, cell.Y+1, 0] == 255 &&
                gridState[cell.X+1, cell.Y+1, 0] == 255 &&
                gridState[cell.X-1, cell.Y, 0] == 255 &&
                gridState[cell.X+1, cell.Y, 0] == 255;
        }
        

    }
}
