using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public ReactiveProperty<string> PageNum { get; private set; }

        /// <summary>
        /// 総ページ数
        /// </summary>
        public ReactiveProperty<string> TotalPageNum { get; private set; }

        /// <summary>
        /// ドキュメント
        /// </summary>
        public ReactiveProperty<IDocumentPaginatorSource> Document { get; private set; }

        /// <summary>
        /// Loadコマンド
        /// </summary>
        public ReactiveCommand LoadCommand { get; private set; }

        public MainWindowViewModel()
        {
            this.PageNum = new ReactiveProperty<string>();
            this.TotalPageNum = new ReactiveProperty<string>();
            this.Document = new ReactiveProperty<IDocumentPaginatorSource>();
            this.LoadCommand = new ReactiveCommand();
            this.LoadCommand.Subscribe(_ => {
                this.Document.Value = LoadDocument();
                this.TotalPageNum.Value = this.Document.Value.DocumentPaginator.PageCount.ToString();
                this.PageNum.Value = "1";
            });
        }

        private string UpdatePageNum()
        {
            return "100";
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
