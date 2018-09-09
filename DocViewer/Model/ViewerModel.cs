using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace DocViewer.Model
{
    public class ViewerModel
    {

        /// <summary>
        /// 総ページ数
        /// </summary>
        public string TotalPageNum { get; set; }

        /// <summary>
        /// ドキュメント
        /// </summary>
        public IDocumentPaginatorSource Document { get; set; }

        /// <summary>
        /// ドキュメント読み込み
        /// </summary>
        public void LoadDocument()
        {
            var filePath = Path.Combine(new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.FullName,
                "TestData", "TestData.xps");
            IDocumentPaginatorSource source;
            using (var xpsDocument = new XpsDocument(filePath, FileAccess.Read, CompressionOption.NotCompressed))
            {
                source = xpsDocument.GetFixedDocumentSequence() as IDocumentPaginatorSource;
            }

            this.Document = source;
            this.TotalPageNum = source.DocumentPaginator.PageCount.ToString();
        }
    }
}
