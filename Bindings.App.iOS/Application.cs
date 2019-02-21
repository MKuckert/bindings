using UIKit;

namespace Bindings.App.iOS
{
    public static class Application
    {
        public static void Main(string[] args)
        {
            UIApplication.Main(args, null, nameof(AppDelegate));
        }
    }
}
