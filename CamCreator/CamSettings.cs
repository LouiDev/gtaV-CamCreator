using GTA;

namespace CamCreator
{
    internal sealed class CamSettings
    {
        public CamSettings(ScriptSettings settings) 
            => LoadSettings(settings);

        public float DefaultSpeed { get; private set; } = 0.5f;
        public float DefaultSpeedMultiplier { get; private set; } = 3f;
        public int DefaultFov { get; private set; } = 65;
        public ControlsProvider ControlsProvider { get; private set; }

        private void LoadSettings(ScriptSettings settings)
        {
            ControlsProvider = new ControlsProvider(settings);
            
            if (settings.TryGetValue("DefaultValues", "Speed", out float defaultSpeed))
                DefaultSpeed = defaultSpeed;
            if (settings.TryGetValue("DefaultValues", "SpeedMultiplier", out float defaultSpeedMultiplier))
                DefaultSpeedMultiplier = defaultSpeedMultiplier;
            if (settings.TryGetValue("DefaultValues", "Fov", out int defaultFOV))
                DefaultFov = defaultFOV;
        }
    }
}
