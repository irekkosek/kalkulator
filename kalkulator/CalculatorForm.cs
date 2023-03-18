namespace kalkulator
{
    using System.Data;
    using System.Text.RegularExpressions;
    public partial class CalculatorForm : Form
    {
        private string previousCalculation;
        private string ongoingCalculation;

        public CalculatorForm()
        {
            previousCalculation = "";
            ongoingCalculation = "";
            InitializeComponent();
        }

        private void updateCalcuationRichTextBox(string previousCalcuation, string ongoingCalculation)
        {
            calculationRichTextBox.Clear();
            calculationRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            calculationRichTextBox.AppendText(previousCalculation + "\n");
            calculationRichTextBox.AppendText(ongoingCalculation);
        }
        private void refreshCalcuationRichTextBox()
        {
            calculationRichTextBox.Clear();
            calculationRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            calculationRichTextBox.AppendText(previousCalculation + "\n");
            calculationRichTextBox.AppendText("");
        }

        private void getClickedButton(object sender, EventArgs e)
        {
            string button = ((Button)sender).Text;

            if (Regex.Match(ongoingCalculation, "nie można dzielić przez zero").Success || Regex.Match(ongoingCalculation, "error").Success)
            {
                ongoingCalculation = "";
                updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
            }


            if (Regex.Match(previousCalculation, "= $").Success)
            {
                previousCalculation = previousCalculation.Replace("= ", "");
                if (Regex.Match(button, "^=$").Success)
                {
                    var match = Regex.Match(previousCalculation, "([-+x÷] [0-9,.]) $");
                    previousCalculation = ongoingCalculation + " ";
                    previousCalculation += match.Groups[1];
                    ongoingCalculation = "";
                }
                if (Regex.Match(button, "^[-+x÷]$").Success)
                {
                    previousCalculation = "";
                }

            }

            if (Regex.Match(button, "^[0-9,]$").Success)
            {
                if (Regex.Match(ongoingCalculation, ",").Success && Regex.Match(button, "^[,]$").Success)
                {
                    return;
                }
                refreshCalcuationRichTextBox();
                digitButton_Click(sender, e);
            }
            if (Regex.Match(button, "^[-+x÷]$").Success)
            {
                operatorButton_Click(sender, e);
                ongoingCalculation = "";
            }
            if (Regex.Match(button, "^[=]$").Success)
            {
                equalsButton_Click(sender, e);
            }
            if (Regex.Match(button, "^⁺/₋$").Success)
            {
                plusMinusButton_Click(sender, e);
            }
            if (Regex.Match(button, "^[⌫]$").Success)
            {
                backspaceButton_Click(sender, e);
            }
            if (Regex.Match(button, "^[C]$").Success)
            {
                clearButton_Click(sender, e);
            }
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }

        private void digitButton_Click(object sender, EventArgs e)
        {
            if (Regex.Match(previousCalculation, " [-+x÷] $").Success && !Regex.Match(ongoingCalculation, ".+").Success)
            {
                ongoingCalculation = "";
                updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
            }
            if (Regex.Match(previousCalculation, "=").Success && !Regex.Match(ongoingCalculation, ".+").Success)
            {
                previousCalculation = "";
                ongoingCalculation = "";
                updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
            }
            string digit = ((Button)sender).Text;
            ongoingCalculation += digit;
            

        }

        private void plusMinusButton_Click(object sender, EventArgs e)
        {
            if (ongoingCalculation.StartsWith("-")){
                ongoingCalculation = ongoingCalculation.Substring(1);
            }
            else
            {
                ongoingCalculation = "-" + ongoingCalculation;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            previousCalculation = "";
            ongoingCalculation = "";
        }

        private void backspaceButton_Click(object sender, EventArgs e)
        {
            if (ongoingCalculation.Length > 0)
            {
                ongoingCalculation = ongoingCalculation.Substring(0, ongoingCalculation.Length - 1);
            }
        }

        private void operatorButton_Click(object sender, EventArgs e)
        {
            string op = ((Button)sender).Text;
            if (Regex.Match(ongoingCalculation, "^[0-9.,]+$").Success)
            {
                previousCalculation += ongoingCalculation + " " + op + " ";

                //ongoingCalculation = "";
            }
            else if (Regex.Match(previousCalculation, "[-+x÷] $").Success) //if op present
            {
                previousCalculation = previousCalculation.Substring(0, previousCalculation.Length - 3);
                previousCalculation += ongoingCalculation + " " + op + " ";
                return;
            }

        }

        private void equalsButton_Click(object sender, EventArgs e)
        {
            //previousCalculation = Evaluate(previousCalculation or ongoing ?);
            string expression=previousCalculation + ongoingCalculation;
            if (Regex.Match(expression, "÷ 0$").Success)
            {
                dividedByZero();
                return;
            }
            if (Regex.Match(expression, "^[0-9,.]+( [-+x÷] [0-9,.]+)+$").Success)
            {
                previousCalculation += ongoingCalculation + " " + "=" + " ";
                ongoingCalculation = Evaluate(expression);
            }
        }

        private void dividedByZero()
        {
            calculationRichTextBox.Clear();
            calculationRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            calculationRichTextBox.AppendText(previousCalculation + "\n");
            ongoingCalculation = "nie można dzielić przez zero";
            calculationRichTextBox.AppendText(ongoingCalculation);
        }

        private void errorAlert()
        {
            calculationRichTextBox.Clear();
            calculationRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            calculationRichTextBox.AppendText(previousCalculation + "\n");
            ongoingCalculation = "error";
            calculationRichTextBox.AppendText(ongoingCalculation);
        }


        private string Evaluate(string expression)
        {
            expression = expression.Replace("x", "*");
            expression = expression.Replace("÷", "/");
            expression = expression.Replace(",", ".");
            expression = expression.Replace("=", "");


            object result_obj = "error";
            if (Regex.Match(expression, "^[0-9.]+( [-+*/] [0-9.]+)+$").Success)
            {
                DataTable table = new DataTable();
                result_obj = table.Compute(expression, "");
            }
            if (result_obj == "error")
            {
                errorAlert();
                return "";
            }
            
            
            string result = result_obj.ToString();
            result = result.Replace(".", ",");

            return result; 
        }

    }
}