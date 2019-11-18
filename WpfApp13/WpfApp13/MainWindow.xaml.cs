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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WpfApp13
{
    public partial class MainWindow : Window
    {
        public static Char flagSP;   
        public MainWindow()
        {
            InitializeComponent();
            FillDataGrid();
        }
        private void FillDataGrid()
        {
            string conString = ConfigurationManager.ConnectionStrings["ConString"].ConnectionString;
            string cmdString = String.Empty;
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmdString = "SELECT * FROM nomenclature";
                SqlCommand cmd = new SqlCommand(cmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Nomenclature");
                sda.Fill(dt);
                grdNomencl.ItemsSource = dt.DefaultView;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flagSP = 'I';
            AddEdit addEditForm = new AddEdit();
            addEditForm.ShowDialog();
            FillDataGrid();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {            
            flagSP = 'U';
            AddEdit addEditForm = new AddEdit();
            addEditForm.ShowDialog();
            FillDataGrid();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {         
            flagSP = 'D';
            DeleteNom deleteNomForm = new DeleteNom();
            deleteNomForm.ShowDialog();
            FillDataGrid();
        }
    }
}
