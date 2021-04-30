using FanControl.Plugins;

namespace FanControl.ProcessSensor
{
    /// <summary>
    /// Process sensor plugin
    /// </summary>
    public class ProcessSensorPlugin : IPlugin
    {
        public string Name => "Process Sensor Plugin";

        public void Close()
        {
        }

        public void Initialize()
        {
        }

        public void Load(IPluginSensorsContainer _container)
        {
            _container.TempSensors.Add(new ProcessSensor("notepad", 20, 100));
        }
    }
}
