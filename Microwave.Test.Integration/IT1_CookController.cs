using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;


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

        [Test]
        public void StartCooking_PowerTube()
        {
            int power = 50;
            int time = 2;
            _cookController.StartCooking(power, time);
            
            _output.Received(1).OutputLine($"PowerTube works with {power}");
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
        public void StartCooking_Timer()
        {
            int power = 50;
            int time = 2;
            _cookController.StartCooking(power, time);

            NUnit.Framework.Assert.That(_timer.TimeRemaining, Is.EqualTo(time));
        }

        [Test] //Hvordan testes stop inde  timer da den er privat?
        public void StartCooking_Timer_Stop()
        {
            int power = 50;
            int time = 2;
            _cookController.StartCooking(power, time);

        }

        [Test]
        public void StartCooking_Display()
        {
            int power = 50;
            int time = 5000;

            _cookController.StartCooking(power, time);
            time = time - 1000;

            int min = time / 60;
            int sec = time % 60;
            System.Threading.Thread.Sleep(2100);

            _output.Received(1).OutputLine($"Display shows: {min:D2}:{sec:D2}");
           
        }
    }
}
