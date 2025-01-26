using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FungiSimulation
{
    public class Simulation
    {

        public List<Point> currentOrganismCells = [];
        public Dictionary<Point, int> repeat = [];
        public List<Branch> branches = [];

        public void Start(SimulationPanel simulationPanel)
        {

            ShowPanel showPanel = simulationPanel.form.showPanelPanel;
            branches = simulationPanel.branches;

            Task.Run(() =>
            {
                BranchHandling(simulationPanel);
            });


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
                Thread.Sleep(700);

            }
        }

        private int CalculationOfGrowth(Point cell, SimulationPanel simulationPanel)
        {
            var gridState = simulationPanel.gridState;


            if (gridState[cell.X, cell.Y, 0] > 0 && gridState[cell.X, cell.Y, 0] < 255)
            {

                return 4;
            }
            else
            {
                return 0;
            }
        }
        private void BranchHandling(SimulationPanel simulationPanel)
        {
            while (true)
            {
                Random random = new Random();
                var gridState = simulationPanel.gridState;

                foreach (var branch in branches.FindAll(x => x._isActive))
                {
                    var eventNumber = random.Next(0, 37);

                    if (eventNumber < 17)
                    {
                        double newPlaceX = branch.technicalEndpointXCoordinates + Math.Cos(branch._currentAngle * Math.PI / 180.0);
                        double newPlaceY = branch.technicalEndpointYCoordinates - Math.Sin(branch._currentAngle * Math.PI / 180.0);

                        branch.technicalEndpointXCoordinates = newPlaceX;
                        branch.technicalEndpointYCoordinates = newPlaceY;

                        if (newPlaceX >= gridState.GetLength(0) || newPlaceY >= gridState.GetLength(1) || newPlaceX < 0.0 || newPlaceY < 0.0)
                        {
                            branches.Remove(branch); continue;
                        }

                        if (branch._endPoint.X != (int)Math.Floor(newPlaceX) || branch._endPoint.Y != (int)Math.Floor(newPlaceY))
                        {
                            branch._endPoint.X = (int)Math.Floor(newPlaceX);
                            branch._endPoint.Y = (int)Math.Floor(newPlaceY);

                            lock (currentOrganismCells)
                            {
                                currentOrganismCells.Add(new Point(branch._endPoint.X, branch._endPoint.Y));
                            }
                            try { repeat.Add(new Point(branch._endPoint.X, branch._endPoint.Y), 0); } catch { }


                            simulationPanel.RenderChangeOnClick(branch._endPoint.X, branch._endPoint.Y, 0);
                        }




                        gridState[branch._endPoint.X, branch._endPoint.Y, 0] += 1;

                        Task.Run(() =>
                        {
                            simulationPanel.RenderChangeOnClick(branch._endPoint.X, branch._endPoint.Y, 0);
                        });

                    }
                    else if (eventNumber < 24)
                    {
                        var angleChange = 0.003 * Math.Pow(random.Next(0, 11) + 3, 3) + 4;
                        int sign = random.Next(0, 2) == 0 ? -1 : 1;

                        angleChange *= sign;

                        branch._currentAngle += angleChange;

                        double newPlaceX = branch.technicalEndpointXCoordinates + Math.Cos(branch._currentAngle * Math.PI / 180.0);
                        double newPlaceY = branch.technicalEndpointYCoordinates - Math.Sin(branch._currentAngle * Math.PI / 180.0);

                        branch.technicalEndpointXCoordinates = newPlaceX;
                        branch.technicalEndpointYCoordinates = newPlaceY;

                        if (newPlaceX >= gridState.GetLength(0) || newPlaceY >= gridState.GetLength(1) || newPlaceX < 0.0 || newPlaceY < 0.0)
                        {
                            branches.Remove(branch); continue;
                        }

                        if (branch._endPoint.X != (int)Math.Floor(newPlaceX) || branch._endPoint.Y != (int)Math.Floor(newPlaceY))
                        {
                            branch._endPoint.X = (int)Math.Floor(newPlaceX);
                            branch._endPoint.Y = (int)Math.Floor(newPlaceY);

                            lock (currentOrganismCells)
                            {
                                currentOrganismCells.Add(new Point(branch._endPoint.X, branch._endPoint.Y));
                            }

                            try { repeat.Add(new Point(branch._endPoint.X, branch._endPoint.Y), 0); } catch { }

                            simulationPanel.RenderChangeOnClick(branch._endPoint.X, branch._endPoint.Y, 0);
                        }




                        gridState[branch._endPoint.X, branch._endPoint.Y, 0] += 1;

                        Task.Run(() =>
                        {
                            simulationPanel.RenderChangeOnClick(branch._endPoint.X, branch._endPoint.Y, 0);
                        });

                    }
                    else if (eventNumber == 24)
                    {
                        double angle = random.NextDouble() * 360;
                        branches.Add(new Branch(branch._endPoint, branch._endPoint, true, angle));
                    }
                    else if (eventNumber == 25)
                    {
                        //branch._isActive = false;
                    }


                }
                Thread.Sleep(100);
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
