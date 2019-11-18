using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp13
{
    public partial class DeleteNom : Window
    {
        public DeleteNom()
        {
            InitializeComponent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorMessages = new StringBuilder();

            MessageBoxResult result = MessageBox.Show("Вы уверены что хотите удалить запись из БД?",
                "Предупреждение", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {

                using (var conn = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDb;
                Initial Catalog=ITPark;Integrated Security=True"))
                using (var command = new SqlCommand("iud_nomenclature", conn)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    SqlParameter flagParam = new SqlParameter
                    {
                        ParameterName = "@flag",
                        Value = MainWindow.flagSP
                    };
                    command.Parameters.Add(flagParam);

                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@id_nomenclature",
                        Value = IdTxtBx.Text
                    };
                    command.Parameters.Add(idParam);

                    int countResult = 0;
                    try
                    {
                        conn.Open();
                        countResult = command.ExecuteNonQuery();

            
                    }
                    catch (SqlException ex)
                    {
                        for (int i = 0; i < ex.Errors.Count; i++)
                        {
                            errorMessages.Append("Index #" + i + "\n" +
                                "Ошибка: " + ex.Errors[i].Message + "\n" +
                                "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                                "Источник: " + ex.Errors[i].Source + "\n" +
                                "Процедура: " + ex.Errors[i].Procedure + "\n");
                        }
                        MessageBox.Show(errorMessages.ToString(), "Ошибка");
                    }  
                    finally
                    {
                        if (countResult == 1)
                        {
                            MessageBox.Show("Номенклатура успешно удалена.", "Результат операции");
                            this.DialogResult = true;
                        }
                            
                        else
                            MessageBox.Show("Номенклатура с указанным ID не найдена. Проверьте корректность введенных данных.",
                                "Ошибка");

                        conn.Close();
                    }
                }
            }
            else if (result == MessageBoxResult.No)
            {

            }
            else if (result == MessageBoxResult.Cancel)
            {

            }
        }

        private void IdTxtBx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void IdTxtBx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
