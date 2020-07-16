using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace MySql.Powershell
{
    public abstract class QueryBase : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string ServerInstance { get; set; } = string.Empty;

        [Parameter]
        public uint Port { get; set; } = 3306;

        [Parameter]
        public string Database { get; set; }

        [Parameter(Mandatory = true)]
        public string Username { get; set; } = string.Empty;

        [Parameter(Mandatory = true)]
        public string Password { get; set; } = string.Empty;

        [Parameter(Mandatory = true)]
        public string Query { get; set; } = string.Empty;

        [Parameter]
        public int QueryTimeout { get; set; } = 0;

        [Parameter]
        public bool AllowUserVariables { get; set; } = false;

        [Parameter]
        public SwitchParameter Ssl { get; set; }

        [Parameter(Mandatory = false)]
        public string SslCa { get; set; }

        [Parameter(Mandatory = false)]
        public string SslCert { get; set; }

        [Parameter(Mandatory = false)]
        public string SslKey { get; set; }

        protected string connectionString { get; private set; }

        protected override void BeginProcessing()
        {
            WriteVerbose("Building Connection");
            var builder = new MySqlConnectionStringBuilder
            {
                Server = ServerInstance,
                Port = Port,
                Database = Database,
                UserID = Username,
                Password = Password,
                AllowUserVariables = AllowUserVariables
            };

            if (Ssl.IsPresent)
            {
                builder.SslMode = MySqlSslMode.Required;
                builder.SslCa = SslCa;
                builder.SslCert = SslCert;
                builder.SslKey = SslKey;
            }

            connectionString = builder.ConnectionString;
        }

        public void ProcessInternal()
        {
            BeginProcessing();
            ProcessRecord();
        }
    }
}
