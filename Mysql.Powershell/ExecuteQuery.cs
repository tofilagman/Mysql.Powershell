using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Management.Automation;

namespace MySql.Powershell
{
    [Cmdlet("Invoke", "Mysqlcmd")]
    public class ExecuteQuery : QueryBase
    {

        [Parameter]
        public SwitchParameter Void { get; set; }

        [Parameter]
        public SwitchParameter Scalar { get; set; }

        protected override void ProcessRecord()
        {
            var npg = new MySqlConnection(connectionString);
            try
            {
                WriteVerbose("Opening Connection");
                npg.Open();
                var cmd = new MySqlCommand(Query, npg);

                WriteVerbose("Executing Query");
                cmd.CommandTimeout = QueryTimeout;
                cmd.CommandType = System.Data.CommandType.Text;

                if (Void.IsPresent)
                {
                    cmd.ExecuteNonQuery();
                }
                else if (Scalar.IsPresent)
                {
                    WriteObject(cmd.ExecuteScalar());
                }
                else
                {
                    using (var adp = new MySqlDataAdapter(cmd))
                    {
                        using(var dt = new DataTable())
                        {
                            adp.Fill(dt);
                            WriteObject(dt, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                npg?.Dispose();
            }
        }
    }
}
