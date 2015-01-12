using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomerQueueAppUsingDatabase
{
    public partial class CustomerServiceUI : Form
    {
        public CustomerServiceUI()
        {
            InitializeComponent();
        }

        Queue<Customer> customers = new Queue<Customer>();
        string connectionstring = "Data Source=SHIFU; Database=CustomerWaitingQueueDB; Integrated Security=true";
        //Customer aCustomer = new Customer();
        private void enqueueButton_Click(object sender, EventArgs e)
        {
            SaveComplain();
            Enqueue();
            ShowComplain();

              }

        private void ShowComplain()
        {
            //string connectionstring = "Data Source=SHIFU; Database=CustomerWaitingQueueDB; Integrated Security=true";
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            string query = "SELECT * FROM t_CustomerQueue";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader aDataReader = command.ExecuteReader();
            List<Customer> customers = new List<Customer>();
            while (aDataReader.Read())
            {
                Customer aCustomer = new Customer();
                aCustomer.serialNo = Convert.ToInt32(aDataReader["Id"]);
                aCustomer.name = aDataReader["name"].ToString();
                aCustomer.complain = aDataReader["complain"].ToString();
                aCustomer.status = aDataReader["progress_status"].ToString();
                customers.Add(aCustomer);
            }

            connection.Close();
            waitingQueueListView.Items.Clear();
            foreach (Customer aCustomer in customers)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Text = aCustomer.serialNo.ToString();
                listViewItem.SubItems.Add(aCustomer.name);
                listViewItem.SubItems.Add(aCustomer.complain);
                listViewItem.SubItems.Add(aCustomer.status);
                //listViewItem.Tag = aCustomer;
                waitingQueueListView.Items.Add(listViewItem);
            }
        }

        public void SaveComplain()
        {
           
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            string query = "insert into t_CustomerQueue values ('" + nametextBox.Text + "','" + complaintextBox.Text + "','Not_seved')";
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
            connection.Close(); 
        }

        private void Enqueue()
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            string query = "SELECT * FROM t_CustomerQueue";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader aDataReader = command.ExecuteReader();
           
            while (aDataReader.Read())
            {
                Customer aCustomer = new Customer();
                aCustomer.serialNo = Convert.ToInt32(aDataReader["Id"]);
                aCustomer.name = aDataReader["name"].ToString();
                aCustomer.complain = aDataReader["complain"].ToString();
                aCustomer.status = aDataReader["progress_status"].ToString();
                customers.Enqueue(aCustomer);
            }
connection.Close();
        }

        private void CustomerServiceUI_Load(object sender, EventArgs e)
        {
            ShowComplain();
            //PerfomDequeue();

        }

        private void dequeueButton_Click(object sender, EventArgs e)
        {
            PerfomDequeue();

        }

        private void PerfomDequeue()
        {
            Customer aCustomer = new Customer();
            aCustomer = customers.Dequeue();
            sntextBox.Text = aCustomer.serialNo.ToString();
            cntextBox.Text = aCustomer.name;
            cmltextBox.Text = aCustomer.complain;
            waitingQueueListView.Items.RemoveAt(0);
            //SqlConnection connection = new SqlConnection(connectionstring);
            //connection.Open();
            //string query = "DELETE TOP(1) FROM t_CustomerQueue";
            //SqlCommand command = new SqlCommand(query, connection);
            //command.ExecuteNonQuery();
            //connection.Close();
             
        }

      
    }
}
