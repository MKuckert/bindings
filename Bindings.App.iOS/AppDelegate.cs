using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace Bindings
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            new MvxIosBindingBuilder().DoRegistration();

            Window = new UIWindow();
            var viewController = UIStoryboard.FromName("Main", null).InstantiateInitialViewController();
            Window.RootViewController = new UINavigationController(viewController);
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}
