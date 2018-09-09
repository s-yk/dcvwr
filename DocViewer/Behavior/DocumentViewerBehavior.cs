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
            get { return (string)GetValue(PageNumProperty); }
            set { SetValue(PageNumProperty, value); }
        }

        public static readonly DependencyProperty PageNumProperty =
            DependencyProperty.Register(nameof(PageNum), typeof(string), typeof(DocumentViewerBehavior), new PropertyMetadata(ChangePageNum));

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

        private static void ChangePageNum(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!int.TryParse(e.NewValue.ToString(), out int newValue)) return;

            var behavior = sender as DocumentViewerBehavior;
            if (behavior == null) return;

            var viewer = behavior.AssociatedObject;
            if (viewer.CanGoToPage(newValue))
            {
                viewer.GoToPage(newValue);
            }
        }
    }
}
