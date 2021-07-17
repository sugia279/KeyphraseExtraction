using KeyphraseExtraction.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyphraseExtraction
{
    /// <summary>
    /// Interaction logic for KeyphraseExtractionWindow.xaml
    /// </summary>
    public partial class KeyphraseExtractionResultWindow : Window
    {
        public KeyphraseExtractionResultWindow()
        {
            InitializeComponent(); 
            
        }

        private void CustomDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            // Adding 1 to make the row count start at 1 instead of 0
            // as pointed out by daub815
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void CustomDataGrid_LoadingRow_1(object sender, DataGridRowEventArgs e)
        {
            // Adding 1 to make the row count start at 1 instead of 0
            // as pointed out by daub815
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void CustomDataGrid_LoadingRow_2(object sender, DataGridRowEventArgs e)
        {
            // Adding 1 to make the row count start at 1 instead of 0
            // as pointed out by daub815
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void CustomDataGrid_LoadingRow_3(object sender, DataGridRowEventArgs e)
        {
            // Adding 1 to make the row count start at 1 instead of 0
            // as pointed out by daub815
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        

    }
}
