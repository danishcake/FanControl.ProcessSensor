using FanControl.Plugins;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FanControl.ProcessSensor
{
    /// <summary>
    /// A sensor that returns a temperature based on the presence of a process
    /// </summary>
    public class ProcessSensor : IPluginSensor
    {
        private int pass = 0;
        private List<string> processNames;
        private float notPresentTemperature;
        private float presentTemperature;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sensorName">The name of the sensor to display in the UI</param>
        /// <param name="processNames">The process names to match</param>
        /// <param name="notPresentTemperature">Temperature to report if process not found</param>
        /// <param name="presentTemperature">Temperature to report if process found</param>
        public ProcessSensor(string sensorName, List<string> processNames, float notPresentTemperature, float presentTemperature)
        {
            Name = sensorName;
            this.processNames = processNames;
            this.notPresentTemperature = notPresentTemperature;
            this.presentTemperature = presentTemperature;
        }

        public string Id => $"Sensor_{string.Join("_", processNames)}";

        public string Name { get; private set; }

        public float? Value { get; private set; }

        /// <summary>
        /// Updates the state  of the sensor. To reduce CPU load, this is only checked every 10 passes
        /// </summary>
        public void Update()
        {
            if (++pass % 10 == 1)
            {
                var processes = Process.GetProcesses();

                if (processes.Any(process => processNames.Contains(process.ProcessName.ToLower())))
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
