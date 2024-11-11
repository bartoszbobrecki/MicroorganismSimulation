using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace FungiSimulation
{
    public class ShowPanel : Panel
    {
        private SimulationPanel _simulationPanel;

        // Properties to hold the values you want to display
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public int OrganismGrowth { get; set; }
        public int FoodAmount { get; set; }
        public int WaterAmount { get; set; }
        public int Temperature { get; set; }
        public int Accessibility { get; set; }

        public ShowPanel(SimulationPanel simulationPanel)
        {
            _simulationPanel = simulationPanel;
            this.DoubleBuffered = true; // Prevents flickering
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Font font = new Font("Arial", 10);
            Brush brush = new SolidBrush(Color.FromArgb(150, 150, 150));

            // Draw each line of text with the current property values
            g.DrawString($"X: {XCoordinate}   Y: {YCoordinate}", font, brush, new PointF(10, 10));
            g.DrawString($"Organism: {OrganismGrowth}", font, brush, new PointF(10, 25));
            g.DrawString($"Food: {FoodAmount}", font, brush, new PointF(10, 40));
            g.DrawString($"Water: {WaterAmount}", font, brush, new PointF(120, 10));
            g.DrawString($"Temperature: {Temperature}", font, brush, new PointF(120, 25));
            g.DrawString($"Accessibility: {Accessibility}", font, brush, new PointF(120, 40));
        }

        // Method to update the values and refresh the display
        public void UpdateDisplay(int x, int y, int organismGrowth, int food, int water, int temperature, int accessibility)
        {
            XCoordinate = x;
            YCoordinate = y;
            OrganismGrowth = organismGrowth;
            FoodAmount = food;
            WaterAmount = water;
            Temperature = temperature;
            Accessibility = accessibility;

            // Trigger a repaint to update the displayed text
            this.Invalidate();
        }

        public void UpdateOrganismGrowth(int value)
        {
            OrganismGrowth = value;
            this.Invalidate();
        }

        public void UpdateFoodAmount(int value)
        {
            FoodAmount = value;
            this.Invalidate();
        }

        public void UpdateWaterAmount(int value)
        {
            WaterAmount = value;
            this.Invalidate();
        }

        public void UpdateTemperature(int value)
        {
            Temperature = value;
            this.Invalidate();
        }

        public void UpdateAccessibility(int value)
        {
            Accessibility = value;
            this.Invalidate();
        }

    }
}
