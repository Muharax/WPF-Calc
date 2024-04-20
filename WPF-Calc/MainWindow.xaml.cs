using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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
using static System.Net.Mime.MediaTypeNames;
using System.Threading;

using WPF_Calc;


namespace WPF_Calc
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string filePath = "wyniki.txt"; // Ścieżka do pliku tekstowego
            if (!File.Exists(filePath))
            {
                // Utwórz nowy pusty plik
                File.WriteAllText(filePath, "");
            }
            string[] allLines = File.ReadAllLines(filePath);
            Array.Reverse(allLines);
            string[] lastFiveLines = allLines.Take(5).ToArray();
            string lastFiveLinesString = string.Join(Environment.NewLine, lastFiveLines);
            txtRead.Text = lastFiveLinesString;
        }

        public class Calculator
        {
            public static string Calculate(string expression)
            {
                try
                {
                    // Wykonaj obliczenia
                    DataTable dt = new DataTable();
                    var result = dt.Compute(expression, "");

                    // Zwróć wynik jako string
                    return result.ToString();
                }
                catch (Exception ex)
                {
                    // Obsłuż błędy, np. błąd dzielenia przez zero
                    return "Błąd: " + ex.Message;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Pobierz zawartość klikniętego przycisku
            string clickedContent = clickedButton.Content.ToString();

            // Dodaj odpowiednią operację w zależności od klikniętego przycisku
            switch (clickedContent)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                case "%":
                    txtResult.Text += "" + clickedContent + "";
                    break;
                case ",":
                    // Pobierz tekst z kontrolki TextBox
                    string text = txtResult.Text;

                    // Sprawdź długość tekstu
                    int length = text.Length;

                    // Sprawdź, czy tekst nie jest pusty i czy ostatni znak to przecinek
                    if (length > 0 && text[length - 1] != ',')
                    {
                        // Dodaj przecinek tylko, jeśli ostatni znak nie jest przecinkiem
                        txtResult.Text += clickedContent;
                    }
                    // Jeśli ostatni znak to przecinek, nic nie rób

                    break;

                case "(":
                    txtResult.Text += clickedContent;
                    break;

                case ")":
                    txtResult.Text += clickedContent;
                    break;

                case "=":
                    // Pobierz tekst z kontrolki TextBox
                    string expression = txtResult.Text;





                    expression = expression.Replace(',', '.');

                    // Oblicz wynik wyrażenia matematycznego
                    string result = Calculator.Calculate(expression);



                    string filePath = "wyniki.txt"; // Ścieżka do pliku tekstowego
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        writer.WriteLine(expression + "=" + result); // Zapisz wynik w nowej linii
                    }
                    string[] allLines = File.ReadAllLines(filePath);
                    Array.Reverse(allLines);
                    string[] lastFiveLines = allLines.Take(5).ToArray();
                    string lastFiveLinesString = string.Join(Environment.NewLine, lastFiveLines);
                    txtRead.Text = lastFiveLinesString;

                    // Sprawdź, czy wynik kończy się na ",0" i jeśli tak, usuń ten fragment
                    if (result.EndsWith(",0"))
                    {
                        result = result.Substring(0, result.Length - 2); // Usuń ostatnie dwa znaki
                    }

                    // Dodaj wynik do pliku tekstowego


                    // Wyświetl wynik w kontrolce TextBox
                    txtResult.Text = result;
                    break;

                case "0":
                    if (txtResult.Text == "0")
                    {
                        txtResult.Text = "0,";
                    }
                    else
                    {
                        txtResult.Text += clickedContent;
                    }
                    break;
                default:

                    if (clickedContent == "0" && txtResult.Text == "0")
                    {
                        txtResult.Text = "0";
                    }



                    // Warunek sprawdzający, czy pole txtResult.Text jest równe "0"
                    if (txtResult.Text == "0" && (clickedContent != "0" && clickedContent != ","))
                    {
                        txtResult.Text = ""; // Zresetuj pole txtResult.Text
                    }

                    // Warunek sprawdzający, czy pole txtResult.Text nie jest równe "0" i czy została kliknięta cyfra od 1 do 9
                    if (txtResult.Text != "0" && (clickedContent != "0" && clickedContent != ","))
                    {
                        txtResult.Text += clickedContent; // Dodaj klikniętą cyfrę do pola txtResult.Text
                    }

                    break;
            }
        }




        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            // Wyczyść zawartość pola tekstowego
            txtResult.Text = "0";

            txtResult.Foreground = Brushes.Black;

        }



        private void MenuItem_Option1_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa opcji 1
        }

        private void MenuItem_Option2_Click(object sender, RoutedEventArgs e)
        {
            // Obsługa opcji 2
        }

        private void MenuItem_Option3_AboutWindow(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }

    }
}
