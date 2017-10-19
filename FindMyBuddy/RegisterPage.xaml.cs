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

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;

namespace FindMyBuddy
{
    /// <summary>
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private AmazonDynamoDBClient client; // DynamoDB client
        private String usersTblName = "users"; // Table name
        private Table usersTbl;

        public RegisterPage()
        {
            InitializeComponent();

            // Initialize DynamoDB client
            client = new AmazonDynamoDBClient();

            // Get users table from DyanamoDB
            try
            {
                usersTbl = Table.LoadTable(client, usersTblName); // Get DynamoDB table
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            // Verify all fields are filled out
            if (!fNameBox.Text.Equals("") && !lNameBox.Text.Equals("") &&
                !emailBox.Text.Equals("") && !usernameBox.Text.Equals("") &&
                !passwordBox.Password.Equals("") && !passwordConfirmBox.Password.Equals(""))
            {
                // Verify that password fields match
                String password = passwordBox.Password.ToString();
                String passwordConfirm = passwordConfirmBox.Password.ToString();

                if (password.Equals(passwordConfirm))
                {
                    // Create user object
                    User user = new User();

                    // Add user values from text boxes
                    user.setFirstName(fNameBox.Text);
                    user.setLastName(lNameBox.Text);
                    user.setEmail(emailBox.Text);
                    user.setUsername(usernameBox.Text);
                    user.setPassword(passwordBox.Password.ToString());

                    // Verify that username is available
                    if (usernameValid(user.getUsername()))
                    {
                        // Add user to DyanamoDB table
                        addUser(user);

                        // Navigate to menu page
                        NavigationService.Navigate(new MenuPage());
                    }
                    else
                    {
                        // Set alert message
                        messageBlock.Text = "Username already taken!";

                        // Clear username field
                        usernameBox.Text = "";
                    }
                }
                else
                {
                    // Set alert message
                    messageBlock.Text = "Passwords do not match!";

                    // Clear password fields
                    passwordBox.Password = "";
                    passwordConfirmBox.Password = "";
                }
            }
            else
            {
                // Set alert message
                messageBlock.Text = "All fields must be filled out!";
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to login page
            NavigationService.Navigate(new LoginPage());
        }

        private Boolean usernameValid(String username)
        {
            // Define GetItem operation
            GetItemOperationConfig config = new GetItemOperationConfig 
            {
                AttributesToGet = new List<string> { "username" },
                ConsistentRead = true
            };

            // Attempt to retrieve user from DynamoDB
            try
            {
                Document doc = usersTbl.GetItem(username, config); // Get item from table

                // Username not taken
                if (doc == null)
                {
                    return true;
                }
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }

            return false;
        }

        private void addUser(User user)
        {
            // Create new user document
            Document newUser = new Document();

            // Add user values to document
            newUser["username"] = user.getUsername();
            newUser["password"] = user.getPassword();
            newUser["firstName"] = user.getFirstName();
            newUser["lastName"] = user.getLastName();
            newUser["email"] = user.getEmail();

            // Add new user to DyanmoDB users table
            try
            {
                usersTbl.PutItem(newUser);
            }
            catch (AmazonDynamoDBException e) { Console.WriteLine(e.Message); }
            catch (AmazonServiceException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
    }
}
