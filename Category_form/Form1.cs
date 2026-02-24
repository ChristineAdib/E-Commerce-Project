using Microsoft.Web.WebView2.Core;

namespace Category_form
{
    public partial class Form1 : Form
    {
        // 1. دي البداية (constructor)
        public Form1()
        {
            InitializeComponent();
        }

        // 2. دي الشقة الأولى: دالة الـ Load (بتشتغل أول ما البرنامج يفتح)
        private async void Form1_Load(object sender, EventArgs e)
        {
            // بنشغل المحرك
            await webView21.EnsureCoreWebView2Async(null);

            // بننادي الدالة اللي تحت عشان ترسم لنا البيانات
            DisplayCategory("Smartphones", "The best phones in 2026", "https://via.placeholder.com/150");
        }

        // 3. دي الشقة التانية: الدالة اللي إنتِ لسه كاتباها (دالة الرسم)
        // لازم تكون "تحت" الـ Load وقبل "القوس" الأخير
        private void DisplayCategory(string name, string description, string imageUrl)
        {
            string html = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: sans-serif; padding: 20px; text-align: center; background-color: #f0f0f0; }}
                        .container {{ background: white; padding: 20px; border-radius: 15px; box-shadow: 0 4px 8px rgba(0,0,0,0.1); }}
                        img {{ width: 120px; border-radius: 50%; }}
                        h2 {{ color: #2c3e50; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <img src='{imageUrl}'>
                        <h2>{name}</h2>
                        <p>{description}</p>
                    </div>
                </body>
                </html>";

            webView21.NavigateToString(html);
        }

        private void webView21_Click(object sender, EventArgs e)
        {
            // سيبيها فاضية
        }
    }
} // <--- ده آخر قوس في الصفحة