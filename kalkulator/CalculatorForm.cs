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
        private void getClickedButton(object sender, EventArgs e)
        {
            string button = ((Button)sender).Text;

            if (Regex.Match(button, "^[0-9,]$").Success)
            {
                digitButton_Click(sender, e);
            }
            if (Regex.Match(button, "^[-+x÷]$").Success)
            {
                operatorButton_Click(sender, e);
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
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ongoingCalculation = "";
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }

        private void backspaceButton_Click(object sender, EventArgs e)
        {
            ongoingCalculation = ongoingCalculation.Substring(0, ongoingCalculation.Length - 1);
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);

        }

        private void updateCalcuationRichTextBox(string previousCalcuation, string ongoingCalculation)
        {
            calculationRichTextBox.Clear();
            calculationRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
            calculationRichTextBox.AppendText(previousCalculation + "\n");
            calculationRichTextBox.AppendText(ongoingCalculation);
        }

        private void digitButton_Click(object sender, EventArgs e)
        {
            string digit = ((Button)sender).Text;
            ongoingCalculation += digit;
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }
        
        private void operatorButton_Click(object sender, EventArgs e)
        {
            string op = ((Button)sender).Text;
            previousCalculation = ongoingCalculation + " " + op + " ";
            ongoingCalculation = "";
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }

        private void equalsButton_Click(object sender, EventArgs e)
        {
            //previousCalculation = Evaluate(previousCalculation or ongoing ?);
            previousCalculation += ongoingCalculation;
            ongoingCalculation = Evaluate(previousCalculation);
            previousCalculation += " " + "=" + " ";
            updateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }

        private string Evaluate(string expression)
        {
            expression = expression.Replace("x", "*");
            expression = expression.Replace("÷", "/");
            expression = expression.Replace(",", ".");

            DataTable table = new DataTable();
            object result_obj = table.Compute(expression, "");

            string result = result_obj.ToString();
            result = result.Replace(".", ",");

            return result; 
        }

    }
}