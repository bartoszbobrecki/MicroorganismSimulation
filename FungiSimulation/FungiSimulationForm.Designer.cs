
namespace FungiSimulation
{
    partial class FungiSimulationForm
    {

        private GroupBox tools;
        private DataGridView dataGrid;
        private Panel dataGridGroupBox;
        private Panel innerDataGridGroupBox;
        private Button cursorButton;
        private Button fungiButton;
        private Button foodButton;
        private Button waterButton;
        private Button temperatureButton;
        private Button accessibilityButton;
        private List<Button> buttons;
        private Panel showPanel;
        private Panel innerShowPanel;
        public ShowPanel showPanelPanel;

        SimulationPanel simulationPanel;
        public Simulation Simulation;

        private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {


            buttons = new List<Button>();
            tools = new GroupBox();
            dataGridGroupBox = new Panel();
            innerDataGridGroupBox = new Panel();
            cursorButton = new Button();
            fungiButton = new Button();
            foodButton = new Button();
            waterButton = new Button();
            temperatureButton = new Button();
            accessibilityButton = new Button();
            buttons.Add(cursorButton);
            buttons.Add(fungiButton);
            buttons.Add(foodButton);
            buttons.Add(waterButton);
            buttons.Add(temperatureButton);
            buttons.Add(accessibilityButton);

            showPanel = new Panel();
            innerShowPanel = new Panel();

            simulationPanel = new SimulationPanel(this)
            {
                Dock = DockStyle.Top | DockStyle.Fill,
            };

            showPanelPanel = new ShowPanel(simulationPanel)
            {
                Dock = DockStyle.Top | DockStyle.Fill,
            };

            //((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            SuspendLayout();
            // 
            // tools
            // 
            tools.Anchor = AnchorStyles.Top;
            tools.AutoSize = true;
            tools.BackColor = Color.FromArgb(30, 30, 30);
            tools.ForeColor = Color.FromArgb(220, 220, 220);
            tools.Location = new Point(12, 0);
            tools.Name = "tools";
            tools.Size = new Size(705, 0);
            tools.TabIndex = 0;
            tools.TabStop = false;
            tools.Text = "Tools";
            tools.Controls.Add(cursorButton);
            tools.Controls.Add(fungiButton);
            tools.Controls.Add(foodButton);
            tools.Controls.Add(waterButton);
            tools.Controls.Add(temperatureButton);
            tools.Controls.Add(accessibilityButton);
            tools.Controls.Add(showPanel);


            showPanel.Location = new Point(350, 20);
            showPanel.Name = "ShowPanel";
            showPanel.Size = new Size(343, 70);
            showPanel.TabIndex = 1;
            showPanel.TabStop = false;
            showPanel.Text = "";
            showPanel.Controls.Add(innerShowPanel);
            showPanel.BackColor = Color.FromArgb(220, 220, 220);
            showPanel.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            innerShowPanel.Location = new Point(1, 1);
            innerShowPanel.Name = "innerShowPanel";
            innerShowPanel.Size = new Size(showPanel.Width-2, showPanel.Height - 2);
            innerShowPanel.TabIndex = 1;
            innerShowPanel.TabStop = false;
            innerShowPanel.Text = "";
            innerShowPanel.BackColor = Color.FromArgb(30, 30, 30);
            innerShowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            innerShowPanel.Controls.Add(showPanelPanel);


            //
            // buttons
            //
            cursorButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            cursorButton.BackColor = Color.FromArgb(70, 70, 70);
            cursorButton.Name = "cursorButton";
            cursorButton.ForeColor = Color.FromArgb(220, 220, 220);
            cursorButton.Location = new Point(10, 30);
            cursorButton.Size = new Size(50, 50);
            cursorButton.FlatStyle = FlatStyle.Flat;
            cursorButton.Click += CursorButton_Click;

            for (int i = 1; i < buttons.Count; i++)
            {
                var button = buttons[i];
                button.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                button.BackColor = Color.FromArgb(30, 30, 30);
                button.ForeColor = Color.FromArgb(220, 220, 220);
                button.Location = new Point(buttons[i - 1].Location.X + buttons[i - 1].Width + 3, buttons[i - 1].Location.Y);
                button.Size = new Size(buttons[i - 1].Size.Width, buttons[i - 1].Height);
                button.FlatStyle = FlatStyle.Flat;
            }

            fungiButton.Name = "fungiButton";
            fungiButton.Click += FungiButton_Click;

            foodButton.Name = "foodButton";
            foodButton.Click += FoodButton_Click;

            waterButton.Name = "waterButton";
            waterButton.Click += WaterButton_Click;

            temperatureButton.Name = "temperatureButton";
            temperatureButton.Click += TemperatureButton_Click;

            accessibilityButton.Name = "accessibilityButton";
            accessibilityButton.Click += AccessibilityButton_Click;


            Bitmap cursorBitmap = new Bitmap(64, 64);

            using (Graphics g = Graphics.FromImage(cursorBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\cursor.png"), new Rectangle(30, 30, 20, 20));
            }

            this.Cursor = new Cursor(cursorBitmap.GetHicon());





            Bitmap cursorBitmapButton = new Bitmap(64, 64);

            using (Graphics g = Graphics.FromImage(cursorBitmapButton))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\cursor.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 70 / 2, 20, 20));

                cursorButton.BackgroundImage = cursorBitmapButton;
            }

            Bitmap fungiBitmap = new Bitmap(50, 50);

            using (Graphics g = Graphics.FromImage(fungiBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\fungi.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 70 / 2, 20, 20));

                fungiButton.BackgroundImage = fungiBitmap;
            }

            Bitmap foodBitmap = new Bitmap(50, 50);

            using (Graphics g = Graphics.FromImage(foodBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\food.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 70 / 2, 20, 20));

                foodButton.BackgroundImage = foodBitmap;
            }

            Bitmap waterBitmap = new Bitmap(50, 50);

            using (Graphics g = Graphics.FromImage(waterBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\water.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 70 / 2, 20, 20));

                waterButton.BackgroundImage = waterBitmap;
            }

            Bitmap temperatureBitmap = new Bitmap(50, 50);

            using (Graphics g = Graphics.FromImage(temperatureBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\temperature.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 73 / 2, 20, 20));

                temperatureButton.BackgroundImage = temperatureBitmap;
            }

            Bitmap accessibilityBitmap = new Bitmap(50, 50);

            using (Graphics g = Graphics.FromImage(accessibilityBitmap))
            {
                g.Clear(Color.Transparent);

                g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\mountain.png"), new Rectangle(cursorButton.Width - 73 / 2, cursorButton.Height - 70 / 2, 20, 20));

                accessibilityButton.BackgroundImage = accessibilityBitmap;
            }




            // 
            // dataGridGroupBox
            // 
            dataGridGroupBox.Location = new Point(12, 106);
            dataGridGroupBox.Name = "DataGridGroupBox";
            dataGridGroupBox.Size = new Size(705, 348);
            dataGridGroupBox.TabIndex = 1;
            dataGridGroupBox.TabStop = false;
            dataGridGroupBox.Text = "";
            dataGridGroupBox.BackColor = Color.FromArgb(220, 220, 220);
            dataGridGroupBox.Controls.Add(innerDataGridGroupBox);
            dataGridGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            //
            // innerDataGridGroupBox
            //
            innerDataGridGroupBox.Location = new Point(1, 1);
            innerDataGridGroupBox.Name = "DataGridGroupBox";
            innerDataGridGroupBox.Size = new Size(dataGridGroupBox.Width - 2, dataGridGroupBox.Height - 2);
            innerDataGridGroupBox.TabIndex = 1;
            innerDataGridGroupBox.TabStop = false;
            innerDataGridGroupBox.Text = "";
            innerDataGridGroupBox.BackColor = Color.FromArgb(30, 30, 30);
            innerDataGridGroupBox.Controls.Add(simulationPanel);
            innerDataGridGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            // 
            // FungiSimulation
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(729, 466);
            Controls.Add(dataGridGroupBox);
            Controls.Add(tools);
            ForeColor = Color.FromArgb(220, 220, 220);
            Name = "FungiSimulation";
            Text = "FungiSimulation";
            Load += Fungi_Load;
            //((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();

            Simulation = new Simulation();

            Task.Run(() => {
                Simulation.Start(simulationPanel);
            });
        }

        private void AccessibilityButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Accessibility;
        }

        private void TemperatureButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Temperature;
        }

        private void WaterButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Water;
        }

        private void FoodButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Food;
        }

        private void CursorButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Cursor;
        }

        private void FungiButton_Click(object sender, EventArgs e)
        {
            simulationPanel.SimulationMode = SimulationMode.Organisms;
        }

        public void ChangeButtonHighlights(SimulationMode simulationMode, SimulationMode value)
        {
            buttons[((int)simulationMode)].BackColor = Color.FromArgb(30, 30, 30);
            buttons[((int)value)].BackColor = Color.FromArgb(70, 70, 70);

            switch (value)
            {
                case SimulationMode.Organisms:

                    Bitmap fungiBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(fungiBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\fungi.png"), new Rectangle(23, 27, 20, 20));
                    }

                    this.Cursor = new Cursor(fungiBitmap.GetHicon());

                    break;

                case SimulationMode.Cursor:

                    Bitmap cursorBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(cursorBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\cursor.png"), new Rectangle(30, 30, 20, 20));
                    }

                    this.Cursor = new Cursor(cursorBitmap.GetHicon());

                    break;

                case SimulationMode.Food:

                    Bitmap foodBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(foodBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\food.png"), new Rectangle(23, 23, 20, 20));
                    }

                    this.Cursor = new Cursor(foodBitmap.GetHicon());

                    break;

                case SimulationMode.Water:

                    Bitmap waterBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(waterBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\water.png"), new Rectangle(23, 23, 20, 20));
                    }

                    this.Cursor = new Cursor(waterBitmap.GetHicon());

                    break;

                case SimulationMode.Temperature:

                    Bitmap temperatureBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(temperatureBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\temperature.png"), new Rectangle(23, 20, 20, 20));
                    }

                    this.Cursor = new Cursor(temperatureBitmap.GetHicon());

                    break;

                case SimulationMode.Accessibility:

                    Bitmap accessibilityBitmap = new Bitmap(64, 64);

                    using (Graphics g = Graphics.FromImage(accessibilityBitmap))
                    {
                        g.Clear(Color.Transparent);

                        g.DrawImage(Bitmap.FromFile(".\\..\\..\\..\\mountain.png"), new Rectangle(23, 23, 20, 20));
                    }

                    this.Cursor = new Cursor(accessibilityBitmap.GetHicon());

                    break;
            }
        }
    }
}