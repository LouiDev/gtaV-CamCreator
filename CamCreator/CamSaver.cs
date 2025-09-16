using GTA;
using GTA.Native;
using GTA.UI;
using System.Globalization;
using System.IO;

namespace CamCreator
{
    internal sealed class CamSaver
    {
        private readonly Camera _camera;
        private readonly Script _script;

        public bool IsAskingForInput { get; private set; } = false;

        public CamSaver(Camera camera, Script script)
        {
            _camera = camera;
            _script = script;
        }

        public void AskForInput()
        {
            Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, 6, "FMMC_KEY_TIP8S", "", "", "", "", "", 20);
            IsAskingForInput = true;
        }

        public void Update()
        {
            if (!IsAskingForInput)
                return;

            int status = Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD);

            if (status == 0)
                return;
            else if (status == 1)
            {
                string fileName = Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Notification.PostTicker("~r~Filename can not be empty or whitespace!", true);
                    IsAskingForInput = false;
                    return;
                }

                SaveCamTo(fileName.Replace(' ', '_'));
            }

            IsAskingForInput = false;
        }

        private void SaveCamTo(string fileName)
        {
            var text = GetCamCreationString();
            var path = Path.Combine(_script.BaseDirectory, fileName + ".cs");

            if (!File.Exists(path))
                File.Create(path).Close();

            File.WriteAllText(path, text);
            Notification.PostTicker($"~g~Camera saved to ~y~{fileName}.cs", true);
        }

        private string GetCamCreationString()
        {
            var position = _camera.Position;
            var rotation = _camera.Rotation;
            var culture = new CultureInfo("en-US");
            return $"var cam = Camera.Create(ScriptedCameraNameHash.DefaultScriptedCamera, new Vector3({position.X.ToString(culture)}f, {position.Y.ToString(culture)}f {position.Z.ToString(culture)}f), new Vector3({rotation.X.ToString(culture)}f, {rotation.Y.ToString(culture)}f, {rotation.Z.ToString(culture)}f), {_camera.FieldOfView});";
        }
    }
}
