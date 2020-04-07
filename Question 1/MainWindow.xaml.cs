using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;

namespace Question_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IEnumerable<Stock> Stocks;

        public MainWindow()
        {
            Stocks = new List<Stock>();

            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IOrderedEnumerable<string> symbols;
            LoadProgressBar.Minimum = 0;
            LoadProgressBar.Value = 0;

            await ReadCSV();

            PercentageLabel.Content = "";
            LoadProgressBar.Value = 0;

            symbols = Stocks.Select(s => s.Symbol).Distinct().OrderBy(s => s);
            SymbolComboBox.ItemsSource = symbols;
            StockDataGrid.ItemsSource = Stocks.OrderBy(s => s.Date);
        }

        private async Task ReadCSV()
        {
            string filePath = "stockData.csv";
            var lines = File.ReadAllLines(filePath);

            LoadProgressBar.Maximum = lines.Length - 1;

            for(int i = 1; i < lines.Length; i++)
            {
                var parser = new TextFieldParser(new StringReader(lines[i]));

                parser.TextFieldType = FieldType.Delimited;
                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                string[] fields = parser.ReadFields();

                await Task.Run(() => 
                { 
                    try
                    {
                        Stock s = new Stock(fields[0],
                                                DateTime.Parse(fields[1]), //TODO: Show only the date, removing the time
                                                decimal.Parse(fields[2].Substring(1)), //TODO: Format as curency when displaying
                                                decimal.Parse(fields[3].Substring(1)),
                                                decimal.Parse(fields[4].Substring(1)),
                                                decimal.Parse(fields[5].Substring(1)));

                        Stocks = Stocks.Append(s);
                    }
                    catch (FormatException)
                    {
                        // the negative values will fall here, as they are inside parentheses
                    }
                });

                var percentage = LoadProgressBar.Value / (LoadProgressBar.Maximum - LoadProgressBar.Minimum + 1);
                PercentageLabel.Content = $"{percentage:P0}";
                LoadProgressBar.Value ++;
            }
        }

        private void SymbolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSymbol = SymbolComboBox.SelectedItem.ToString();

            StockDataGrid.ItemsSource = Stocks.Where(s => s.Symbol == selectedSymbol).OrderBy(s => s.Date);
        }

        private void CalculateBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int factorial = int.Parse(FactorialTextBox.Text);
                int result = 1;

                for (int i = 1; i <= factorial; i++)
                {
                    result *= i;
                }

                ResultTextBox.Text = result.ToString();
            }
            catch (FormatException)
            {
                FactorialTextBox.Text = "Invalid input, enter an integer number";
                ResultTextBox.Text = "";
            }
        }

        private void StockDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Open" || e.PropertyName == "High" || e.PropertyName == "Low" || e.PropertyName == "Close")
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "c";
            }
            else if(e.PropertyName == "Date")
            {
               //(e.Column as DataGridTextColumn).Binding.
            }
        }
    }
}
