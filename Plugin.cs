using FanControl.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FanControl.ProcessSensor
{
    /// <summary>
    /// Process sensor plugin
    /// </summary>
    public class ProcessSensorPlugin : IPlugin
    {
        public string Name => "Process Sensor Plugin";
        private List<ProcessSensor> sensors = new List<ProcessSensor>();

        public void Close()
        {
        }

        public void Initialize()
        {
            // Loads configured sensors. These are specified in a simple CSV format
            // NAME, PROCESS_NAMES, NOT PRESENT TEMP, PRESENT TEMP

            // Multiple process names can be specified using commas, provided the field occurs
            // in a quote
            // Demo, "notepad,calc", 10, 99
            // Lines starting with a # are explicitly ignored
            // Lines that don't match the regex below are implicitly ignored

            try
            {
                // Regex repeats  *(\"[^\"]+\"|[^\",]+) *,
                // This consumes leading/trailing whitespace, then match either everything within double quotes, or
                // anything up to the next comma
                var regex = new System.Text.RegularExpressions.Regex(" *(\"[^\"]+\"|[^\",]+) *, *(\"[^\"]+\"|[^\",]+) *, *(\"[^\"]+\"|[^\",]+) *, *(\"[^\"]+\"|[^\",]+) *$");
                var lines = System.IO.File.ReadAllLines("Plugins\\FanControl.ProcessSensor.cfg");

                var configurationLines = lines.Where(p => !p.StartsWith("#"))
                    .Select(p => regex.Match(p))
                    .Where(p => p.Success);

                foreach (var line in configurationLines)
                {
                    try
                    {
                        sensors.Add(new ProcessSensor(
                            line.Groups[1].Value,
                            line.Groups[2].Value.Split(',').Select(p => p.ToLower().Trim('\"', ' ')).ToList(),
                            float.Parse(line.Groups[3].Value),
                            float.Parse(line.Groups[4].Value)));
                    }
                    catch (FormatException)
                    {
                        // Malformed row. Ignore and continue
                    }
                }
            } catch(Exception e)
            {
                // Broad catch to handle all cases like 'configuration file missing'
            }
        }

        public void Load(IPluginSensorsContainer _container)
        {
            _container.TempSensors.AddRange(sensors);
        }
    }
}
