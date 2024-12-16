using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _045_Sorita_Telan_F2
{
    public partial class frmPirates : Form
    {
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\User\\Desktop\\045_Sorita_Telan_F2\\dpPirates1.accdb";
        OleDbConnection conn;
        bool NewRecord;
        bool clicked = false;
        public frmPirates()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            string query = "SELECT piratename AS ALIAS, givenname AS NAME, " +
                           "pirategroup AS PIRATEGROUP, bounty AS BOUNTY " +
                           "FROM pirates " +
                           "WHERE (piratename LIKE @keyword OR givenname LIKE @keyword) " +
                           "AND pirategroup = @group";


            OleDbConnection conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@keyword", "%" + txtKeyword.Text + "%");
            cmd.Parameters.AddWithValue("@group", cboPirateGroup.Text);

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(dt);

            conn.Close();
            grdView.DataSource = dt;

        }

        private void frmPirates_Load(object sender, EventArgs e)
        {
            Reload();
            SelectDistinct();

            DataTable dt = new DataTable();
            string query = "Select ID as ID, piratename as ALIAS, givenname as NAME, age as AGE, bounty as BOUNTY, pirategroup as PIRATEGROUP from pirates";

            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;

            grdView.Columns["age"].Visible = false;
            grdView.Columns["id"].Visible = false;
        }
        public void TextBoxEnable()
        {
            txtAge.Enabled = true;
            txtAlias.Enabled = true;
            txtBounty.Enabled = true;
            cboPirateInfoGroup.Enabled = true;
            txtBounty.Enabled = true;
            txtName.Enabled = true;
            btnNewRecord.Enabled = true;
        }
        public void TextBoxDisable()
        {
            txtAlias.Enabled = false;
            txtAge.Enabled = false;
            txtBounty.Enabled = false;
            cboPirateInfoGroup.Enabled = false;
            txtBounty.Enabled = false;
            txtName.Enabled = false;
            btnSave.Enabled = true;
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            NewRecord = false;

            btnSave.Enabled = true;
            btnNewRecord.Enabled = false;
            txtAlias.Enabled = true;
            txtName.Enabled = true;
            txtAge.Enabled = true;
            cboPirateInfoGroup.Enabled = true;
            txtBounty.Enabled = true;
            DataTable dt = new DataTable();
            string query = "Select piratename as ALIAS, givenname as NAME, age as AGE, pirategroup as PIRATEGROUP, bounty as BOUNTY from pirates where piratename = '" + txtAlias.Text + "'";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();

            grdView.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "Delete from pirates where piratename = @alias";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show("Deleted");

            txtAlias.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            cboPirateInfoGroup.Text = "";
            txtBounty.Text = "";

            Reload();
        }

        private void grdView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAlias.Text = grdView.SelectedCells[1].Value.ToString();
            txtName.Text = grdView.SelectedCells[2].Value.ToString();
            txtAge.Text = grdView.SelectedCells[3].Value.ToString();
            txtBounty.Text = grdView.SelectedCells[4].Value.ToString();
            cboPirateGroup.Text = grdView.SelectedCells[5].Value.ToString();

        }
        private void Reload()
        {
            DataTable dt = new DataTable();
            string query = "SELECT piratename AS ALIAS, givenname AS NAME, " +
                           "pirategroup AS PIRATEGROUP, bounty AS BOUNTY FROM pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();


            grdView.DataSource = dt;



        }
        private void SelectDistinct()
        {
            DataTable dt = new DataTable();
            string query = "select distinct pirategroup from pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            adapter.Fill(dt);
            conn.Close();


            cboPirateGroup.DataSource = dt;
            cboPirateGroup.DisplayMember = "pirategroup";
        }

        private void btnNewRecord_Click(object sender, EventArgs e)
        {
            NewRecord = true;
            txtAge.Text = " ";
            txtName.Text = " ";
            txtAlias.Text = " ";
            txtBounty.Text = " ";
            cboPirateInfoGroup.Text = " ";
            btnSave.Enabled = true;

            TextBoxEnable();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtAlias.Text = "";
            txtName.Text = "";
            txtAge.Text = "";
            cboPirateInfoGroup.Text = "";
            txtBounty.Text = "";

            TextBoxDisable();
            btnSave.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (NewRecord == true)
            {
                string query = "INSERT into [pirates] (piratename, givenname, age, bounty, pirategroup) values(@alias, @name, @age, @bounty, @pirategroup)";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@bounty", txtBounty.Text);
                cmd.Parameters.AddWithValue("@pirategroup", cboPirateInfoGroup.Text);
                cmd.ExecuteNonQuery();
                conn.Close();

                MessageBox.Show("Added");

                Reload();
            }

            else
            {

                string query = "Update [pirates] set piratename = @alias, givenname = @name, age = @age, bounty = @bounty, pirategroup = @pirategroup where ID = @id";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@alias", txtAlias.Text);
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@bounty", txtBounty.Text);
                cmd.Parameters.AddWithValue("@pirategroup", cboPirateInfoGroup.Text);
                cmd.Parameters.AddWithValue("@id", grdView.SelectedCells[0].Value.ToString());
                cmd.ExecuteNonQuery();
                conn.Close();

                if (clicked)
                {
                    MessageBox.Show("Updated");
                }




                Reload();
            }
        }
    }
}
