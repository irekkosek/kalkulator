namespace kalkulator
{
    using System.Data;
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
        private void UpdateCalcuationRichTextBox(string previousCalcuation, string ongoingCalculation)
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
            UpdateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }
        
        private void operatorButton_Click(object sender, EventArgs e)
        {
            string op = ((Button)sender).Text;
            previousCalculation = ongoingCalculation + " " + op + " ";
            ongoingCalculation = "";
            UpdateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
        }

        private void equalsButton_Click(object sender, EventArgs e)
        {
            //previousCalculation = Evaluate(previousCalculation or ongoing ?);
            previousCalculation += ongoingCalculation;
            ongoingCalculation = Evaluate(previousCalculation);
            previousCalculation += " " + "=" + " ";
            UpdateCalcuationRichTextBox(previousCalculation, ongoingCalculation);
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