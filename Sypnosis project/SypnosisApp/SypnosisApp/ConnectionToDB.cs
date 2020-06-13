using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using SypnosisApp.Stores_classes;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using ZXing;
using System.Net.Mail;
using System.Drawing.Imaging;

namespace SypnosisApp
{
    class ConnectionToDB
    {
        public MySqlConnection connection;
        DataTable dt;
        MySqlDataAdapter da;
        public ConnectionToDB()
        {
            String connectionInfo = "server=studmysql01.fhict.local;" +
                                    "database=dbi377952;" +
                                    "user id=dbi377952;" +
                                    "password=1234;" +
                                    "connect timeout=2;" +
                                    "SslMode = none";

            connection = new MySqlConnection(connectionInfo);
        }

        public async Task<bool> Login(string emailAddress, string password)
        {
            bool result = false;
            string queryString = "SELECT password FROM account WHERE email = @email;";
            MySqlCommand command = new MySqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@email", emailAddress);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result = BCrypt.Net.BCrypt.Verify(password, reader["password"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        public async Task<Vendor> GetVendor(string emailAddress, List<Store> storeList)
        {
            Vendor vendor = null;
            string queryString = "SELECT a.id account_id, a.first_name, a.last_name, a.email, e.id employee_id, e.role_id, r.name role_name, v.store_id " +
                "FROM account a " +
                "JOIN employee e ON (a.id = e.account_id) " +
                "JOIN role r ON (e.role_id = r.id) " +
                "JOIN vendor v ON(e.id = v.employee_id) " +
                "WHERE a.email = @email;";
            MySqlCommand command = new MySqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@email", emailAddress);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Role role = new Role(
                            Convert.ToInt32(reader["role_id"]),
                            Convert.ToString(reader["role_name"])
                            );

                        Store store = storeList.Find(x => x.StoreId == Convert.ToInt32(reader["store_id"]));

                        vendor = new Vendor(
                            store,
                            Convert.ToInt32(reader["employee_id"]),
                            role,
                            Convert.ToInt32(reader["account_id"]),
                            Convert.ToString(reader["first_name"]),
                            Convert.ToString(reader["last_name"]),
                            Convert.ToString(reader["email"])
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return vendor;
        }

        public bool VisitorCheckIn(int QRvalue)
        {
            int id = 0;
            string sql = "SELECT `id` FROM `ticket` WHERE id='" + QRvalue + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    id = Convert.ToInt32(reader["id"]);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public int GetCampspotIdFromRfid(string rfidValue)
        {
            int id = 0;
            int ticketID = GetTicketIdFromRfid(rfidValue);
            string sql = "SELECT `campspot_id` FROM `camper` WHERE ticket_id='" + ticketID + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["campspot_id"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        public int GetCampspotIdFromTicketId(int ticketId)
        {
            int id = 0;
            string sql = "SELECT `campspot_id` FROM `camper` WHERE ticket_id='" + ticketId + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["campspot_id"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        public int GetVisitorIdFromRfid(string rfidValue)
        {
            int id = 0;
            string sql = "SELECT `account_id` FROM `ticket` WHERE rfid_code='" + rfidValue + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["account_id"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        public int GetVisitorIdFromEmail(string email)
        {
            int id = 0;
            string sql = "SELECT `id` FROM `account` WHERE email='" + email + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["id"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }
        public int GetVisitorIdFromTicketId(int ticketId)
        {
            int id = 0;
            string sql = "SELECT `account_id` FROM `ticket` WHERE id='" + ticketId + "';";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["account_id"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return id;
        }

        public string GetVisitorEmailFromRfid(string rfidValue)
        {
            string email = null;
            string sql = "SELECT email FROM account WHERE id=(SELECT account_id FROM ticket WHERE rfid_code='" + rfidValue + "');";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    email = reader["email"].ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return email;
        }

        public string GetVisitorName(int QRvalue)
        {
            string firstName = "";
            string lastName = "";
            string sql = "SELECT `first_name`,`last_name` FROM `account` WHERE account.id=(SELECT `account_id` FROM ticket WHERE ticket.id ='" + QRvalue + "');";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    firstName = reader["first_name"].ToString();
                    lastName = reader["last_name"].ToString();
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
            return firstName + " " + lastName;
        }
        public string GetVisitorNameRfid(string rfidValue)
        {
            string firstName = "";
            string lastName = "";
            string sql = "SELECT `first_name`,`last_name` FROM `account` WHERE account.id=(SELECT `account_id` FROM ticket WHERE ticket.rfid_code ='" + rfidValue + "');";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    firstName = reader["first_name"].ToString();
                    lastName = reader["last_name"].ToString();
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
            return firstName + " " + lastName;

        }

        public int GetTicketIdFromVisitorId(int visitorId)
        {
            int ticketId = 0;
            string sql = "SELECT `id` FROM `ticket` WHERE account_id='" + visitorId + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ticketId = (Int32)reader[0];
                }
            }
            catch
            {
                
            }
            finally
            {
                connection.Close();
            }
            return ticketId;
        }

        public void SendEmail(string email, int ticketId)
        {
            try
            {
                SmtpClient mailServer = new SmtpClient("smtp.gmail.com", 587);
                mailServer.EnableSsl = true;

                mailServer.Credentials = new System.Net.NetworkCredential("OnFestEmail", "realPassword");

                string from = "OnFestEmail";
                string to = email;
                MailMessage msg = new MailMessage(from, to);
                msg.Subject = "OnFest Ticket";
                msg.Body = "Hello," +
                    "You have bought a ticket for our event - OnFest. " +
                    "This is the attachment that contains the QR code that you have to show when entering the event. " +
                    "After that you will be given a bracelet with which you will be able to buy food and drinks inside the event. " +
                    "OnFest team";
                msg.Attachments.Add(new Attachment(@"C:\Users\user\Desktop\Prop\ProP_Group_31\SypnosisApp\SypnosisApp\QRCodes\Visitor" + ticketId + "QR.Png"));
                mailServer.Send(msg);
            }
            catch (Exception ex)
            {
                
            }
        }

        public string VisitorHasRfidAssigned(int ticketId)
        {
            string rfidValue = null;
            string sql = "SELECT `rfid_code` FROM `ticket` WHERE id='" + ticketId + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    rfidValue = reader[0].ToString();
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return rfidValue;
        }

        public int GetTicketIdFromRfid(string rfidValue)
        {
            int ticketId = 0;
            string sql = "SELECT `id` FROM `ticket` WHERE rfid_code='" + rfidValue + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ticketId = (Int32)reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return ticketId;
        }

        public int GetAccountIdFromRfid(string rfidValue)
        {
            int accountId = 0;
            string sql = "SELECT a.id FROM account a JOIN ticket t ON (a.id = t.account_id) WHERE rfid_code='" + rfidValue + "'";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    accountId = (Int32)reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return accountId;
        }

        public int GetTicketIdFromEmail(string email)
        {
            int ticketId = 0;
            string sql = "SELECT `id` FROM `ticket` WHERE account_id= (SELECT `id` FROM `account` WHERE email= '" + email + "');";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ticketId = (Int32)reader[0];
                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
            return ticketId;
        }

        public bool IsCamper(int ticketId)
        {
            int value = 0;
            string sql = "SELECT ticket_id FROM `camper` WHERE ticket_id='" + ticketId + "';";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    value = Convert.ToInt32(reader[0]);
                }
                if (value != ticketId || value == 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool IsRegistered(string email)
        {
            int count = 0;
            string sql = "SELECT COUNT(*) FROM `account` WHERE email='" + email + "';";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                    if (count > 0)
                    {
                        break;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool IsEmployee(int accountId)
        {
            int value = 0;
            string sql = "SELECT account_id FROM employee WHERE account_id='" + accountId + "';";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    value = Convert.ToInt32(reader[0]);
                }
                if (value != accountId || value == 0)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public int GetFreeCampspot()
        {
            int freeCampId = 0;
            string getFreeCampspotSql = "SELECT `id` FROM campspot WHERE reserved_by_id is null;";
            MySqlCommand getFreeCampspotCommand = new MySqlCommand(getFreeCampspotSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = getFreeCampspotCommand.ExecuteReader();
                while (reader.Read())
                {
                    freeCampId = (Int32)reader[0];
                }
            }
            catch
            {
                
            }
            finally
            {
                connection.Close();
            }
            return freeCampId;

        }

        public bool UpdateCampspot(int ticketId, int freeSpot)
        {
            String updateSql = "UPDATE campspot SET reserved_by_id='" + ticketId + "' WHERE id= '" + freeSpot + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool AddCamper(int campSpotId, int ticketId)
        {
            String addCamperSql = "INSERT INTO camper (campspot_id, ticket_id)" +
                "VALUES ('" + campSpotId + "', '" + ticketId + "');";
            MySqlCommand addCamperCommand = new MySqlCommand(addCamperSql, connection);
            try
            {
                addCamperCommand.Connection.Open();
                addCamperCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public double GetBalanceByRfid(string tag)
        {
            double visitorBalance = 0;
            string balanceSql = "SELECT `balance` FROM `account` WHERE account.id = (SELECT account_id FROM ticket WHERE rfid_code='" + tag + "')";
            MySqlCommand balanceCommand = new MySqlCommand(balanceSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = balanceCommand.ExecuteReader();
                while (reader.Read())
                {
                    visitorBalance = Convert.ToDouble(reader["balance"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return visitorBalance;
        }

        public double GetBalanceByEmail(string email)
        {
            double visitorBalance = 0;
            string balanceSql = "SELECT `balance` FROM `account` WHERE email = '" + email + "';";
            MySqlCommand balanceCommand = new MySqlCommand(balanceSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = balanceCommand.ExecuteReader();
                while (reader.Read())
                {
                    visitorBalance = Convert.ToDouble(reader["balance"]);
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                connection.Close();
            }
            return visitorBalance;
        }

        public bool SetRFID(string rfidTag, int id)
        {
            String updateSql = "UPDATE ticket SET rfid_code='" + rfidTag + "' WHERE id= '" + id + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        

        public bool SetTicketStatusCamp(string rfidTag)
        {
            String updateSql = "UPDATE ticket SET ticket_status_id='3' WHERE rfid_code= '" + rfidTag + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public bool SetTicketStatusFestivalQr(int QrValue)
        {
            String updateSql = "UPDATE ticket SET ticket_status_id='2' WHERE id= '" + QrValue + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public bool SetTicketStatusFestivalRfid(string rfidTag)
        {
            String updateSql = "UPDATE ticket SET ticket_status_id='2' WHERE rfid_code= '" + rfidTag + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public bool SetTicketStatusOutside(string rfidTag)
        {
            String updateSql = "UPDATE ticket SET ticket_status_id='1' WHERE rfid_code= '" + rfidTag + "' ;";
            MySqlCommand updateCommand = new MySqlCommand(updateSql, connection);
            try
            {
                updateCommand.Connection.Open();
                updateCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        public bool IsAlreadyCheckedCamp(string tag)
        {
            int ticketStatusId = 0;
            bool alreadyCheckedCamp = false;
            string isAlreadyCheckedCampSql = "SELECT `ticket_status_id` FROM `ticket` WHERE rfid_code= '" + tag + "';";
            MySqlCommand isAlreadyCheckedCampCommand = new MySqlCommand(isAlreadyCheckedCampSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = isAlreadyCheckedCampCommand.ExecuteReader();
                while (reader.Read())
                {
                    ticketStatusId = Convert.ToInt32(reader["ticket_status_id"]);
                }
                if (ticketStatusId == 3 || ticketStatusId == 1)
                    alreadyCheckedCamp = true;
                else
                    alreadyCheckedCamp = false;
            }
            catch
            {
                //error
            }
            finally
            {
                connection.Close();
            }
            return alreadyCheckedCamp;
        }
        public bool IsAlreadyCheckedFestival(string tag)
        {
            int ticketStatusId = 0;
            bool alreadyCheckedFestival = false;
            string isAlreadyCheckedCampSql = "SELECT `ticket_status_id` FROM `ticket` WHERE rfid_code= '" + tag + "';";
            MySqlCommand isAlreadyCheckedCampCommand = new MySqlCommand(isAlreadyCheckedCampSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = isAlreadyCheckedCampCommand.ExecuteReader();
                while (reader.Read())
                {
                    ticketStatusId = Convert.ToInt32(reader["ticket_status_id"]);
                }
                if (ticketStatusId == 2 || ticketStatusId == 3)
                    alreadyCheckedFestival = true;
                else
                    alreadyCheckedFestival = false;
            }
            catch
            {
                //error
            }
            finally
            {
                connection.Close();
            }
            return alreadyCheckedFestival;
        }
        public bool IsAlreadyCheckedFestivalQR(int qrValue)
        {
            int ticketStatusId = 0;
            bool alreadyCheckedFestival = false;
            string isAlreadyCheckedCampSql = "SELECT `ticket_status_id` FROM `ticket` WHERE id= '" + qrValue + "';";
            MySqlCommand isAlreadyCheckedCampCommand = new MySqlCommand(isAlreadyCheckedCampSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = isAlreadyCheckedCampCommand.ExecuteReader();
                while (reader.Read())
                {
                    ticketStatusId = Convert.ToInt32(reader["ticket_status_id"]);
                }
                if (ticketStatusId == 2 || ticketStatusId == 3)
                    alreadyCheckedFestival = true;
                else
                    alreadyCheckedFestival = false;
            }
            catch
            {
                //error
            }
            finally
            {
                connection.Close();
            }
            return alreadyCheckedFestival;
        }

        public int GetTicketStatus(string tag)
        {
            int ticketStatusId = 0;
            string ticketStatusSql = "SELECT `ticket_status_id` FROM `ticket` WHERE rfid_code= '" + tag + "';";
            MySqlCommand ticketStatusCommand = new MySqlCommand(ticketStatusSql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = ticketStatusCommand.ExecuteReader();
                while (reader.Read())
                {
                    ticketStatusId = Convert.ToInt32(reader["ticket_status_id"]);
                }
            }
            catch
            {
                //error
            }
            finally
            {
                connection.Close();
            }
            return ticketStatusId;

        }

        public bool RegisterVisitor(string firstName, string lastName, string email, string password)
        {
            String registerSql = "INSERT INTO account (first_name, last_name, email, password)" +
                "VALUES ('" + firstName + "', '" + lastName + "', '" + email + "', '" + password + "');";
            MySqlCommand registerCommand = new MySqlCommand(registerSql, connection);
            try
            {
                registerCommand.Connection.Open();
                registerCommand.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;

        }

        public bool BuyTicket(int accountId, string email, double cost)
        {
            double balance = GetBalanceByEmail(email);

            if (balance - cost >= 0)
            {
                String buyTicketSql = "INSERT INTO ticket (account_id)" +
                                    "VALUES ('" + accountId + "');";
                MySqlCommand buyTicketCommand = new MySqlCommand(buyTicketSql, connection);

                string updateSql = "UPDATE account SET balance = balance - @cost WHERE email = @email;";
                MySqlCommand command = new MySqlCommand(updateSql, connection);
                command.Parameters.AddWithValue("@cost", cost);
                command.Parameters.AddWithValue("@email", email);
                try
                {
                    buyTicketCommand.Connection.Open();
                    buyTicketCommand.ExecuteNonQuery();
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                return false;
            }
            return true;

        }


        public async Task<List<Store>> GetShops()
        {
            List<Store> storeList = new List<Store>();
            string queryString = "SELECT s.id, s.store_type_id, s.name, st.name store_type_name " +
                "FROM store s " +
                "JOIN store_type st ON (s.store_type_id = st.id);";
            MySqlCommand mySqlCommand = new MySqlCommand(queryString, connection);
            try
            {
                mySqlCommand.Connection.Open();
                MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();
                if (mySqlReader.HasRows)
                {
                    while (mySqlReader.Read())
                    {
                        Store store = new Store(
                            Convert.ToInt32(mySqlReader["id"]),
                            Convert.ToString(mySqlReader["name"]),
                            Convert.ToInt32(mySqlReader["store_type_id"]),
                            Convert.ToString(mySqlReader["store_type_name"])
                            );
                        storeList.Add(store);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return storeList;
        }

        public async Task<List<StoreSaleItem>> GetStoreSaleItems(Store store, MetroWindow window)
        {
            ProgressDialogController controller = await window.ShowProgressAsync("Please wait...", "Fetching items!");
            controller.Maximum = 100;
            controller.Minimum = 0;
            int progress = 0;

            List<StoreSaleItem> storeItemList = new List<StoreSaleItem>();
            string queryString = "SELECT ssi.quantity ssi_quantity, ssi.price, ssi.item_id, i.item_type_id, it.name item_type_name, i.name item_name, i.quantity item_quantity, i.picture " +
                "FROM store_sale_item ssi " +
                "JOIN item i ON (ssi.item_id = i.id) " +
                "JOIN item_type it ON (i.item_type_id = it.id)" +
                "WHERE ssi.store_id = " + store.StoreId;
            MySqlCommand mySqlCommand = new MySqlCommand(queryString, connection);
            await Task.Run(() =>
            {
                try
                {
                    mySqlCommand.Connection.Open();
                    MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();
                    
                    if (mySqlReader.HasRows)
                    {
                        while (mySqlReader.Read())
                        {
                            StoreSaleItem storeItem = new StoreSaleItem(
                                Convert.ToInt32(mySqlReader["ssi_quantity"]),
                                Convert.ToDouble(mySqlReader["price"]),
                                Convert.ToInt32(mySqlReader["item_id"]),
                                Convert.ToString(mySqlReader["item_name"]),
                                Convert.ToInt32(mySqlReader["item_quantity"]),
                                (byte[])mySqlReader["picture"],
                                Convert.ToInt32(mySqlReader["item_type_id"]),
                                Convert.ToString(mySqlReader["item_type_name"])
                                );
                            storeItemList.Add(storeItem);
                            progress++;
                            controller.SetProgress(progress);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    connection.Close();
                }
            });
            await controller.CloseAsync();

            return storeItemList;
        }

        public async Task<List<StoreRentItem>> GetStoreRentItems(Store store, MetroWindow window)
        {
            ProgressDialogController controller = await window.ShowProgressAsync("Please wait...", "Fetching items!");
            controller.Maximum = 100;
            controller.Minimum = 0;
            int progress = 0;

            List<StoreRentItem> storeItemList = new List<StoreRentItem>();
            string queryString = "SELECT sri.quantity sri_quantity, sri.price, sri.item_id, i.item_type_id, it.name item_type_name, i.name item_name, i.quantity item_quantity, i.picture " +
                "FROM store_rent_item sri " +
                "JOIN item i ON (sri.item_id = i.id) " +
                "JOIN item_type it ON (i.item_type_id = it.id)" +
                "WHERE sri.store_id = " + store.StoreId;
            MySqlCommand mySqlCommand = new MySqlCommand(queryString, connection);
            await Task.Run(() =>
            {
                try
                {
                    mySqlCommand.Connection.Open();
                    MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();

                    if (mySqlReader.HasRows)
                    {
                        while (mySqlReader.Read())
                        {
                            StoreRentItem storeItem = new StoreRentItem(
                                Convert.ToInt32(mySqlReader["sri_quantity"]),
                                Convert.ToInt32(mySqlReader["price"]),
                                Convert.ToInt32(mySqlReader["item_id"]),
                                Convert.ToString(mySqlReader["item_name"]),
                                Convert.ToInt32(mySqlReader["item_quantity"]),
                                (byte[])mySqlReader["picture"],
                                Convert.ToInt32(mySqlReader["item_type_id"]),
                                Convert.ToString(mySqlReader["item_type_name"])
                                );
                            storeItemList.Add(storeItem);
                            progress++;
                            controller.SetProgress(progress);
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    connection.Close();
                }
            });
            await controller.CloseAsync();

            return storeItemList;
        }

        public byte[] getVisitorPicture(int accId)
        {
            byte[] pic = null;
            string query = "SELECT `picture` FROM `account` WHERE id ='" + accId + "';";
            MySqlCommand mySqlCommand = new MySqlCommand(query, connection);

            try
            {
                mySqlCommand.Connection.Open();
                MySqlDataReader mySqlReader = mySqlCommand.ExecuteReader();

                if (mySqlReader.HasRows)
                {
                    while (mySqlReader.Read())
                    {
                        pic = (byte[])mySqlReader["picture"];
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return pic;
        }

        public async void DepositBalance(int accountId, double depositAmount)
        {
            string updateSql = "UPDATE account SET balance = balance + @depositAmount WHERE id = @accountId;";
            MySqlCommand command = new MySqlCommand(updateSql, connection);
            command.Parameters.AddWithValue("@depositAmount", depositAmount);
            command.Parameters.AddWithValue("@accountId", accountId);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        //public void BuyTicketFromApp(string email, double cost)
        //{
        //    string updateSql = "UPDATE account SET balance = balance - @cost WHERE email = @email;";
        //    MySqlCommand command = new MySqlCommand(updateSql, connection);
        //    command.Parameters.AddWithValue("@cost", cost);
        //    command.Parameters.AddWithValue("@email", email);
        //    try
        //    {
        //        command.Connection.Open();
        //        command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        public void ShopPayRFID(double totalPrice, string rfid)
        {
            if (rfid != "")
            {
                string paySql = "UPDATE account a JOIN ticket t ON (a.id = t.account_id) SET a.balance = a.balance - @totalPrice WHERE t.rfid_code = @rfid";
                MySqlCommand command = new MySqlCommand(paySql, connection);
                command.Parameters.AddWithValue("@totalPrice", totalPrice);
                command.Parameters.AddWithValue("@rfid", rfid);
                try
                {
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void InsertSaleItems(int storeId, int accountId, ObservableCollection<StoreSaleItem> purchaseList)
        {
            var groupedPurchaseList = purchaseList.GroupBy(x => x.ItemId)
                .Select(g => g.ToList())
                .ToList();

            string queryStringItems = "";

            foreach (List<StoreSaleItem> item in groupedPurchaseList)
            {
                queryStringItems += "(" + storeId + ", " + item.First().ItemId + ", " + accountId + ", '" + 
                    DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + "', " + item.Count() + ", " + 
                    Convert.ToDouble(item.First().StoreSaleItemPrice).ToString(new CultureInfo("en-US")) + "), ";
            }

            queryStringItems = queryStringItems.Substring(0, queryStringItems.Length - 2);

            string queryString = "INSERT INTO sale (store_id, item_id, account_id, date_time, quantity, price) VALUES " + queryStringItems;

            MySqlCommand command = new MySqlCommand(queryString, connection);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void InsertRentItems(int storeId, int accountId, ObservableCollection<StoreRentItem> rentalList)
        {
            //var groupedPurchaseList = rentalList.GroupBy(x => x.ItemId)
            //    .Select(g => g.ToList())
            //    .ToList();

            double totalPrice = 0;
            string queryStringItems = "";

            foreach (StoreRentItem item in rentalList)
            {
                queryStringItems += "(" + storeId + ", " + item.ItemId + ", " + accountId + ", '" +

                    DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + "', " + "NULL" + ", " + Convert.ToDouble(item.StoreRentItemPrice).ToString(new CultureInfo("en-US")) + "), ";

                totalPrice += Convert.ToDouble(item.StoreRentItemPrice);

            }

            queryStringItems = queryStringItems.Substring(0, queryStringItems.Length - 2);

            string queryString = "INSERT INTO rented (store_id, item_id, account_id, date_time, return_date_time, price) " +
                "VALUES " + queryStringItems;

            MySqlCommand command = new MySqlCommand(queryString, connection);

            string updateBalanceSql = "UPDATE account SET balance = balance - " + totalPrice.ToString(new CultureInfo("en-US")) + " WHERE id =" + accountId + ";";
            MySqlCommand balanceCommand = new MySqlCommand(updateBalanceSql, connection);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
                balanceCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public ObservableCollection<StoreRentItem> ShowReturnItems(string rfid)
        {
            ObservableCollection<StoreRentItem> returnItemsList = new ObservableCollection<StoreRentItem>();
            string sql = "SELECT r.price, i.id item_id,i.name," +
                "i.quantity item_quantiy,i.picture,i.item_type_id,it.id real_item_type_id " +
                "FROM rented r JOIN ticket t on(r.account_id= t.account_id) " +
                "JOIN item i ON(r.item_id= i.id) " +
                "JOIN item_type it ON(i.item_type_id= it.id) " +
                "WHERE r.return_date_time IS NULL AND t.rfid_code = @rfid_code; ";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@rfid_code", rfid);
            try
            {
                command.Connection.Open();
                MySqlDataReader readerShop = command.ExecuteReader();
                while (readerShop.Read())
                {
                    StoreRentItem sri = new StoreRentItem(0,
                                                          Convert.ToInt32(readerShop["price"]),
                                                          Convert.ToInt32(readerShop["item_id"])
                                                          , Convert.ToString(readerShop["name"]), 
                                                          Convert.ToInt32(readerShop["item_quantiy"]),
                                                          (byte[])readerShop["picture"], 
                                                          Convert.ToInt32(readerShop["item_type_id"]), 
                                                         Convert.ToString(readerShop["real_item_type_id"]));
                    returnItemsList.Add(sri);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return returnItemsList;

        }


        public void ReturnItems(ObservableCollection<StoreRentItem> returnList, string rfid)
        {
            int accountId = GetAccountIdFromRfid(rfid);
            string finalSql = "";
            
            if (accountId != 0)
            {
                foreach (StoreRentItem item in returnList)
                {
                    string sql = "UPDATE rented SET return_date_time = '" + 
                        DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss") + 
                        "' WHERE item_id = " + item.ItemId.ToString() + 
                        " AND account_id = " + accountId.ToString() + 
                        " AND return_date_time IS NULL LIMIT 1;";
                    finalSql += sql;
                }
            }

            MySqlCommand command = new MySqlCommand(finalSql, connection);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        public void GetShopName(ComboBox combo)
        {
            string sqlShop = "SELECT * FROM store WHERE store_type_id=1";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[2]+"("+readerShop[0]+")");
            }
            connection.Close();
        }

        public void GetRental(ComboBox combo)
        {
            string sqlShop = "SELECT * FROM store WHERE store_type_id=2";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[2]+"(" + readerShop[0] + ")");
            }
            connection.Close();
        }
        
       

        public void GetShopEmployees(ComboBox combo)
        {
            string sqlShop = "SELECT s.id,employee_id FROM store s JOIN vendor v ON(s.id=v.store_id) WHERE s.store_type_id=1;";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[1]+"(Store:"+readerShop[0]+")");
            }
            connection.Close();
        }

        public void GetRentalEmployees(ComboBox combo)
        {
            string sqlShop = "SELECT s.id,employee_id FROM store s JOIN vendor v ON(s.id=v.store_id) WHERE s.store_type_id=2;";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[1] + "(Rental:" + readerShop[0] + ")");
            }
            connection.Close();
        }

        public int GetTicketIncome()
        {
            string sql = "SELECT COUNT(*) FROM ticket";
            int nr =0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return  55*nr;
        }
        public int GetCampIncome()
        {
            string sql = "SELECT COUNT(*) FROM campspot";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return 20 * nr+10*nr;
        }

        public double GetShopIncome()
        {
            string sql = "SELECT * FROM sale";
            double nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                  double q= double.Parse(reader["quantity"].ToString());
                  double p = double.Parse(reader["price"].ToString());
                  nr+=q* p;
                
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;

        }


        public double GetRentalIncome()
        {
            string sql = "SELECT COUNT(*) FROM rented where store_id=4";
            double nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr * 1.99;
        }

        public double GetRentalIncome1()
        {
            string sql = "SELECT COUNT(*) FROM rented where store_id=5";
            double nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr * 9.99;
        }
        public double GetTotalIncome()
        {
            return this.GetCampIncome() + GetTicketIncome() + GetShopIncome()+GetRentalIncome()+GetRentalIncome1();
        }
        public int GetNrRegistered()
        {
            string sql = "SELECT COUNT(id) FROM account";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrChecksIn()
        {
            string sql = "SELECT COUNT(id) FROM ticket WHERE ticket_status_id is NOT NULL ";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrTicket()
        {
            string sql = "SELECT COUNT(id) FROM ticket ";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrLeft()
        {
            string sql = "SELECT COUNT(id) FROM ticket WHERE ticket_status_id=1";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrCamp()
        {
            string sql = "SELECT COUNT(id) FROM ticket WHERE ticket_status_id=3";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrTotalShopLoan()
        {
            string sql = "SELECT COUNT(id) FROM store";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrTotalShop()
        {
            string sql = "SELECT COUNT(id) FROM store where store_type_id=1";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrTotalLoan()
        {
            string sql = "SELECT COUNT(id) FROM store where store_type_id=2";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrItems()
        {
            string sql = "SELECT COUNT(id) FROM item";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrItemTypes()
        {
            string sql = "SELECT COUNT(id) FROM item_type";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrEmployees()
        {
            string sql = "SELECT COUNT(id) FROM employee where role_id=2";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrAdmins()
        {
            string sql = "SELECT COUNT(id) FROM employee where role_id=1";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrRentedItems()
        {
            string sql = "SELECT COUNT(item_id) FROM store_rent_item";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }
        public int GetNrSaleItems()
        {
            string sql = "SELECT COUNT(item_id) FROM store_sale_item";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public int GetNrSubscriber()
        {
            string sql = "SELECT COUNT(email) FROM subscriber";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }


        public void AddShops(int storeid, string storename)
        {
            string sql = "INSERT INTO store (store_type_id,name)" +
               "VALUES ('" + storeid + "', '" + storename+"');";
            
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public int emailAlreadyInDB(string email)
        {
            string sql = "SELECT COUNT(*) FROM account WHERE email='" + email + "';";
            int nr = 0;
            MySqlDataReader reader = null;
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    nr = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return nr;
        }

        public void GetSearchInfo(DataGrid datagrid)
        {
            string sql = "SELECT id,first_name,last_name,email,balance FROM account;";
            try
            {
                connection.Open();
                MySqlCommand command1 = new MySqlCommand(sql, connection);
                MySqlCommand cmdSel = new MySqlCommand(sql, connection);
                 dt = new DataTable();
                 da = new MySqlDataAdapter(cmdSel);
                da.Fill(dt);
                datagrid.DataContext = dt;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }

        public bool HavePicture(int id)
        {
            bool s=false;
            string sql = "SELECT picture FROM account WHERE id=;"+id;
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader != null)
                    {
                        s = true;

                    }
                    else
                    {
                        s = false;
                    }
                }
               
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return s;
        }

        public void GetNotAssignEmployee(ComboBox combo)
        {
            
            string sql = "SELECT id FROM employee e Left JOIN vendor v ON(e.id=v.employee_id)" +
                " WHERE e.role_id=2 AND v.store_id IS NULL;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    combo.Items.Add(reader[0]);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void AssignEmployee(int storeid,int empid)
        {

            string sql = "INSERT INTO vendor (store_id,employee_id) " +
                "VALUES ('" + storeid + "', '" + empid + "');";
           
            MySqlCommand command = new MySqlCommand(sql, connection);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void GetShopID(ComboBox combo)
        {
            string sqlShop = "SELECT * FROM store WHERE store_type_id=1";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[0]);
            }
            connection.Close();
        }
        public void GetLoanID(ComboBox combo)
        {
            string sqlShop = "SELECT * FROM store WHERE store_type_id=2";
            MySqlCommand command1 = new MySqlCommand(sqlShop, connection);
            connection.Open();

            MySqlDataReader readerShop = command1.ExecuteReader();
            while (readerShop.Read())
            {
                combo.Items.Add(readerShop[0]);
            }
            connection.Close();
        }

        public void Search(string va,DataGrid datagrid)
        {
            string sql = "SELECT * FROM account WHERE CONCAT(`id`,`first_name`, `last_name`, " +
                "`email`,`balance`) like '%" + va+"%'";
            MySqlCommand command1 = new MySqlCommand(sql, connection);
            da = new MySqlDataAdapter(command1);
            dt = new DataTable();
            da.Fill(dt);
            datagrid.DataContext = dt;
        }



    }
}