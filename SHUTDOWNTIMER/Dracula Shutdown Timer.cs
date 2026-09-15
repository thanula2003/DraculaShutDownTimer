using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SHUTDOWNTIMER
{
    public partial class Dracula_Shutdown_Timer : Form
    {
        private DateTime targetTime;
        private string selectedAction = "";
        private bool timerRunning = false;

        public Dracula_Shutdown_Timer()
        {
            InitializeComponent();

            // Transparent labels
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            label3.BackColor = Color.Transparent;
            label4.BackColor = Color.Transparent;
            label5.BackColor = Color.Transparent;
            label6.BackColor = Color.Transparent;
            label7.BackColor = Color.Transparent;
            label8.BackColor = Color.Transparent;
            label9.BackColor = Color.Transparent;
            label10.BackColor = Color.Transparent;

            // Button appearance
            SetupButton(button1);
            SetupButton(button2);
            SetupButton(button3);
            SetupButton(button4);
            SetupButton(button5);

            // IMPORTANT:
            // Connect buttons directly to their click events
            button1.Click += button1_Click;
            button2.Click += button2_Click;
            button3.Click += button3_Click;
            button4.Click += button4_Click;
            button5.Click += button5_Click;
            button6.Click += button6_Click;

            // Timer
            timer1.Interval = 1000;
            timer1.Tick += timer1_Tick;

            // AM / PM
            comboBox1.Items.Clear();
            comboBox1.Items.Add("AM");
            comboBox1.Items.Add("PM");
            comboBox1.SelectedIndex = 0;

            label10.Text = "No action scheduled.";
        }

        private void SetupButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Black;
            button.FlatAppearance.MouseDownBackColor = Color.Black;
        }

        // =========================
        // BUTTONS
        // =========================

        private void button1_Click(object? sender, EventArgs e)
        {
            StartAction("shutdown");
        }

        private void button2_Click(object? sender, EventArgs e)
        {
            StartAction("restart");
        }

        private void button3_Click(object? sender, EventArgs e)
        {
            StartAction("sleep");
        }

        private void button4_Click(object? sender, EventArgs e)
        {
            StartAction("lock");
        }

        private void button5_Click(object? sender, EventArgs e)
        {
            AbortAction();
        }

        private void button6_Click(object? sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();

            comboBox1.SelectedIndex = 0;

            label10.Text = "No action scheduled.";
        }

        // =========================
        // START ACTION
        // =========================

        private void StartAction(string action)
        {
            bool afterTimeEntered = IsAfterTimeEntered();
            bool exactTimeEntered = IsExactTimeEntered();

            // Both methods entered
            if (afterTimeEntered && exactTimeEntered)
            {
                MessageBox.Show(
                    "Please use only one method.\n\n" +
                    "Enter either an 'After a Certain Time' value " +
                    "OR an 'Exact Scheduled Time'.",
                    "Two Methods Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Nothing entered
            if (!afterTimeEntered && !exactTimeEntered)
            {
                MessageBox.Show(
                    "Please enter a time before selecting an action.",
                    "Time Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            selectedAction = action;

            if (afterTimeEntered)
            {
                StartAfterTime();
            }
            else
            {
                StartExactTime();
            }
        }

        // =========================
        // CHECK AFTER-TIME INPUT
        // =========================

        private bool IsAfterTimeEntered()
        {
            return IsRealInput(textBox1.Text) ||
                   IsRealInput(textBox2.Text) ||
                   IsRealInput(textBox3.Text);
        }

        // =========================
        // CHECK EXACT-TIME INPUT
        // =========================

        private bool IsExactTimeEntered()
        {
            bool hourEntered = IsRealInput(textBox4.Text);
            bool minuteEntered = IsRealInput(textBox5.Text);

            // 00 is the default seconds value,
            // so don't count it as user input.
            bool secondEntered =
                IsRealInput(textBox6.Text) &&
                textBox6.Text.Trim() != "00";

            return hourEntered ||
                   minuteEntered ||
                   secondEntered;
        }

        private bool IsRealInput(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            value = value.Trim();

            if (value == "0" || value == "00")
                return false;

            return true;
        }

        // =========================
        // AFTER CERTAIN TIME
        // =========================

        private void StartAfterTime()
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            // Hours
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                if (!int.TryParse(textBox1.Text.Trim(), out hours))
                {
                    MessageBox.Show(
                        "Invalid hours.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            // Minutes
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                if (!int.TryParse(textBox2.Text.Trim(), out minutes))
                {
                    MessageBox.Show(
                        "Invalid minutes.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            // Seconds
            if (!string.IsNullOrWhiteSpace(textBox3.Text))
            {
                if (!int.TryParse(textBox3.Text.Trim(), out seconds))
                {
                    MessageBox.Show(
                        "Invalid seconds.",
                        "Invalid Input",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            // Validation
            if (hours < 0)
            {
                MessageBox.Show("Hours cannot be negative.");
                return;
            }

            if (minutes < 0 || minutes > 59)
            {
                MessageBox.Show("Minutes must be between 0 and 59.");
                return;
            }

            if (seconds < 0 || seconds > 59)
            {
                MessageBox.Show("Seconds must be between 0 and 59.");
                return;
            }

            int totalSeconds =
                (hours * 3600) +
                (minutes * 60) +
                seconds;

            if (totalSeconds <= 0)
            {
                MessageBox.Show(
                    "Please enter a time greater than 0.",
                    "Invalid Time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Calculate target
            targetTime = DateTime.Now.AddSeconds(totalSeconds);

            timerRunning = true;

            timer1.Start();

            UpdateLabel();
        }

        // =========================
        // EXACT TIME
        // =========================

        private void StartExactTime()
        {
            int hour;
            int minute;
            int second = 0;

            // Hour
            if (!int.TryParse(textBox4.Text.Trim(), out hour))
            {
                MessageBox.Show(
                    "Please enter a valid hour.",
                    "Invalid Time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Minute
            if (!int.TryParse(textBox5.Text.Trim(), out minute))
            {
                MessageBox.Show(
                    "Please enter valid minutes.",
                    "Invalid Time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Seconds
            if (!string.IsNullOrWhiteSpace(textBox6.Text))
            {
                if (!int.TryParse(textBox6.Text.Trim(), out second))
                {
                    MessageBox.Show(
                        "Please enter valid seconds.",
                        "Invalid Time",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

            // Validate hour
            if (hour < 1 || hour > 12)
            {
                MessageBox.Show(
                    "Hour must be between 1 and 12.",
                    "Invalid Hour",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Validate minute
            if (minute < 0 || minute > 59)
            {
                MessageBox.Show(
                    "Minutes must be between 0 and 59.",
                    "Invalid Minutes",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Validate seconds
            if (second < 0 || second > 59)
            {
                MessageBox.Show(
                    "Seconds must be between 0 and 59.",
                    "Invalid Seconds",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // AM / PM
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select AM or PM.",
                    "Time Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string amPm = comboBox1.SelectedItem.ToString()!;

            // Convert 12-hour time to 24-hour time
            int hour24 = hour;

            if (amPm == "AM" && hour == 12)
            {
                hour24 = 0;
            }

            if (amPm == "PM" && hour != 12)
            {
                hour24 += 12;
            }

            DateTime now = DateTime.Now;

            DateTime scheduledTime = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                hour24,
                minute,
                second);

            // If time already passed today,
            // schedule it for tomorrow.
            if (scheduledTime <= now)
            {
                scheduledTime = scheduledTime.AddDays(1);
            }

            targetTime = scheduledTime;

            timerRunning = true;

            timer1.Start();

            UpdateLabel();
        }

        // =========================
        // TIMER
        // =========================

        private void timer1_Tick(object? sender, EventArgs e)
        {
            if (!timerRunning)
                return;

            TimeSpan remaining = targetTime - DateTime.Now;

            if (remaining.TotalSeconds <= 0)
            {
                timer1.Stop();

                timerRunning = false;

                ExecuteAction();

                return;
            }

            UpdateLabel();
        }

        // =========================
        // UPDATE LABEL
        // =========================

        private void UpdateLabel()
        {
            TimeSpan remaining = targetTime - DateTime.Now;

            if (remaining.TotalSeconds < 0)
            {
                remaining = TimeSpan.Zero;
            }

            int hours = (int)remaining.TotalHours;
            int minutes = remaining.Minutes;
            int seconds = remaining.Seconds;

            string actionText = GetActionText();

            string exactTime =
                targetTime.ToString("hh:mm:ss tt");

            label10.Text =
                $"PC will {actionText} after " +
                $"{hours:D2} hours " +
                $"{minutes:D2} minutes and " +
                $"{seconds:D2} seconds " +
                $"at {exactTime}";
        }

        // =========================
        // ACTION TEXT
        // =========================

        private string GetActionText()
        {
            switch (selectedAction)
            {
                case "shutdown":
                    return "shutdown";

                case "restart":
                    return "restart";

                case "sleep":
                    return "sleep";

                case "lock":
                    return "lock";

                default:
                    return "perform the selected action";
            }
        }

        // =========================
        // EXECUTE ACTION
        // =========================

        private void ExecuteAction()
        {
            try
            {
                switch (selectedAction)
                {
                    case "shutdown":

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "shutdown.exe",
                            Arguments = "/s /t 0",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        });

                        break;

                    case "restart":

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "shutdown.exe",
                            Arguments = "/r /t 0",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        });

                        break;

                    case "sleep":

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "rundll32.exe",
                            Arguments = "powrprof.dll,SetSuspendState 0,1,0",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        });

                        break;

                    case "lock":

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "rundll32.exe",
                            Arguments = "user32.dll,LockWorkStation",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        });

                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not perform the action.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // =========================
        // ABORT
        // =========================

        private void AbortAction()
        {
            if (!timerRunning)
            {
                label10.Text =
                    "No action is currently scheduled.";

                return;
            }

            timer1.Stop();

            timerRunning = false;

            selectedAction = "";

            label10.Text =
                "Scheduled action aborted.";
        }

        // =========================
        // FORM LOAD
        // =========================

        private void Dracula_Shutdown_Timer_Load(
            object? sender,
            EventArgs e)
        {
        }
    }
}