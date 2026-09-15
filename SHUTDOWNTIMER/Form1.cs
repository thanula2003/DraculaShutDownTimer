namespace SHUTDOWNTIMER
{
    public partial class Form1 : Form
    {
        int counter = 0;

        public Form1()
        {
            InitializeComponent();

            label1.BackColor = Color.Transparent;

            timer1.Interval = 20;
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter +=1;

            label1.Text = counter + "%";

            if (counter >= 100)
            {
                timer1.Stop();

                Dracula_Shutdown_Timer form2 = new Dracula_Shutdown_Timer();
                form2.Show();

                this.Hide();
            }
        }
    }
}