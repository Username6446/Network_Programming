using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project6_Gmail_FInal
{
    /// <summary>
    /// Interaction logic for EmailDetailWindow.xaml
    /// </summary>
    public partial class EmailDetailWindow : Window
    {
        public EmailDetailWindow()
        {
            InitializeComponent();
        }
        public EmailDetailWindow(EmailMessage email)
        {
            InitializeComponent();

            SubjectTxt.Text = email.Subject;
            FromTxt.Text = email.From;
            DateTxt.Text = email.Date.ToString("f");
            BodyTxt.Text = email.Body;
        }
    }
}
