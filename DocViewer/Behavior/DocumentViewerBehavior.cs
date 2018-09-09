using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DocViewer.Behavior
{
    public class DocumentViewerBehavior : Behavior<DocumentViewer>
    {
        public string PageNum
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(PageNum), typeof(string), typeof(DocumentViewerBehavior), new UIPropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PageViewsChanged += OnPageViewsChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PageViewsChanged -= OnPageViewsChanged;
        }

        private void OnPageViewsChanged(object sender, EventArgs e)
        {
        }
    }
}
