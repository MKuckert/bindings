using Bindings.Core.Binding;
using Bindings.iOS.Binding;
using Foundation;
using UIKit;

namespace Bindings.App.iOS
{
    [Register(nameof(AppDelegate))]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AppDelegate : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            BindingBuilder.Create<IosBindingBuilder>();

            Window = new UIWindow();
            var viewController = UIStoryboard.FromName("Main", null).InstantiateInitialViewController();
            Window.RootViewController = new UINavigationController(viewController);
            Window.MakeKeyAndVisible();
            return true;
        }
    }
}
