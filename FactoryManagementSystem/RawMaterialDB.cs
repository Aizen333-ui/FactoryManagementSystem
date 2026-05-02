using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FactoryManagementSystem
{
    public static class RawMaterialDb
    {
        private static string connStr = "@";


       
        public static bool RemoveQuantity(string materialName, int quantity, string v)
        {
            if (string.IsNullOrEmpty(materialName))
            {
                MessageBox.Show("Material name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (quantity <= 0)
            {
                MessageBox.Show("Quantity must be greater than 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    // 1) Check current quantity in the main RawMaterials table
                    string checkQuery = "SELECT Quantity FROM RawMaterials WHERE MaterialName = @name";
                    int currentQty = 0;

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, con))
                    {
                        checkCmd.Parameters.AddWithValue("@name", materialName);
                        var result = checkCmd.ExecuteScalar();
                        if (result != null)
                            currentQty = Convert.ToInt32(result);
                        else
                        {
                            MessageBox.Show($"{materialName} does not exist in the inventory.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return false;
                        }
                    }

                    // 2) Subtract or delete
                    if (currentQty <= quantity)
                    {
                        string deleteQuery = "DELETE FROM RawMaterials WHERE MaterialName = @name";
                        using (SqlCommand delCmd = new SqlCommand(deleteQuery, con))
                        {
                            delCmd.Parameters.AddWithValue("@name", materialName);
                            delCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string updateQuery = "UPDATE RawMaterials SET Quantity = Quantity - @qty WHERE MaterialName = @name";
                        using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                        {
                            updateCmd.Parameters.AddWithValue("@qty", quantity);
                            updateCmd.Parameters.AddWithValue("@name", materialName);
                            updateCmd.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating material: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }


}
