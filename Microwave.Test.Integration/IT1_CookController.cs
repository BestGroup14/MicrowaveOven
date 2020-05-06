using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
//using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;


namespace Microwave.Test.Integration
{
    [TestFixture]
    public class UnitTest1
    {
        private CookController _cookController;
        private Display _display;
        private PowerTube _powerTube;
        private Timer _timer;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Timer();
            _cookController = new CookController(_timer, _display, _powerTube);
        }

        [TestCase(1)]
        [TestCase(24)]
        [TestCase(66)]
        [TestCase(99)]
        [TestCase(100)]
        public void StartCooking_PowerTube(int power)
        {
            int time = 2;
            _cookController.StartCooking(power, time);
            
            _output.Received(1).OutputLine($"PowerTube works with {power}");
        }


        [TestCase(101)]
        [TestCase(150)]
        [TestCase(-1)]
        public void StartCooking__PowerTube_OutOfRangeException(int power)
        {
            int time = 2;

            NUnit.Framework.Assert.That(()=> _cookController.StartCooking(power, time),Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void StartCooking__PowerTube_TurnOnException()
        {
            int power = 50;
            int time = 2;
            
            _cookController.StartCooking(power, time);

            NUnit.Framework.Assert.That(() => _cookController.StartCooking(power, time), Throws.TypeOf<ApplicationException>());
        }


        [Test]
        public void StartCooking_PowerTube_Stop()
        {
            int power = 50;
            int time = 2;
            _cookController.StartCooking(power, time);
            _cookController.Stop();

            _output.Received(1).OutputLine($"PowerTube turned off");
        }

        [Test]
        public void StartCooking_Timer_Start()
        {
            int power = 50;
            int time = 2;
            _cookController.StartCooking(power, time);


            NUnit.Framework.Assert.That(_timer.TimeRemaining/1000, Is.EqualTo(time));
        }

        [Test]
        public void StartCooking_Display()
        {
            int power = 50;
            int time = 5;

            _cookController.StartCooking(power, time);
            time = time - 1000;

            int min = 0;
            int sec = 3;
            System.Threading.Thread.Sleep(2100);

            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
        }


        [Test]
        public void StartCooking_Expired()
        {
            int power = 50;
            int time = 5;

            _cookController.StartCooking(power, time);
            time = time - 1000;

            System.Threading.Thread.Sleep(5100);

            _output.Received(1).OutputLine($"PowerTube turned off");
        }
    }
}
