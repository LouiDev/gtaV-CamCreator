using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using System;

namespace CamCreator
{
    internal sealed class FreeCam
    {
        private readonly CamSettings _settings;
        private readonly Script _script;
        private Camera _cam;
        private Scaleform _scaleform;
        private CamSaver _camSaver;
        private bool _showHud;

        public FreeCam(Script script)
        {
            _script = script;
            _settings = new CamSettings(_script.Settings);
            _showHud = true;
            InitScaleform();
        }

        public bool IsActive
        {
            get 
            { 
                return _cam != null && _cam.IsActive; 
            }
            set
            {
                if (value)
                    EnableOrCreateCam();
                else
                    DisableCam();

                Game.Player.SetControlState(!value);
            }
        }

        public bool ShowHud
        {
            get => _showHud;
            set
            {
                _showHud = value;
                
                Function.Call(Hash.DISPLAY_RADAR, value);
            }
        }

        public Vector3 Position
        {
            get => _cam.Position;
            set => _cam.Position = value;
        }

        public Vector3 ForwardVector
        {
            get => _cam.ForwardVector;
        }

        public Vector3 RightVector
        {
            get => _cam.RightVector;
        }

        public Vector3 DefaultPosition
        {
            get 
            {
                var player = Game.Player.Character;
                return player.Position + player.ForwardVector * -3;
            }
        }

        private void EnableOrCreateCam()
        {
            if (_cam == null)
                _cam = Camera.Create(ScriptedCameraNameHash.DefaultScriptedCamera, DefaultPosition, Vector3.Zero);

            _cam.IsActive = true;
            ScriptCameraDirector.StartRendering();
        }

        private void DisableCam()
        {
            if (_cam == null)
                return;

            _cam.IsActive = false;
            ScriptCameraDirector.StopRendering();
        }

        private void InitScaleform()
        {
            var controls = _settings.ControlsProvider;

            _scaleform = Scaleform.RequestMovie("INSTRUCTIONAL_BUTTONS");

            var failSafeAttempts = 0;
            while (!_scaleform.IsLoaded && failSafeAttempts < 1000)
            {
                failSafeAttempts++;
                Script.Yield();
            }

            if (failSafeAttempts >= 100)
                Notification.PostTicker("[CamCreator] ~r~Failed to load scaleform. Please reload scripts.", true);

            _scaleform.CallFunction("CLEAR_ALL");
            _scaleform.CallFunction("TOGGLE_MOUSE_BUTTONS", 0);
            _scaleform.CallFunction("CREATE_CONTAINER");

            _scaleform.CallFunction("SET_DATA_SLOT", 0, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)controls.ReturnToPlayer), "Return to Player");
            _scaleform.CallFunction("SET_DATA_SLOT", 1, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)controls.SaveCam), "Save camera");
            _scaleform.CallFunction("SET_DATA_SLOT", 2, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)controls.SaveCam), "Toggle Hud");
            _scaleform.CallFunction("SET_DATA_SLOT", 3, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)controls.DecreaseFov), "Decrease Fov");
            _scaleform.CallFunction("SET_DATA_SLOT", 4, Function.Call<string>(Hash.GET_CONTROL_INSTRUCTIONAL_BUTTONS_STRING, 0, (int)controls.IncreaseFov), "Increase Fov");

            _scaleform.CallFunction("DRAW_INSTRUCTIONAL_BUTTONS", -1);
            _scaleform.CallFunction("SET_BACKGROUND_COLOUR", 0, 0, 0, 80);
        }

        public void Update()
        {
            var controls = _settings.ControlsProvider;

            if (Game.IsControlJustReleased(controls.ToggleCam))
                IsActive = !IsActive;

            if (!IsActive) 
                return;

            _camSaver?.Update();

            if (_camSaver != null && _camSaver.IsAskingForInput)
                return;

            #region Scaleform
            if (ShowHud)
                _scaleform.Render2D();
            #endregion

            #region Movement
            var position = _cam.Position;
            var sprintMultiplier = Game.IsControlPressed(controls.Sprint) ? 3f : 1f;
            var speed = 0.5f;
            var upDownDirection = Game.GetDisabledControlValueNormalized(controls.MoveUpDown);
            var leftRightDirection = Game.GetDisabledControlValueNormalized(controls.MoveLeftRight);

            position += _cam.ForwardVector * (-upDownDirection * speed * sprintMultiplier);
            position += _cam.RightVector * (leftRightDirection * speed * sprintMultiplier);

            _cam.Position = position;
            #endregion

            #region Rotation
            Game.EnableControlThisFrame(controls.LookLeftRight);
            Game.EnableControlThisFrame(controls.LookUpDown);

            var rotation = _cam.Rotation;
            float mouseX = Game.GetDisabledControlValueNormalized(controls.LookLeftRight);
            float mouseY = Game.GetDisabledControlValueNormalized(controls.LookUpDown);
            float sensitivity = 5.0f;

            rotation.Z -= mouseX * sensitivity;
            rotation.X -= mouseY * sensitivity;

            rotation.X = Math.Max(-89f, Math.Min(89f, rotation.X));

            _cam.Rotation = rotation;
            #endregion

            #region FOV
            if (Game.IsControlPressed(controls.IncreaseFov))
                _cam.FieldOfView++;

            if (Game.IsControlPressed(controls.DecreaseFov))
                _cam.FieldOfView--;
            #endregion

            #region Return to player
            if (Game.IsControlJustReleased(controls.ReturnToPlayer))
                _cam.Position = DefaultPosition;
            #endregion

            #region Toggle Hud
            if (Game.IsControlJustReleased(controls.ToggleHud))
                ShowHud = !ShowHud;
            #endregion

            #region Cam Saver
            if (Game.IsControlJustReleased(controls.SaveCam))
            {
                if (_camSaver != null && _camSaver.IsAskingForInput)
                    return;

                _camSaver = new CamSaver(_cam, _script);
                _camSaver.AskForInput();
            }
            #endregion
        }

        public void Delete()
        {
            IsActive = false;
            ShowHud = true;

            _cam?.Delete();
            _cam = null;
        }
    }
}
