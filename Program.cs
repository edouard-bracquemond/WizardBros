using System;

namespace WizardBros
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static WizardBrosGame game;
        static void Main()
        {
            using (game = new WizardBrosGame())
                game.Run();
        }
    }
#endif
}
