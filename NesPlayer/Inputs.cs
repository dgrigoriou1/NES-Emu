using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SharpDX.XInput;
namespace NesPlayer
{
    class Inputs
    {
        private static int InputCounter = 0;
        private static bool InputTrigger = false;
        public static int Input4017 = 0;
        public static int Input4016;
        public static int[] InputArray = new int[8];
        public static GamepadButtonFlags GamepadFlags = new GamepadButtonFlags();        
        public static Dictionary<string, int> NewInputDictionary = new Dictionary<string, int>();
        public static Dictionary<string, string> InputKeyBoardDictionary = new Dictionary<string, string>();
        public static Dictionary<string, KeyValuePair<string, string>> InputDictionary = new Dictionary<string, KeyValuePair<string, string>>();       
        public static Controller controller = new Controller(UserIndex.One);

        public static void LoadInputs()
        {
            Inputs.InputDictionary["Up"] = new KeyValuePair<string, string>("Keyboard", "W");
            Inputs.InputDictionary["Down"] = new KeyValuePair<string, string>("Keyboard", "S");
            Inputs.InputDictionary["Left"] = new KeyValuePair<string, string>("Keyboard", "A");
            Inputs.InputDictionary["Right"] = new KeyValuePair<string, string>("Keyboard", "D");
            Inputs.InputDictionary["Select"] = new KeyValuePair<string, string>("Keyboard", "U");
            Inputs.InputDictionary["Start"] = new KeyValuePair<string, string>("Keyboard", "I");
            Inputs.InputDictionary["A"] = new KeyValuePair<string, string>("Keyboard", "N");
            Inputs.InputDictionary["B"] = new KeyValuePair<string, string>("Keyboard", "M");
            InputKeyBoardDictionary.Add("W", "Up");
            InputKeyBoardDictionary.Add("S", "Down");
            InputKeyBoardDictionary.Add("A", "Left");
            InputKeyBoardDictionary.Add("D", "Right");
            InputKeyBoardDictionary.Add("U", "Select");
            InputKeyBoardDictionary.Add("I", "Start");
            InputKeyBoardDictionary.Add("N", "A");
            InputKeyBoardDictionary.Add("M", "B");
            if (Inputs.NewInputDictionary.Count < 1)
            {
                Inputs.NewInputDictionary.Add("Up", 0);
                Inputs.NewInputDictionary.Add("Down", 0);
                Inputs.NewInputDictionary.Add("Left", 0);
                Inputs.NewInputDictionary.Add("Right", 0);
                Inputs.NewInputDictionary.Add("Select", 0);
                Inputs.NewInputDictionary.Add("Start", 0);
                Inputs.NewInputDictionary.Add("A", 0);
                Inputs.NewInputDictionary.Add("B", 0);
            }
            //InputKeyBoardDictionary = new Dictionary<string, string>();
            if (ConfigurationManager.AppSettings["Up"] != null)
            {
                string UpConfiguration = ConfigurationManager.AppSettings["Up"];
                string UpGamepadType = UpConfiguration.Split(',')[0];
                string UpValue = UpConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == UpGamepadType) || UpGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Up"] = new KeyValuePair<string, string>(UpGamepadType, UpValue);
                    if(UpGamepadType=="Keyboard" && UpValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(UpValue, "Up");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["Down"] != null)
            {
                string DownConfiguration = ConfigurationManager.AppSettings["Down"];
                string DownGamepadType = DownConfiguration.Split(',')[0];
                string DownValue = DownConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == DownGamepadType) || DownGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Down"] = new KeyValuePair<string, string>(DownGamepadType, DownValue);
                    if (DownGamepadType == "Keyboard" && DownValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(DownValue, "Down");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["Left"] != null)
            {
                string LeftConfiguration = ConfigurationManager.AppSettings["Left"];
                string LeftGamepadType = LeftConfiguration.Split(',')[0];
                string LeftValue = LeftConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == LeftGamepadType) || LeftGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Left"] = new KeyValuePair<string, string>(LeftGamepadType, LeftValue);
                    if (LeftGamepadType == "Keyboard" && LeftValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(LeftValue, "Left");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["Right"] != null)
            {
                string RightConfiguration = ConfigurationManager.AppSettings["Right"];
                string RightGamepadType = RightConfiguration.Split(',')[0];
                string RightValue = RightConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == RightGamepadType) || RightGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Right"] = new KeyValuePair<string, string>(RightGamepadType, RightValue);
                    if (RightGamepadType == "Keyboard" && RightValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(RightValue, "Right");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["Select"] != null)
            {
                string SelectConfiguration = ConfigurationManager.AppSettings["Select"];
                string SelectGamepadType = SelectConfiguration.Split(',')[0];
                string SelectValue = SelectConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == SelectGamepadType) || SelectGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Select"] = new KeyValuePair<string, string>(SelectGamepadType, SelectValue);
                    if (SelectGamepadType == "Keyboard" && SelectValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(SelectValue, "Select");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["Start"] != null)
            {
                string StartConfiguration = ConfigurationManager.AppSettings["Start"];
                string StartGamepadType = StartConfiguration.Split(',')[0];
                string StartValue = StartConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == StartGamepadType) || StartGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["Start"] = new KeyValuePair<string, string>(StartGamepadType, StartValue);
                    if (StartGamepadType == "Keyboard" && StartValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(StartValue, "Start");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["A"] != null)
            {
                string AConfiguration = ConfigurationManager.AppSettings["A"];
                string AGamepadType = AConfiguration.Split(',')[0];
                string AValue = AConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == AGamepadType) || AGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["A"] = new KeyValuePair<string, string>(AGamepadType, AValue);
                    if (AGamepadType == "Keyboard" && AValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(AValue, "A");
                    }
                }
            }
            if (ConfigurationManager.AppSettings["B"] != null)
            {
                string BConfiguration = ConfigurationManager.AppSettings["B"];
                string BGamepadType = BConfiguration.Split(',')[0];
                string BValue = BConfiguration.Split(',')[1];
                if ((controller.IsConnected == true && controller.GetType().Name == BGamepadType) || BGamepadType == "Keyboard")
                {
                    Inputs.InputDictionary["B"] = new KeyValuePair<string, string>(BGamepadType, BValue);
                    if (BGamepadType == "Keyboard" && BValue != "Not Mapped")
                    {
                        InputKeyBoardDictionary.Add(BValue, "B");
                    }
                }
            }
        }
        public static void WriteInput4016(int Val)
        {
            if ((Val & 1) == 1)
            {
                Input4016 = 65;
                InputArray = new int[8];
                InputArray[0] = NewInputDictionary["B"];
                InputArray[1] = NewInputDictionary["A"];
                InputArray[2] = NewInputDictionary["Select"];
                InputArray[3] = NewInputDictionary["Start"];
                InputArray[4] = NewInputDictionary["Up"];
                InputArray[5] = NewInputDictionary["Down"];
                InputArray[6] = NewInputDictionary["Left"];
                InputArray[7] = NewInputDictionary["Right"];
               
                if (controller.IsConnected == true)
                {
                    var GamepadState = controller.GetState();
                    
                    if (GamepadState.Gamepad.Buttons != GamepadFlags)
                    {                       
                        string CurrentButtonsPressed = GamepadState.Gamepad.Buttons.ToString();
                        List<string> InputList = InputDictionary.Keys.ToList();
                        string GamepadType = controller.GetType().Name;
                        for (int i = 0; i < InputList.Count; i++)
                        {
                            if (InputDictionary[InputList[i]].Key== GamepadType)
                            {
                                if (CurrentButtonsPressed.IndexOf(InputDictionary[InputList[i]].Value) != -1) {
                                    NewInputDictionary[InputList[i]] = 1;
                                }
                            }
                        }
                        InputArray[0] = NewInputDictionary["B"];
                        InputArray[1] = NewInputDictionary["A"];
                        InputArray[2] = NewInputDictionary["Select"];
                        InputArray[3] = NewInputDictionary["Start"];
                        InputArray[4] = NewInputDictionary["Up"];
                        InputArray[5] = NewInputDictionary["Down"];
                        InputArray[6] = NewInputDictionary["Left"];
                        InputArray[7] = NewInputDictionary["Right"];
                        Inputs.NewInputDictionary["Up"] = 0;
                        Inputs.NewInputDictionary["Down"] = 0;
                        Inputs.NewInputDictionary["Left"] = 0;
                        Inputs.NewInputDictionary["Right"] = 0;
                        Inputs.NewInputDictionary["Select"] = 0;
                        Inputs.NewInputDictionary["Start"] = 0;
                        Inputs.NewInputDictionary["A"] = 0;
                        Inputs.NewInputDictionary["B"] = 0;
                    }
                }               
            }
            else
            {
                Input4016 = 65;
                InputCounter = 0;
             }

        }
        public static int ReadInput4017()
        {
            Input4017 = 64;
            return Input4017;
        }
        public static int ReadInput4016()
        {
            if (Input4016 == 65)
            {
                InputTrigger = true;
            }
            if (InputTrigger == true)
            {
                if (InputCounter >= 0)
                {
                    if ((InputArray[InputCounter] == 0))
                    {
                        Input4016 = 64;
                    }
                    else
                    {
                        Input4016 = 65;
                    }
                }
                InputCounter++;
                if (InputCounter == 8)
                {
                    InputTrigger = false;
                    InputCounter = 0;
                    InputArray = new int[8];
               }
            }
            return Input4016;
        }               
    }
}
