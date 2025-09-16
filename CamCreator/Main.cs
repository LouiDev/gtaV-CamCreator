using GTA;

namespace CamCreator
{
    public sealed class Main : Script
    {
        private readonly FreeCam _freeCam;

        public Main()
        {
            Tick += (s, e) => _freeCam.Update();
            Aborted += (s, e) => _freeCam.Delete();
            
            _freeCam = new FreeCam(this);
        }
    }
}