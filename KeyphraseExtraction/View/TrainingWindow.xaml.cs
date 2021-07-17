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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class TrainingWindow : Window
    {
        public TrainingWindow()
        {
            InitializeComponent();
            vm = new TrainingViewModel(this);
        }

        TrainingViewModel vm = null;

    }
}
