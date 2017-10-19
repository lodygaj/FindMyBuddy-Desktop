using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;

namespace FindMyBuddy
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private AmazonDynamoDBClient client; // DynamoDB client
        private String userTbl = "users"; // Table name

        public LoginPage()
        {
            InitializeComponent();

            // Initialize DynamoDB client
            client = new AmazonDynamoDBClient();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            // Get username and password from text box
            String username = usernameBox.Text;
            String password = passwordBox.Password.ToString();

            Document document; // Document to be returned from DyanmoDB
            String passwordRetrieved = ""; // Password to be returned from Document

            // Verify that information is entered in text boxes
            if (!username.Equals("") && !password.Equals(""))
            {
                try
                {
                    Table users = Table.LoadTable(client, userTbl); // Get DynamoDB table
                    GetItemOperationConfig config = new GetItemOperationConfig // Define GetItem operation
                    {
                        AttributesToGet = new List<string> { "username", "password" },
                        ConsistentRead = true
                    };

                    document = users.GetItem(username, config); // Get item from table
                    passwordRetrieved = document["password"]; // Get password attribute variable from item
                }
                catch (AmazonDynamoDBException e1) { Console.WriteLine(e1.Message); }
                catch (AmazonServiceException e2) { Console.WriteLine(e2.Message); }
                catch (Exception e3) { Console.WriteLine(e3.Message); }

                // Verify credentials from database
                if (passwordRetrieved.Equals(password))
                {
                    // Navigate to Menu page
                    NavigationService.Navigate(new MenuPage());
                    Console.WriteLine("Correct username or password!");
                }
                else
                {
                    // Incorrect username of password!
                    Console.WriteLine("Incorrect username or password!");
                    messageBlock.Text = "Incorrect username or password!";
                }
            }
            else
            {
                // Must enter a username and password!
                Console.WriteLine("Must enter a username and password!");
                messageBlock.Text = "Must enter a username and password!";
            }
        }

        private void registerBtn_Click_1(object sender, RoutedEventArgs e)
        {
            // Navigate to Register page
            this.NavigationService.Navigate(new RegisterPage());
        }
    }
}
