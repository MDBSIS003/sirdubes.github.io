using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net;
using System.Text;

namespace FuturePath.Business
{
    public sealed class ReportDocument
    {
        private string title = "";
        private DateTime generatedUtc;
        private int? sessionId;
        private string content = "";
        public string Title
        {
            get { return title; }

            set { title = value; }
        }

        public DateTime GeneratedUtc
        {
            get { return generatedUtc; }

            set { generatedUtc = value; }
        }

        public int? SessionId
        {
            get { return sessionId; }

            set { sessionId = value; }
        }

        public string Content
        {
            get { return content; }

            set { content = value; }
        }

        public string ToHtml()
        {
            return "<!doctype html><html lang=\"en\"><meta charset=\"utf-8\"><meta name=\"viewport\" content=\"width=device-width\">" + $"<title>{WebUtility.HtmlEncode(Title)}</title><style>body{{font:16px Segoe UI,Arial,sans-serif;color:#172f40;max-width:1000px;margin:40px auto;padding:24px}}" + "h1{color:#11695c}pre{white-space:pre-wrap;font:14px/1.65 Consolas,monospace;border-top:3px solid #168b76;padding-top:24px}" + "@media print{body{margin:0;padding:0}pre{font-size:10pt}}</style>" + $"<h1>{WebUtility.HtmlEncode(Title)}</h1><pre>{WebUtility.HtmlEncode(Content)}</pre></html>";
        }
    }
}
