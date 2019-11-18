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
    public partial class AddEdit : Window
    {
        public AddEdit()
        {
            InitializeComponent();
        }

        private void AddEdit_Click(object sender, RoutedEventArgs e)
        {
            String check = "OK";
            StringBuilder errorMessages = new StringBuilder();

            if ((IdTxtBx.Text == "") && (MainWindow.flagSP == 'U'))
            {
                MessageBox.Show("Укажите ID номенклатуры, которую хотите исправить.", 
                    "Ошибка");
                check = "Error";
            }

            if (nameTxtBx.Text == "")
            {
                check = "Error";
                MessageBox.Show("Не заполнено наименование.", "Ошибка");                
            }

            if (priceTxtBx.Text == "")
            {
                check = "Error";
                MessageBox.Show("Не заполнена цена.", "Ошибка");
            }

            string pricePattern = @"(^[0-9]+([.][0-9][0-9]?)?$)";

            if (check == "OK")
                if (!Regex.IsMatch(priceTxtBx.Text, pricePattern))
                {
                    check = "Error";
                    MessageBox.Show("Неверный формат цены. Проверьте введенные данные.", "Ошибка");
                }
            
            if(fromDateTxtBx.SelectedDate > toDateTxtBx.SelectedDate)
            {
                check = "Error";
                MessageBox.Show("Начальная дата не может быть больше конечной даты.", "Ошибка");
            }

            if (fromDateTxtBx.SelectedDate == null || toDateTxtBx.SelectedDate == null)
            {
                check = "Error";
                MessageBox.Show("Не заполнена дата действия цены", "Ошибка");
            }

            if (check == "OK")
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

                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@name",
                        Value = nameTxtBx.Text
                    };
                    command.Parameters.Add(nameParam);

                    SqlParameter priceParam = new SqlParameter
                    {
                        ParameterName = "@price",
                        Value = priceTxtBx.Text
                    };
                    command.Parameters.Add(priceParam);

                    SqlParameter fromDateParam = new SqlParameter
                    {
                        ParameterName = "@fromDate",
                        Value = fromDateTxtBx.Text
                    };
                    command.Parameters.Add(fromDateParam);

                    SqlParameter toDateParam = new SqlParameter
                    {
                        ParameterName = "@toDate",
                        Value = toDateTxtBx.Text
                    };
                    command.Parameters.Add(toDateParam);
                    
                    try
                    {
                        conn.Open();
                        int countResult = command.ExecuteNonQuery();

                        if (countResult == 1)
                            MessageBox.Show("Данные переданы успешно.", "Результат операции");
                        else
                        {
                            MessageBox.Show("Данные не были переданы. Проверьте корректность " +
                             "введенных данных.", "Ошибка");
                        }
                            
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
                        MessageBox.Show(errorMessages.ToString());
                    }
                    catch (Exception exep)
                    {
                        MessageBox.Show("Произошла ошибка при работе с БД. Повторите операцию. Описание ошибки: " + exep.Message, "Ошибка");
                    }
                    finally
                    {
                        conn.Close();
                    }

                    this.DialogResult = true;
                }
            }         
        }

        private void IdTxtBx_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void IdTxtBx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PriceTxtBx_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {  
            Regex regex = new Regex("[0-9]|[.]");

            e.Handled = !regex.IsMatch(e.Text);
        }
    }
}
