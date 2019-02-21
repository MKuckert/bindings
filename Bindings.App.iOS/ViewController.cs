using System;
using Bindings.Core.Base;
using Bindings.Core.Binding.BindingContext;
using UIKit;

namespace Bindings.App.iOS
{
    public partial class ViewController : UIViewController, IDataConsumer, IBindingContextOwner
    {
        public IBindingContext BindingContext { get; set; }
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
