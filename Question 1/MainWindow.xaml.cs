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
            //LoadProgressBar.IsIndeterminate = true;
            var loadContent = Task.Run(() => ReadCSV());
            await loadContent;

            symbols = Stocks.Select(s => s.Symbol).Distinct().OrderBy(s => s);
            SymbolComboBox.ItemsSource = symbols;
            StockDataGrid.ItemsSource = Stocks;

            //var updateUIControls = new Task(() => 
            //{
            //});

            ////await loadContent;
        }

        private void ReadCSV()
        {
            string filePath = "stockData.csv";
            //string filePath = "stockDataTest.csv";
            string[] fields;

            TextFieldParser parser = new TextFieldParser(filePath);
            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");
            parser.ReadFields(); // ignores the header

            LoadProgressBar.BeginInit();

            while (!parser.EndOfData)
            {
                fields = parser.ReadFields();

                try
                {
                    Stock s = new Stock(fields[0],
                                            DateTime.Parse(fields[1]), //TODO: Show only the date, removing the time
                                            decimal.Parse(fields[2].Substring(1)),
                                            decimal.Parse(fields[3].Substring(1)),
                                            decimal.Parse(fields[4].Substring(1)),
                                            decimal.Parse(fields[5].Substring(1)));

                    Stocks = Stocks.Append(s);
                    //LoadProgressBar.Value ++;
                }
                catch (FormatException)
                {
                    continue;
                }
            }
        }

        private void UpdateStatusBar(int done = 0)
        {
            LoadProgressBar.Value = done;
        }

        private void SymbolComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSymbol = SymbolComboBox.SelectedItem.ToString();

            StockDataGrid.ItemsSource = Stocks.Where(s => s.Symbol == selectedSymbol).OrderBy(s => s.Date);
        }
    }
}
