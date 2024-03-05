using System;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace InputAndSendMessage
{
    public partial class Form1 : Form
    {
        private SpeechSynthesizer synthesizer;
        const string AccountSid = "ACa6928524470645180c62694e7c43fcc2";
        const string AuthToken = "78de6c3f5e9dc2de68447ad5a24e3059";
        const string SenderNumber = "+12513330115";

        public Form1()
        {
            InitializeComponent();
            synthesizer = new SpeechSynthesizer();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TwilioClient.Init(AccountSid, AuthToken);
            string inputText = textBox1.Text;

            try
            {
                if (ValidatePhoneNumber(inputText))
                {
                    var phoneNumberRecieve = "+84" + inputText.Substring(1);
                    SendSMS(phoneNumberRecieve);
                }
                else if (ValidateInternationalNumber(inputText))
                {
                    ConvertAndSpeakDigits(inputText);
                }
                else
                {
                    // Handle invalid input scenario (e.g., display an error message)
                    label1.Text = "Invalid input. Please enter a valid 10-digit phone number or a 14-digit international number starting with '0'.";
                }
            }
            catch (TwilioException ex)
            {
                // Handle Twilio API exceptions (e.g., log the error or display a user-friendly message)
                label1.Text = "An error occurred: " + ex.Message;
            }
        }

        // Improved validation functions with reusability and clarity
        private bool ValidatePhoneNumber(string inputText) => inputText.Length == 10 && inputText.All(char.IsDigit);
        private bool ValidateInternationalNumber(string inputText) => inputText.Length == 14;

        // Refactored sending logic with clarity and exception handling
        private void SendSMS(string phoneNumber)
        {
            try
            {
                MessageResource.Create(
                    to: new PhoneNumber(phoneNumber),
                    from: new PhoneNumber(SenderNumber),
                    body: "hello!!!!"
                );
                label1.Text = "SMS sent successfully!";
            }
            catch (TwilioException ex)
            {
                label1.Text = $"SMS sending failed: {ex.Message}";
            }
        }

        // Consolidated logic for converting and speaking digits
        private void ConvertAndSpeakDigits(string inputText)
        {
            label1.Text = "Nhập: " + inputText;
            string digits = inputText.Substring(inputText.Length - 4).TrimStart('0');
            foreach (char digit in digits)
            {
                synthesizer.Speak(digit.ToString());
            }
        }
    }
}
