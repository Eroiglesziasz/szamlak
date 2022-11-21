using MySql.Data.MySqlClient;
using System.Data;

namespace szamlak1121
{
    public partial class Form1 : Form
    {
        MySqlConnection conn = null;
        MySqlCommand cmd = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void szamlak_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var xd = (szamla)szamla_listbox.SelectedItem;
            if (xd is null) return;
            textBox1.Text = xd.Id.ToString();
            textBox2.Text = xd.Nev;
            numericUpDown1.Value = xd.Egyenleg;
            dateTimePicker1.Value = xd.Datum;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = "localhost";
            builder.UserID = "root";
            builder.Password = "";
            builder.Database = "szamlak";
            conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                conn.Open();
                cmd = conn.CreateCommand();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "A program le�ll!!");
                Environment.Exit(0);
            }
            finally
            {
                conn.Close();



            }
            gyumolcs_update();
        }
        private void gyumolcs_update()
        {
            szamla_listbox.Items.Clear();
            cmd.CommandText = "SELECT * FROM `szamlak`;";
            conn.Open();
            using (MySqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    szamla update = new szamla(dr.GetInt32("id"), dr.GetString("tulajdonosNeve"), dr.GetInt32("egyenleg"), dr.GetDateTime("nyitasdatum"));
                    szamla_listbox.Items.Add(update);
                }
            }
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (szamla_listbox.SelectedIndex < 0)
            {
                return;
            }
            cmd.CommandText = "DELETE FROM `szamlak` WHERE `id` = @id;";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@id", textBox1.Text);
            conn.Open();

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("T�rl�s sikeres volt!", "Sikeres!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                conn.Close();
                gyumolcs_update();
            }
            else
            {
                MessageBox.Show("Az adatok t�rl�se sikertelen volt!");
            }
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "INSERT INTO `szamlak` (`tulajdonosNeve`, `egyenleg`, `nyitasdatum`) VALUES (@tulajdonosNeve, @egyenleg, @nyitasdatum)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@tulajdonosNeve", textBox2.Text);
            cmd.Parameters.AddWithValue("@egyenleg", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@nyitasdatum", dateTimePicker1.Value);
            conn.Open();
            try
            {

                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Sikeresen r�gz�tve");
                }
                else
                {
                    MessageBox.Show("Sikertelen r�gz�t�s!");
                }

            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message, "Sikertelen!");

            }
            conn.Close();
            gyumolcs_update();
        }
    }
}