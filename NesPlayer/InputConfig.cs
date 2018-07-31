using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SharpDX.XInput;
namespace NesPlayer
{
    public partial class InputConfig : Form
    {
        public InputConfig()
        {
            InitializeComponent();
        }

        private void InputConfig_Load(object sender, EventArgs e)
        {
            LoadMappedInputs();
            var Controller = new Controller(UserIndex.One);
            UpComboBox.Items.Add("Keyboard");            
            DownComboBox.Items.Add("Keyboard");
            LeftComboBox.Items.Add("Keyboard");
            RightComboBox.Items.Add("Keyboard");
            SelectComboBox.Items.Add("Keyboard");
            StartComboBox.Items.Add("Keyboard");
            AComboBox.Items.Add("Keyboard");
            BComboBox.Items.Add("Keyboard");
            DownComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            LeftComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            RightComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            SelectComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            StartComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            AComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            BComboBox.SelectedText = Inputs.InputDictionary["Up"].Key;
            
            if (Controller.IsConnected == true)
            {
                UpComboBox.Items.Add(Controller.GetType().Name);
                DownComboBox.Items.Add(Controller.GetType().Name);
                LeftComboBox.Items.Add(Controller.GetType().Name);
                RightComboBox.Items.Add(Controller.GetType().Name);
                SelectComboBox.Items.Add(Controller.GetType().Name);
                StartComboBox.Items.Add(Controller.GetType().Name);
                AComboBox.Items.Add(Controller.GetType().Name);
                BComboBox.Items.Add(Controller.GetType().Name);
            }
            UpComboBox.SelectedItem = Inputs.InputDictionary["Up"].Key;
            BComboBox.SelectedItem = Inputs.InputDictionary["B"].Key;
            DownComboBox.SelectedItem = Inputs.InputDictionary["Down"].Key;
            LeftComboBox.SelectedItem = Inputs.InputDictionary["Left"].Key;
            RightComboBox.SelectedItem = Inputs.InputDictionary["Right"].Key;
            SelectComboBox.SelectedItem = Inputs.InputDictionary["Select"].Key;
            StartComboBox.SelectedItem = Inputs.InputDictionary["Start"].Key;
            AComboBox.SelectedItem = Inputs.InputDictionary["A"].Key;
        }

        public void LoadMappedInputs()
        {
            UpMappedLabel.Text = Inputs.InputDictionary["Up"].Value;
            DownMappedLabel.Text = Inputs.InputDictionary["Down"].Value;
            LeftMappedLabel.Text = Inputs.InputDictionary["Left"].Value;
            RightMappedLabel.Text = Inputs.InputDictionary["Right"].Value;
            SelectMappedLabel.Text = Inputs.InputDictionary["Select"].Value;
            StartMappedLabel.Text = Inputs.InputDictionary["Start"].Value;
            AMappedLabel.Text = Inputs.InputDictionary["A"].Value;
            BMappedLabel.Text = Inputs.InputDictionary["B"].Value;
        }
        private void UpConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Up";
            Inputs.InputDictionary["Up"]= new KeyValuePair<string, string>(UpComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = UpComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Up";
                }
            }
        }

        private void DownConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Down";
            Inputs.InputDictionary["Down"] = new KeyValuePair<string, string>(DownComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = DownComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Down";
                }
            }
        }

        private void LeftConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Left";
            Inputs.InputDictionary["Left"] = new KeyValuePair<string, string>(LeftComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = LeftComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Left";
                }
            }
        }

        private void RightConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Right";
            Inputs.InputDictionary["Right"] = new KeyValuePair<string, string>(RightComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = RightComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Right";
                }
            }
        }

        private void SelectConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Select";
            Inputs.InputDictionary["Select"] = new KeyValuePair<string, string>(SelectComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = SelectComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Select";
                }
            }
        }

        private void StartConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "Start";
            Inputs.InputDictionary["Start"] = new KeyValuePair<string, string>(StartComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = StartComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "Start";
                }
            }
        }

        private void AConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "A";
            Inputs.InputDictionary["A"] = new KeyValuePair<string, string>(AComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = AComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "A";
                }
            }
        }

        private void BConfigureButton_Click(object sender, EventArgs e)
        {
            InputPopup.CurrentInput = "B";
            Inputs.InputDictionary["B"] = new KeyValuePair<string, string>(BComboBox.SelectedText, "Not Mapped");
            InputPopup.InputMode = BComboBox.SelectedItem.ToString();
            using (InputPopup InputPopUpForm = new InputPopup())
            {
                if (InputPopUpForm.ShowDialog() == DialogResult.OK)
                {
                    InputPopup.CurrentInput = "B";
                }
            }
        }
    }
}
