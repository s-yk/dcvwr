using Reactive.Bindings;
using System;
using System.IO;
using System.IO.Packaging;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace DocViewer.ViewModel
{
    public class MainWindowViewModel
    {
        /// <summary>
        /// ページ番号
        /// </summary>
        public ReactiveProperty<string> PageNum { get; set; } = new ReactiveProperty<string>();

        /// <summary>
        /// 総ページ数
        /// </summary>
        public ReactiveProperty<string> TotalPageNum { get; private set; } = new ReactiveProperty<string>();

        /// <summary>
        /// ドキュメント
        /// </summary>
        public ReactiveProperty<IDocumentPaginatorSource> Document { get; private set; } = new ReactiveProperty<IDocumentPaginatorSource>();

        public ReactiveProperty<bool> CanPrev { get; private set; } = new ReactiveProperty<bool>();
        public ReactiveProperty<bool> CanNext { get; private set; } = new ReactiveProperty<bool>();

        /// <summary>
        /// Loadコマンド
        /// </summary>
        public ReactiveCommand LoadCommand { get; private set; }

        /// <summary>
        /// 次ページコマンド
        /// </summary>
        public ReactiveCommand NextButtonCommand { get; private set; }

        /// <summary>
        /// 前ページコマンド
        /// </summary>
        public ReactiveCommand PrevButtonCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.NextButtonCommand = CanNext.ToReactiveCommand();
            this.NextButtonCommand.Subscribe(_ =>
            {
                if (int.TryParse(this.PageNum.Value, out int num))
                {
                    num++;
                    this.PageNum.Value = num.ToString();
                    if (PageNum.Value == TotalPageNum.Value) this.CanNext.Value = false;
                    this.CanPrev.Value = true;
                }
            });
            this.PrevButtonCommand = CanPrev.ToReactiveCommand();
            this.PrevButtonCommand.Subscribe(_ => 
            {
                if(int.TryParse(this.PageNum.Value, out int num))
                {
                    num--;
                    this.PageNum.Value = num.ToString();
                    if (PageNum.Value == "1") CanPrev.Value = false;
                    this.CanNext.Value = true;
                }
            });

            this.LoadCommand = new ReactiveCommand();
            this.LoadCommand.Subscribe(_ => 
            {
                this.Document.Value = LoadDocument();
                this.TotalPageNum.Value = this.Document.Value.DocumentPaginator.PageCount.ToString();
                this.PageNum.Value = "1";
            });
            this.CanPrev.Value = false;
            this.CanNext.Value = true;
        }

        private IDocumentPaginatorSource LoadDocument()
        {
            var filePath = Path.Combine(new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName,
                "TestData", "TestData.xps");
            IDocumentPaginatorSource source;
            using(var xpsDocument = new XpsDocument(filePath, FileAccess.Read, CompressionOption.NotCompressed))
            {
                source = xpsDocument.GetFixedDocumentSequence() as IDocumentPaginatorSource;
            }
            return source;
        }
    }
}
