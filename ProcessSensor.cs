using System.Linq;
using System.Diagnostics;
using FanControl.Plugins;

namespace FanControl.ProcessSensor
{
    /// <summary>
    /// A sensor that returns a temperature based on the presence of a process
    /// </summary>
    public class ProcessSensor : IPluginSensor
    {
        private int pass = 0;
        private string processName;
        private float notPresentTemperature;
        private float presentTemperature;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="processName">The process name to match</param>
        /// <param name="notPresentTemperature">Temperature to report if process not found</param>
        /// <param name="presentTemperature">Temperature to report if process found</param>
        public ProcessSensor(string processName, float notPresentTemperature, float presentTemperature)
        {
            this.processName = processName;
            this.notPresentTemperature = notPresentTemperature;
            this.presentTemperature = presentTemperature;
        }

        public string Id => $"Sensor_{processName}";

        public string Name => $"Sensor ({processName})";

        public float? Value { get; private set; }

        /// <summary>
        /// Updates the state  of the sensor. To reduce CPU load, this is only checked every 10 passes
        /// </summary>
        public void Update()
        {
            if (++pass % 10 == 1)
            {
                var processes = Process.GetProcesses();
                if (processes.Any(process => process.ProcessName.ToLower() == processName.ToLower()))
                {
                    Value = presentTemperature;
                }
                else
                {
                    Value = notPresentTemperature;
                }
            }
        }
    }
}
