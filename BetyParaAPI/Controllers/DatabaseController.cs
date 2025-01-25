using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System;

namespace BetyParaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private string GetBackupPath(string backupFileName)
        {
            string basePath = @"C:\Program Files\Microsoft SQL Server";
            var directories = Directory.GetDirectories(basePath, "MSSQL*");

            if (directories.Length > 0)
            {
                string sqlInstancePath = directories[0];
                return Path.Combine(sqlInstancePath, "MSSQL", "Backup", backupFileName);
            }

            throw new DirectoryNotFoundException("SQL Server instance directory not found.");
        }

        [HttpGet("backup")]
        public IActionResult BackupDatabase()
        {
            var dbName = "Para";
            string backupFileName = "Para.bak";

            try
            {
                string backupPath = GetBackupPath(backupFileName);
                string sqlCommand = $"/C sqlcmd -S DESKTOP-1PPINIT\\SQLEXPRESS -Q \"BACKUP DATABASE [{dbName}] TO DISK='{backupPath}' WITH NOFORMAT, NOINIT, NAME='{dbName}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS=10\"";

                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = sqlCommand;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return Ok("Backup completed successfully.");
                    }
                    else
                    {
                        return StatusCode(500, $"Error during backup: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }

        [HttpGet("import")]
        public IActionResult ImportDatabase()
        {
            var dbName = "Para";
            string backupFileName = "Para.bak";

            try
            {
                string backupPath = GetBackupPath(backupFileName);
                string sqlCommand = $"/C sqlcmd -S DESKTOP-1PPINIT\\SQLEXPRESS -Q \"RESTORE DATABASE [{dbName}] FROM DISK='{backupPath}' WITH REPLACE\"";

                using (Process process = new Process())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = sqlCommand;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        return Ok("Database import completed successfully.");
                    }
                    else
                    {
                        return StatusCode(500, $"Error during database import: {errors}");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }
    }
}
