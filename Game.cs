// Include the namespaces (code libraries) you need below.
using System;
using System.Numerics;

// The namespace your code is in.
namespace MohawkGame2D
{
    /// <summary>
    ///     Your game code goes inside this class!
    /// </summary>
    public class Game
    {
        // Place your variables here:
        
        float _timeofday;
        
        // Framecount Variable.
        int frameCount = 0;
        
        


        /// <summary>
        ///     Setup runs once before the game loop begins.
        /// </summary>
        public void Setup()
        {
            Window.SetTitle("Dark Forest Quest");
            Window.SetSize(800, 600);
        }

        /// <summary>
        ///     Update runs every frame.
        /// </summary>
        public void Update()
        {
            _timeofday += Time.DeltaTime;
            
            
            if (_timeofday >= 12)
            {
                _timeofday = 0;
                //_timeofday -= Time.DeltaTime;
            }
            
            // if (_timeofday >= 24)
            {
                // _timeofday = 0;
            }
            
            // Color Variables
            // Sky
            float r = 0.0f;
            float g = _timeofday / 18;
            float b = _timeofday / 12;
            float c = _timeofday / 12;
            
            // Background
            ColorF bgColor = new ColorF(r, g, b, c);
            Window.ClearBackground(bgColor);
        }
    }

}
