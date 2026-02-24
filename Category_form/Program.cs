namespace Category_form
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // التعديل هنا: أضفنا كلمة new وغيرنا القوسين ليكون اسم الفورم صح
            System.Windows.Forms.Application.Run(new Form1());
        }
    }
}