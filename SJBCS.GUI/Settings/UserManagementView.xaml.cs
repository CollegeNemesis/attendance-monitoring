using SJBCS.Data;
using SJBCS.GUI.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace SJBCS.GUI.Settings
{
    /// <summary>
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
//string Connection_string = @"Server=W7PHSSPNCFX3N72\SQLEXPRESS;Database=AMS;Trusted_Connection=True;";
        string lastSelectedUser = string.Empty;
        DataGridRow dgRow;
        DataGridCell dgCell;

        const string appName = "EntityFramework";
        const string providerName = "System.Data.SqlClient";

        string metaData = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Metadata;
        string dataSource = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Hostname;
        string initialCatalog = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.InitialCatalog;
        string userId = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Username;
        string password = ConnectionHelper.Config.AppConfiguration.Settings.DataSource.Password;

        SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

        public UserManagementView()
        {
            try
            {
                InitializeComponent();
                LoadUsers();

                sqlBuilder.DataSource = dataSource;
                sqlBuilder.InitialCatalog = initialCatalog;
                sqlBuilder.MultipleActiveResultSets = true;
                sqlBuilder.IntegratedSecurity = false;
                sqlBuilder.UserID = userId;
                sqlBuilder.Password = password;
                sqlBuilder.ApplicationName = appName;

                BindUserType();

                EnableUserDetails(false);
                UserDetailsVisibility(Visibility.Hidden);

                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
            catch (Exception error)
            {
                LogError(error.Message);
                Logger.Error(error);
            }
        }

        private async void LogError(string error)
        {
            var result = await DialogHelper.ShowDialog(DialogType.Error, error);
        }

        private void BindUserType()
        {
            cboUserType.Items.Add("Admin");
            cboUserType.Items.Add("User");
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("PRC_ViewUsers", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable("Users");
                    sda.Fill(dt);
                    dgResults.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception error)
            {
                LogError("Error in loading users.");
                Logger.Error(error);
            }

        }

        private object ExtractBoundValue(DataGridRow row, DataGridCell cell)
        {
            DataGridBoundColumn col = cell.Column as DataGridBoundColumn;
            Binding binding = col.Binding as Binding;
            string boundPropertyName = "Username";

            object data = row.Item;
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(data);

            PropertyDescriptor property = properties[boundPropertyName];

            return property.GetValue(data);
        }

        private void DataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UserDetailsVisibility(Visibility.Hidden);

                DependencyObject dep = (DependencyObject)e.OriginalSource;

                // iteratively traverse the visual tree
                while ((dep != null) &&
                !(dep is DataGridCell) &&
                !(dep is DataGridColumnHeader))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                if (dep == null)
                    return;

                if (dep is DataGridColumnHeader)
                {
                    DataGridColumnHeader columnHeader = dep as DataGridColumnHeader;
                    // do something
                }

                if (dep is DataGridCell)
                {
                    DataGridCell cell = dep as DataGridCell;

                    // navigate further up the tree
                    while ((dep != null) && !(dep is DataGridRow))
                    {
                        dep = VisualTreeHelper.GetParent(dep);
                    }

                    DataGridRow row = dep as DataGridRow;

                    dgRow = row;
                    dgCell = cell;

                    btnUpdate.IsEnabled = true;
                    btnDelete.IsEnabled = true;

                }
            }
            catch (Exception error)
            {
                LogError("Error in retrieving user details.");
                Logger.Error(error);
            }
        }

        private void PopulateFields(object userName)
        {
            User[] allRecords = null;
            SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString);

            using (var command = new SqlCommand("PRC_SelectUser [" + userName + "]", con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<User>();
                    while (reader.Read())
                        list.Add(new User { UserId = reader.GetGuid(0), Username = reader.GetString(1), Password = reader.GetString(2), UserType = reader.GetString(3) });
                    allRecords = list.ToArray();
                }
                con.Close();
            }

            txtUsername.Text = allRecords[0].Username;
            txtPassword.Password = DecryptText(allRecords[0].Password);
            txtPassword2.Text = txtPassword.Password;
            cboUserType.SelectedValue = allRecords[0].UserType;

            lastSelectedUser = userName.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearFields();
                EnableUserDetails(true);
                UserDetailsVisibility(Visibility.Visible);
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;

                btnSaveEdit.Visibility = Visibility.Hidden;
            }
            catch (Exception error)
            {
                LogError("Error in loading add new user fields.");
                Logger.Error(error);
            }
        }

        private void ClearFields()
        {
            cboUserType.SelectedIndex = 0;
            txtUsername.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtPassword2.Text = string.Empty;
            txtUsername.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PopulateFields(ExtractBoundValue(dgRow, dgCell));
                EnableUserDetails(true);
                UserDetailsVisibility(Visibility.Visible);
                btnSaveAdd.Visibility = Visibility.Hidden;

            }
            catch (Exception error)
            {
                LogError(error.Message);
                Logger.Error(error);
            }

        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await DialogHelper.ShowDialog(DialogType.Validation, "Are you sure you want to delete selected " + lastSelectedUser + "?");

                if ((bool)result)
                {
                    DeleteUser();
                    LoadUsers();
                    lastSelectedUser = string.Empty;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    UserDetailsVisibility(Visibility.Hidden);
                    await DialogHelper.ShowDialog(DialogType.Informational, "User deleted.");
                }
            }
            catch (Exception error)
            {
                LogError("Error in deleting user.");
                Logger.Error(error);

            }
        }

        private void DeleteUser()
        {
            SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString);

            using (SqlCommand cmd = new SqlCommand("PRC_DeleteUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = lastSelectedUser;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

        }

        private void EnableUserDetails(bool isEnabled)
        {
            cboUserType.IsEnabled = isEnabled;
            txtUsername.IsEnabled = isEnabled;
            txtPassword.IsEnabled = isEnabled;
            btnSaveAdd.IsEnabled = isEnabled;
            btnSaveEdit.IsEnabled = isEnabled;
        }

        private void ValidateInput()
        {
            CheckInvalidCharacters(txtUsername.Text.Trim());
            CheckInvalidCharacters(txtPassword.Password.Trim());

            if (txtUsername.Text.Trim().Length < 8)
            {
                throw new Exception("Username must be atleast 8 letters.");
            }

            if (txtUsername.Text.Contains(" "))
            {
                throw new Exception("Username must not contain spaces.");
            }

            if (txtPassword.Password.Trim().Length < 8)
            {
                throw new Exception("Password must be atleast 8 letters.");
            }

            if (!Regex.IsMatch(txtPassword.Password.Trim(), @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,100})$"))
            {
                throw new Exception("Password must contain letters and numbers.");
            }
        }

        private void CheckInvalidCharacters(string text)
        {
            bool bad = false;
            string strInput = text;

            List<string> MyList = new List<string>()
                        { "'", ",", "--", "/* ... */", "xp_", "\"", "\\'", "\\\"", "+", "!", "<", ">", ";", "*", "~","#","$","%","^","&","(",")","-","=",":","?","`"};

            foreach (var item in MyList)
            {
                if (strInput.ToLower().Contains(item))
                {
                    bad = true;
                    break;
                }
            }
            if (bad)
            {
                throw new Exception("Some field contains invalid characters.");
            }
        }

        private void UserDetailsVisibility(Visibility vs)
        {
            lblUsername.Visibility = vs;
            lblPassword.Visibility = vs;
            lblUserType.Visibility = vs;
            cboUserType.Visibility = vs;
            txtUsername.Visibility = vs;
            txtPassword.Visibility = vs;
            txtPassword2.Visibility = vs;
            btnSaveAdd.Visibility = vs;
            btnSaveEdit.Visibility = vs;
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await DialogHelper.ShowDialog(DialogType.Validation, "Are you sure you want to add new user?");

                if ((bool)result)
                {
                    ValidateInput();
                    CheckIfUserExists();

                    InsertUser();
                    LoadUsers();
                    UserDetailsVisibility(Visibility.Hidden);
                    await DialogHelper.ShowDialog(DialogType.Validation, "User added.");
                }
            }
            catch (Exception error)
            {
                LogError(error.Message);
                Logger.Error(error);
            }
        }

        private void InsertUser()
        {
            SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString);

            using (SqlCommand cmd = new SqlCommand("PRC_InsertUser", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserGuid", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = txtUsername.Text.Trim();
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = EncryptText(txtPassword.Password.Trim());
                cmd.Parameters.Add("@UserType", SqlDbType.VarChar).Value = cboUserType.SelectedValue.ToString();

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void CheckIfUserExists()
        {
            SqlConnection con = new SqlConnection(sqlBuilder.ConnectionString);

            using (var command = new SqlCommand("PRC_SelectUser [" + txtUsername.Text.Trim() + "]", con))
            {
                con.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read() && !reader.GetString(1).Equals(lastSelectedUser))
                        throw new Exception("Username is already used.");
                }
                con.Close();
            }
        }

        private async void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = await DialogHelper.ShowDialog(DialogType.Validation, "Are you sure you want to add update user?");

                if ((bool)result)
                {
                    ValidateInput();
                    CheckIfUserExists();

                    DeleteUser();
                    InsertUser();
                    LoadUsers();
                    lastSelectedUser = string.Empty;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    UserDetailsVisibility(Visibility.Hidden);
                    await DialogHelper.ShowDialog(DialogType.Validation, "User details updated.");
                }
            }
            catch (Exception error)
            {
                LogError(error.Message);
                Logger.Error(error);
            }
        }

        public string EncryptText(string strText)
        {
            return Encrypt(strText, "&%#@?,:*");
        }

        public string DecryptText(string strText)
        {
            return Decrypt(strText, "&%#@?,:*");
        }

        private string Encrypt(string strText, string strEncrKey)
        {
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(StringExtensions.Left(strEncrKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception error)
            {
                LogError(error.Message);
                Logger.Error(error);
                return error.Message;
                
            }
        }

        private string Decrypt(string strText, string sDecrKey)
        {
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray;
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(StringExtensions.Left(sDecrKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

        private void ImgShowHide_MouseLeave(object sender, MouseEventArgs e)
        {
            HidePassword();
        }

        private void ShowPassword()
        {
            //ImgShowHide.Source = new BitmapImage(new Uri(imgLoc + "Hide.jpg"));
            txtPassword2.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Hidden;
            txtPassword2.Text = txtPassword.Password;
        }

        private void HidePassword()
        {
            //ImgShowHide.Source = new BitmapImage(new Uri(imgLoc + "Show.jpg"));
            txtPassword2.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Visible;
            txtPassword.Focus();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password.Length > 0)
            {
                ImgShowHide.Visibility = Visibility.Visible;
            }
            else
            {
                ImgShowHide.Visibility = Visibility.Hidden;
            }
        }
    }

    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }
    }
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
}
