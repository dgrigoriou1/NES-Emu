using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NesPlayer
{
    public partial class InputPopup : Form
    {
        public static string CurrentInput;
        public static string InputMode = "Keyboard";
        public InputPopup()
        {
            InitializeComponent();
        }
        public static Controller controller = new Controller(UserIndex.One);
        private void InputPopup_KeyDown(object sender, KeyEventArgs e)
        {
            if (InputMode == "Keyboard")
            {
                timer1.Stop();
                Inputs.InputDictionary[InputLabel.Text.ToString()] = new KeyValuePair<string, string>("Keyboard", e.KeyCode.ToString());
                InputConfig InputConfigureForm = (InputConfig)Application.OpenForms["InputConfig"];
                //int InputIndex = 0;
                //switch (InputLabel.Text.ToString())
                //{
                //    case "B":
                //        InputIndex = 0;
                //        break;
                //    case "A":
                //        InputIndex = 1;
                //        break;
                //    case "Select":
                //        InputIndex = 2;
                //        break;
                //    case "Start":
                //        InputIndex = 3;
                //        break;
                //    case "Up":
                //        InputIndex = 4;
                //        break;
                //    case "Down":
                //        InputIndex = 5;
                //        break;
                //    case "Left":
                //        InputIndex = 6;
                //        break;
                //    case "Right":
                //        InputIndex = ;
                //        break;
                //}
                if (Inputs.InputKeyBoardDictionary.ContainsKey(e.KeyCode.ToString()))
                {
                    Inputs.InputKeyBoardDictionary[e.KeyCode.ToString()] = InputLabel.Text.ToString();
                }
                else
                {
                    Inputs.InputKeyBoardDictionary.Add(e.KeyCode.ToString(), InputLabel.Text.ToString());
                }
                InputConfigureForm.LoadMappedInputs();
                InputConfigureForm.Show();
                this.Close();
            }       
        }

        private void InputPopup_Load(object sender, EventArgs e)
        {
            InputLabel.Text = CurrentInput;
            timer1.Start();
        }

        private void InputPopup_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Dispose();
            timer1 = null;
        }

        private void InputPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
         }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (controller.IsConnected == true)
            {
                var CurrentGamepadFlags = controller.GetState();
                GamepadButtonFlags Gamepadflags = new GamepadButtonFlags();
                if (CurrentGamepadFlags.Gamepad.Buttons != Gamepadflags)
                {
                    string ButtonPressed = CurrentGamepadFlags.Gamepad.Buttons.ToString().Split(',')[0];
                    timer1.Stop();
                    Inputs.InputDictionary[InputLabel.Text.ToString()] = new KeyValuePair<string, string>(controller.GetType().Name, ButtonPressed);
                 
                    Configuration ControllerConfiguration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
                    ControllerConfiguration.AppSettings.Settings.Remove(InputLabel.Text.ToString());
                    ControllerConfiguration.AppSettings.Settings.Add(InputLabel.Text.ToString(), controller.GetType().Name + "," + ButtonPressed);
                    ControllerConfiguration.Save(ConfigurationSaveMode.Minimal);
                    
                    InputConfig InputConfigureForm = (InputConfig)Application.OpenForms["InputConfig"];
                    InputConfigureForm.LoadMappedInputs();
                    InputConfigureForm.Show();
                    this.Close();
                }
            }
        }
    }
}
