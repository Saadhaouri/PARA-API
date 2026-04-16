using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BetyParaAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private const string ServerName = @".\SQLEXPRESS";
        private const string DatabaseName = "Para";
        private const string BackupFileName = "Para.bak";

        private string GetDesktopExportFolder()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string exportFolder = Path.Combine(desktopPath, "Exported Data");

            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }

            return exportFolder;
        }

        private string GetDesktopBackupPath()
        {
            return Path.Combine(GetDesktopExportFolder(), BackupFileName);
        }

        private string GetSqlServerBackupFolder()
        {
            string basePath = @"C:\Program Files\Microsoft SQL Server";

            if (!Directory.Exists(basePath))
            {
                throw new DirectoryNotFoundException("SQL Server base directory not found.");
            }

            var backupFolders = Directory
                .GetDirectories(basePath, "Backup", SearchOption.AllDirectories)
                .Where(path => path.Contains(@"\MSSQL\Backup") || path.EndsWith(@"\Backup"))
                .ToList();

            if (backupFolders.Count > 0)
            {
                return backupFolders[0];
            }

            throw new DirectoryNotFoundException("SQL Server Backup folder not found.");
        }

        private string GetSqlBackupPath()
        {
            return Path.Combine(GetSqlServerBackupFolder(), BackupFileName);
        }

        private (int ExitCode, string Output, string Error) RunSqlCmd(string query)
        {
            using Process process = new Process();

            process.StartInfo.FileName = "sqlcmd";
            process.StartInfo.Arguments = $"-S \"{ServerName}\" -E -C -Q \"{query}\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            return (process.ExitCode, output, error);
        }

        private bool HasSqlError(string output, string error)
        {
            string allText = $"{output}\n{error}".ToLower();

            return allText.Contains("msg ")
                || allText.Contains("error")
                || allText.Contains("access is denied")
                || allText.Contains("terminating abnormally")
                || allText.Contains("cannot open backup device");
        }

        [HttpGet("backup")]
        public IActionResult BackupDatabase()
        {
            try
            {
                string sqlBackupPath = GetSqlBackupPath();
                string desktopBackupPath = GetDesktopBackupPath();

                if (System.IO.File.Exists(sqlBackupPath))
                {
                    System.IO.File.Delete(sqlBackupPath);
                }

                if (System.IO.File.Exists(desktopBackupPath))
                {
                    System.IO.File.Delete(desktopBackupPath);
                }

                string query =
                    $"BACKUP DATABASE [{DatabaseName}] TO DISK = N'{sqlBackupPath}' " +
                    $"WITH FORMAT, INIT, NAME = N'{DatabaseName}-Backup', STATS = 10";

                var result = RunSqlCmd(query);

                if (result.ExitCode != 0 || HasSqlError(result.Output, result.Error))
                {
                    return StatusCode(500, new
                    {
                        message = "Backup failed.",
                        sqlBackupPath,
                        desktopBackupPath,
                        error = result.Error,
                        output = result.Output
                    });
                }

                if (!System.IO.File.Exists(sqlBackupPath))
                {
                    return StatusCode(500, new
                    {
                        message = "Backup command finished but SQL backup file was not created.",
                        sqlBackupPath,
                        output = result.Output,
                        error = result.Error
                    });
                }

                System.IO.File.Copy(sqlBackupPath, desktopBackupPath, true);

                return Ok(new
                {
                    message = "Backup completed successfully.",
                    path = desktopBackupPath,
                    sqlBackupPath,
                    output = result.Output
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server error.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("import")]
        public IActionResult ImportDatabase()
        {
            try
            {
                string desktopBackupPath = GetDesktopBackupPath();
                string sqlBackupPath = GetSqlBackupPath();

                if (!System.IO.File.Exists(desktopBackupPath))
                {
                    return NotFound(new
                    {
                        message = "Backup file not found on Desktop.",
                        path = desktopBackupPath
                    });
                }

                System.IO.File.Copy(desktopBackupPath, sqlBackupPath, true);

                string query =
                    $"RESTORE DATABASE [{DatabaseName}] FROM DISK = N'{sqlBackupPath}' " +
                    $"WITH REPLACE";

                var result = RunSqlCmd(query);

                if (result.ExitCode != 0 || HasSqlError(result.Output, result.Error))
                {
                    return StatusCode(500, new
                    {
                        message = "Restore failed.",
                        sqlBackupPath,
                        desktopBackupPath,
                        error = result.Error,
                        output = result.Output
                    });
                }

                return Ok(new
                {
                    message = "Database restored successfully.",
                    path = desktopBackupPath,
                    sqlBackupPath,
                    output = result.Output
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Server error.",
                    error = ex.Message
                });
            }
        }
    }
}