using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DocViewer.Behavior
{
    public class PrevButtonBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(PrevButtonBehavior), new UIPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Click += OnClick;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Click -= OnClick;
        }

        private void OnClick(object sender, EventArgs e)
        {
            if (Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }
}
