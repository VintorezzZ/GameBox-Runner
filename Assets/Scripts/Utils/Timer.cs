namespace Utils
{
    public class Timer
    {
        public float _startTime = -1;
        private float _pauseTime = -1;

        public Timer(bool instantStart = false)
        {
            if (instantStart)
                Start();
        }

        public void Start(float startTime = 0)
        {
            _startTime = UnityEngine.Time.realtimeSinceStartup - startTime;
            _pauseTime = -1;
        }

        public void Stop()
        {
            _startTime = -1;
        }

        public void SetPause(bool pause)
        {
            if (pause)
                _pauseTime = UnityEngine.Time.realtimeSinceStartup;
            else
                Start(Time);
        }

        public float Time
        {
            get
            {
                if (_startTime < 0)
                    return 0;
                if (_pauseTime >= 0)
                    return _pauseTime - _startTime;
                return UnityEngine.Time.realtimeSinceStartup - _startTime;
            }
        }

        public bool IsStarted
        {
            get { return _startTime >= 0; }
        }

        public bool IsPaused
        {
            get { return _pauseTime >= 0; }
        }
    }
}