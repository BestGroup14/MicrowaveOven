using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT2_UserInterface
    {
        private Display _display;
        private Light _light;
        private IOutput _fakeOutput;
        private UserInterface _userInterface;
        private IButton _fakeButton;
        private IButton _timeButton;
        private IButton _startCancelButton;
        private IDoor _door;
        private Timer _timer;
        private PowerTube _powertube;
        private CookController _cookController;


        [SetUp]
        public void SetUp()
        {
            _fakeOutput = Substitute.For<IOutput>();
            _display = new Display(_fakeOutput);
            _light = new Light(_fakeOutput);
            _fakeButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _timer = new Timer();
            _powertube = new PowerTube(_fakeOutput);
            _cookController = new CookController(_timer,_display,_powertube);
            _userInterface = new UserInterface(_fakeButton,_timeButton,_startCancelButton,_door,_display,_light,_cookController);

        }

        [Test]
        public void Userinterface_Light_TurnOff()
        {
            _door.Opened += Raise.EventWith(this,new EventArgs());
            _door.Closed += Raise.EventWith(this, new EventArgs());
            _fakeOutput.Received(1).OutputLine("Light is turned off");
        }
        
        [Test]
        public void Userinterface_Light_TurnOn()
        {
            _door.Opened += Raise.EventWith(this,new EventArgs());
            _fakeOutput.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void Userinterface_Display_ShowPower()
        {
            int power = 50;
            _fakeButton.Pressed += Raise.EventWith(this, new EventArgs());
            _fakeOutput.Received(1).OutputLine($"Display shows: {power} W");
        }

        [Test]
        public void Userinterface_Display_ShowTime()
        {
            _fakeButton.Pressed += Raise.EventWith(this, new EventArgs());
            _timeButton.Pressed += Raise.EventWith(this, new EventArgs());

            int min = 1;
            int sec = 0; 
            _fakeOutput.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }



    }
}
