using GTA;

namespace CamCreator
{
    internal sealed class ControlsProvider
    {
        public ControlsProvider(ScriptSettings settings)
            => LoadControls(settings);

        public Control MoveUpDown { get; private set; } = Control.MoveUpDown;
        public Control MoveLeftRight { get; private set; } = Control.MoveLeftRight;
        public Control LookUpDown { get; private set; } = Control.LookUpDown;
        public Control LookLeftRight { get; private set; } = Control.LookLeftRight;
        public Control Sprint { get; private set; } = Control.Sprint;
        public Control IncreaseFov { get; private set; } = Control.WeaponWheelNext;
        public Control DecreaseFov { get; private set; } = Control.WeaponWheelPrev;
        public Control ReturnToPlayer { get; private set; } = Control.FrontendRright;
        public Control SaveCam { get; private set; } = Control.FrontendEndscreenExpand;
        public Control ToggleCam { get; private set; } = Control.ThrowGrenade;
        public Control ToggleHud { get; private set; } = Control.FrontendLeaderboard;

        private void LoadControls(ScriptSettings settings)
        {
            if (settings.TryGetValue("Controls", "MoveUpDown", out Control moveUpDown))
                MoveUpDown = moveUpDown;
            if (settings.TryGetValue("Controls", "MoveLeftRight", out Control moveLeftRight))
                MoveLeftRight = moveLeftRight;
            if (settings.TryGetValue("Controls", "LookUpDown", out Control lookUpDown))
                LookUpDown = lookUpDown;
            if (settings.TryGetValue("Controls", "LookLeftRight", out Control lookLeftRight))
                LookLeftRight = lookLeftRight;
            if (settings.TryGetValue("Controls", "Sprint", out Control sprint))
                Sprint = sprint;
            if (settings.TryGetValue("Controls", "IncreaseFov", out Control increaseFov))
                IncreaseFov = increaseFov;
            if (settings.TryGetValue("Controls", "DecreaseFov", out Control decreaseFov))
                DecreaseFov = decreaseFov;
            if (settings.TryGetValue("Controls", "ReturnToPlayer", out Control returnToPlayer))
                ReturnToPlayer = returnToPlayer;
            if (settings.TryGetValue("Controls", "SaveCam", out Control saveCam))
                SaveCam = saveCam;
            if (settings.TryGetValue("Controls", "ToggleCam", out Control toggleCam))
                ToggleCam = toggleCam;
            if (settings.TryGetValue("Controls", "Togglehud", out Control frontendLeaderboard))
                ToggleHud = frontendLeaderboard;
        }
    }
}
