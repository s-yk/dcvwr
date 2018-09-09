using DocViewer.Model;
using Reactive.Bindings;
using System;
using System.IO;
using System.IO.Packaging;
using System.Reactive.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace DocViewer.ViewModel
{
    public class MainWindowViewModel
    {
        private Regex _regex = new Regex(@"(\d+)-(\d+)", RegexOptions.Compiled);

        /// <summary>
        /// モデル
        /// </summary>
        private ViewerModel _model;

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

        /// <summary>
        /// 前コマンド実行可否
        /// </summary>
        public ReactiveProperty<bool> CanPrev { get; private set; } = new ReactiveProperty<bool>();

        /// <summary>
        /// 次コマンド実行可否
        /// </summary>
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

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="model"></param>
        public MainWindowViewModel(ViewerModel model)
        {
            this._model = model;

            // Command
            // Next
            this.NextButtonCommand = CanNext.ToReactiveCommand();
            this.NextButtonCommand.Subscribe(_ =>
            {
                if (int.TryParse(this.PageNum.Value, out int num))
                {
                    num++;
                    UpdatePageNum(num.ToString());
                }
                else
                {
                    var matches = _regex.Matches(this.PageNum.Value);
                    if (matches.Count > 0)
                    {
                        var first = matches[0].Groups[1].Value;
                        var last = matches[0].Groups[2].Value;
                        this.PageNum.Value = (int.Parse(first) + 1) + "-" + (int.Parse(last) + 1);
                    }
                }
            });

            // Prev
            this.PrevButtonCommand = CanPrev.ToReactiveCommand();
            this.PrevButtonCommand.Subscribe(_ => 
            {
                if(int.TryParse(this.PageNum.Value, out int num))
                {
                    num--;
                    UpdatePageNum(num.ToString());
                }
                else
                {
                    var matches = _regex.Matches(this.PageNum.Value);
                    if (matches.Count > 0)
                    {
                        var first = matches[0].Groups[1].Value;
                        var last = matches[0].Groups[2].Value;
                        this.PageNum.Value = (int.Parse(first) - 1) + "-" + (int.Parse(last) - 1);
                    }
                }
            });

            // Load
            this.LoadCommand = new ReactiveCommand();
            this.LoadCommand.Subscribe(_ => 
            {
                this._model.LoadDocument();
                this.TotalPageNum.Value = this._model.TotalPageNum;
                this.Document.Value = this._model.Document;

                // ドキュメントロード時のページがデフォルトで1なので個別に設定しておく
                UpdatePageNum("1");
            });
        }

        /// <summary>
        /// ページ番号更新
        /// </summary>
        /// <param name="updateNum"></param>
        private void UpdatePageNum(string updateNum)
        {
            this.PageNum.Value = updateNum;
            ApplyButtonEnable();
        }

        /// <summary>
        /// ボタン活性更新
        /// </summary>
        private void ApplyButtonEnable()
        {
            if (this.TotalPageNum.Value == "1")
            {
                this.CanNext.Value = false;
                this.CanPrev.Value = false;
            }
            else if (this.PageNum.Value == this.TotalPageNum.Value)
            {
                this.CanNext.Value = false;
                this.CanPrev.Value = true;
            }
            else if (this.PageNum.Value == "1")
            {
                this.CanNext.Value = true;
                this.CanPrev.Value = false;
            }
            else
            {
                this.CanNext.Value = true;
                this.CanPrev.Value = true;
            }
        }
    }
}
