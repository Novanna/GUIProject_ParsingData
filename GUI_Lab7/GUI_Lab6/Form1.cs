using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GUI_Lab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //baudrate
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("14400");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("56000");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("76800");
            comboBox2.Items.Add("115200");

            //chart
            chart1.Series[0].Name = "Temp";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[0].Color = Color.Red;
            chart1.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";

            chart2.Series[0].Name = "Hum";
            chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart2.Series[0].BorderWidth = 2;
            chart2.Series[0].Color = Color.Aqua;
            chart2.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";

            chart3.Series[0].Name = "Press";
            chart3.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart3.Series[0].BorderWidth = 2;
            chart3.Series[0].Color = Color.Purple;
            chart3.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";

            chart4.Series[0].Name = "Uv";
            chart4.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart4.Series[0].BorderWidth = 2;
            chart4.Series[0].Color = Color.Orange;
            chart4.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            chart4.ChartAreas[0].AxisX.LabelStyle.Format = "0.00";

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String[] portList = System.IO.Ports.SerialPort.GetPortNames();
            foreach (String portName in portList)
                comboBox1.Items.Add(portName);
            comboBox1.Text = comboBox1.Items[comboBox1.Items.Count - 1].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Connect";
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                toolStripStatusLabel1.Text = serialPort1.PortName + " is closed.";
                button1.Text = "Connect";
                
            }
            else
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                serialPort1.NewLine = "\r\n";
                serialPort1.Open();
                toolStripStatusLabel1.Text = serialPort1.PortName + " is connected.";
                button1.Text = "Disconnect";
            }  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            String receivedMsg = serialPort1.ReadLine();
            Tampilkan(receivedMsg);
        }

        private delegate void TampilkanDelegate(object item);
        private void Tampilkan(object item)
        {
            if (InvokeRequired)
                listBox1.Invoke(new TampilkanDelegate(Tampilkan), item);
            else
            {
                // This is the UI thread so perform the task.
                listBox1.Items.Add(item);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                splitData(item);
            }
        }

        private void splitData(object item)
        {
            if (timer1.Enabled == true)
            {
                String[] data = item.ToString().Split(',');
                /*label13.Text = data[1]; // textbox untuk data suhu
                label14.Text = data[2]; // textbox untuk data kelembaban
                label15.Text = data[3]; // textbox untuk data tekanan udara
                label16.Text = data[4]; // textbox untuk data uv index */
                time += 0.001; 

                string temp = data[1];
                label13.Text = temp + "°C";
                double Temp = Convert.ToDouble(temp);
                chart1.Series[0].Points.AddXY(time, Temp);
                
                string hum = data[2];
                label14.Text = hum + "%RH"; 
                double Hum = Convert.ToDouble(hum);
                chart2.Series[0].Points.AddXY(time, Hum);

                string pres = data[3];
                label15.Text = pres + "mb";
                double Pres = Convert.ToDouble(pres);
                chart3.Series[0].Points.AddXY(time, Pres);

                string uv = data[4];
                label16.Text = uv;
                double Uv = Convert.ToDouble(uv);
                chart4.Series[0].Points.AddXY(time, Uv);
                
            }
            else
                toolStripStatusLabel1.Text = "Start your application!";
        }
        private double time = 0.0;

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                timer1.Enabled = !(timer1.Enabled);
                if (timer1.Enabled == true)
                    button3.Text = "Stop";
                else
                    button3.Text = "Start";
            }
            else
            {
                toolStripStatusLabel1.Text =  "Connect your serial port first!";
            }

        }

        private void chartControl1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            label13.ResetText();
            label14.ResetText();
            label15.ResetText();
            label16.ResetText();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Are you sure to close the app?", "Close Application", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.No) e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

   }
}

