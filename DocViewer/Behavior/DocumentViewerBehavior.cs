using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace DocViewer.Behavior
{
    public class DocumentViewerBehavior : Behavior<DocumentViewer>
    {
        private static Regex _regex = new Regex(@"(\d+)-(\d+)", RegexOptions.Compiled);

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
            var viewer = sender as DocumentViewer;
            var pages = viewer.PageViews;
            if (pages == null || pages.Count == 0) return;
         
            // ページ番号を \d-\d の形式に
            if (pages.Count > 1)
            {
                this.PageNum = (pages[0].PageNumber + 1) + "-" + (pages[pages.Count - 1].PageNumber + 1);
            }
            else
            {
                this.PageNum = (pages[0].PageNumber + 1).ToString();
            }
        }

        private static void ChangePageNum(object sender, DependencyPropertyChangedEventArgs e)
        {
            var behavior = sender as DocumentViewerBehavior;
            if (behavior == null) return;
            var viewer = behavior.AssociatedObject;

            var newValue = e.NewValue;
            var oldValue = e.OldValue;
            var newMatches = _regex.Matches(newValue.ToString());
            if (newMatches.Count > 0)
            {
                var newFirstNum = newMatches[0].Groups[1].Value;
                var newLastNum = newMatches[0].Groups[2].Value;

                var oldMatches = _regex.Matches(oldValue.ToString());
                if (oldMatches.Count > 0)
                {
                    var oldFirstNum = oldMatches[0].Groups[1].Value;
                    var oldLastNum = oldMatches[0].Groups[2].Value;

                    if (int.Parse(oldFirstNum) < int.Parse(newFirstNum))
                    {
                        // 次ページ遷移
                        if (viewer.CanGoToPage(int.Parse(newFirstNum)))
                        {
                            viewer.GoToPage(int.Parse(newFirstNum));
                        }
                    }
                    else
                    {
                        // 前のページが全て表示されている場合は、その1ページ前に遷移すべきだが判定方法が難しい
                        // そうでない場合は前のページに遷移
                        var pages = viewer.PageViews;
                        // 前ページ遷移
                        if (viewer.CanGoToPage(int.Parse(newFirstNum)))
                        {
                            viewer.GoToPage(int.Parse(newFirstNum));
                        }
                    }
                }

                return;
            }

            if (!int.TryParse(newValue.ToString(), out int newPageNum)) return;

            if (viewer.CanGoToPage(newPageNum))
            {
                viewer.GoToPage(newPageNum);
            }
        }
    }
}
