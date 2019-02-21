using System;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace Bindings
{
    public partial class ViewController : UIViewController, IMvxDataConsumer, IMvxBindingContextOwner
    {
        public IMvxBindingContext BindingContext { get; set; }
        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }
        public ViewModel ViewModel
        {
            get => (ViewModel)DataContext;
            set => DataContext = value;
        }

        protected ViewController(IntPtr handle) : base(handle)
        {
            this.CreateBindingContext();
            ViewModel = new ViewModel
            {
                StringProperty = "foo"
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = this.CreateBindingSet(ViewModel);
            set.Bind(this).For(v => v.Title).To(vm => vm.StringProperty);
            set.Bind(Button).For("TouchUpInside").To(vm => vm.Clicked);
            set.Bind(Label).For(v => v.Text).To(vm => vm.StringProperty);
            set.Bind(TextField).For(v => v.Text).To(vm => vm.StringProperty);
            set.Apply();
        }
    }
}
