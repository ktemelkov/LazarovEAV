using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using LazarovEAV.Model.Config;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO.Compression;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    class DriveInfoComparer : IEqualityComparer<DriveInfo>
    {
        bool IEqualityComparer<DriveInfo>.Equals(DriveInfo x, DriveInfo y)
        {
            return x.RootDirectory.FullName == y.RootDirectory.FullName;
        }

        int IEqualityComparer<DriveInfo>.GetHashCode(DriveInfo obj)
        {
            return obj.GetHashCode();
        }
    };


    /// <summary>
    /// 
    /// </summary>
    class BackupInfo
    {
        public string DisplayName { get; set; }
        public string Filename { get; set; }
        public string Hash { get; set; }
        public DateTime CreateDate { get; set; }
    };


    /// <summary>
    /// 
    /// </summary>
    class BackupViewModel: DependencyObject, IDisposable
    {
        const string BACKUP_DIR_NAME = "LazarovEAV_Backups";

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty UsbDrivesProperty =
                        DependencyProperty.Register("UsbDrives", typeof(ObservableCollection<DriveInfo>), typeof(BackupViewModel),
                        new PropertyMetadata(new ObservableCollection<DriveInfo>()));

        internal ObservableCollection<DriveInfo> UsbDrives { get { return (ObservableCollection<DriveInfo>)GetValue(UsbDrivesProperty); } set { SetValue(UsbDrivesProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedDriveProperty =
                        DependencyProperty.Register("SelectedDrive", typeof(DriveInfo), typeof(BackupViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((BackupViewModel)o).onSelectedDriveChanged((DriveInfo)arg.OldValue, (DriveInfo)arg.NewValue); }));

        internal DriveInfo SelectedDrive { get { return (DriveInfo)GetValue(SelectedDriveProperty); } set { SetValue(SelectedDriveProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty DriveFreeSpaceProperty =
                        DependencyProperty.Register("DriveFreeSpace", typeof(string), typeof(BackupViewModel),
                        new PropertyMetadata(""));

        internal string DriveFreeSpace { get { return (string)GetValue(DriveFreeSpaceProperty); } set { SetValue(DriveFreeSpaceProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ProgressMessageProperty =
                        DependencyProperty.Register("ProgressMessage", typeof(string), typeof(BackupViewModel),
                        new PropertyMetadata(""));

        internal string ProgressMessage { get { return (string)GetValue(ProgressMessageProperty); } set { SetValue(ProgressMessageProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ErrorMessageProperty =
                        DependencyProperty.Register("ErrorMessage", typeof(string), typeof(BackupViewModel),
                        new PropertyMetadata(""));

        internal string ErrorMessage { get { return (string)GetValue(ErrorMessageProperty); } set { SetValue(ErrorMessageProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty BackupsListProperty =
                        DependencyProperty.Register("BackupsList", typeof(ObservableCollection<BackupInfo>), typeof(BackupViewModel),
                        new PropertyMetadata(new ObservableCollection<BackupInfo>()));

        internal ObservableCollection<BackupInfo> BackupsList { get { return (ObservableCollection<BackupInfo>)GetValue(BackupsListProperty); } set { SetValue(BackupsListProperty, value); } }

        
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SelectedBackupProperty =
                        DependencyProperty.Register("SelectedBackup", typeof(BackupInfo), typeof(BackupViewModel),
                        new PropertyMetadata(null, (o, arg) => { ((BackupViewModel)o).onSelectedBackupChanged((BackupInfo)arg.OldValue, (BackupInfo)arg.NewValue); }));


        internal BackupInfo SelectedBackup { get { return (BackupInfo)GetValue(SelectedBackupProperty); } set { SetValue(SelectedBackupProperty, value); } }

        public static readonly DependencyProperty DBHashProperty =
                        DependencyProperty.Register("DBHash", typeof(string), typeof(BackupViewModel),
                        new PropertyMetadata("", (o, arg) => { ((BackupViewModel)o).onDBHashChanged((string)arg.OldValue, (string)arg.NewValue); }));

        internal string DBHash { get { return (string)GetValue(DBHashProperty); } set { SetValue(DBHashProperty, value); } }

        private DispatcherTimer drivesPollingTimer;


        public static readonly DependencyProperty BackupEnabledProperty =
                        DependencyProperty.Register("BackupEnabled", typeof(bool), typeof(BackupViewModel),
                        new PropertyMetadata(false));
        internal bool BackupEnabled { get { return (bool)GetValue(BackupEnabledProperty); } set { SetValue(BackupEnabledProperty, value); } }

        public static readonly DependencyProperty RestoreEnabledProperty =
                        DependencyProperty.Register("RestoreEnabled", typeof(bool), typeof(BackupViewModel),
                        new PropertyMetadata(false));
        internal bool RestoreEnabled { get { return (bool)GetValue(RestoreEnabledProperty); } set { SetValue(RestoreEnabledProperty, value); } }


        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartBackupCommandProperty =
                        DependencyProperty.Register("StartBackupCommand", typeof(ICommand), typeof(BackupViewModel),
                        new PropertyMetadata(null));

        internal ICommand StartBackupCommand { get { return (ICommand)GetValue(StartBackupCommandProperty); } set { SetValue(StartBackupCommandProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty StartRestoreCommandProperty =
                        DependencyProperty.Register("StartRestoreCommand", typeof(ICommand), typeof(BackupViewModel),
                        new PropertyMetadata(null));

        internal ICommand StartRestoreCommand { get { return (ICommand)GetValue(StartRestoreCommandProperty); } set { SetValue(StartRestoreCommandProperty, value); } }

        private Task<bool> pendingTask;


        /// <summary>
        /// 
        /// </summary>
        public BackupViewModel()
        {
            this.StartBackupCommand = new CommandDelegate(new Action<object>(startBackup));
            this.StartRestoreCommand = new CommandDelegate(new Action<object>(startRestore));

            Task<string>.Factory
                .StartNew(() =>
                {
                    return CalculateMD5(ModelConfig.DATABASE_PATH);
                })
                .ContinueWith(t =>
                {
                    this.DBHash = t.Result;
                }, TaskScheduler.FromCurrentSynchronizationContext());


            pollUsbDrives(null, null);
            (this.drivesPollingTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.ApplicationIdle, pollUsbDrives, this.Dispatcher)).Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void startRestore(object obj)
        {
            if (this.pendingTask != null)
            {
                return;
            }

            this.ErrorMessage = "";
            this.ProgressMessage = "Възстановяване от архив ...";
            var hash = this.SelectedBackup.Hash;
            var filename = this.SelectedBackup.Filename;
            var error = "";

            int progress = 0;

            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.ApplicationIdle, (o, e) => {
                string[] dots = { "   ", ".  ", ".. ", "..." };
                this.ProgressMessage = $"Възстановяване от архив { dots[progress] }";

                progress = (progress + 1) % 4;
            }, this.Dispatcher);

            timer.Start();

            this.pendingTask = Task<bool>.Factory
                .StartNew(() =>
                {
                    string backupFile = $"{ModelConfig.DATABASE_PATH}.backup.zip";

                    try
                    {
                        if (File.Exists(backupFile))
                        {
                            File.Delete(backupFile);
                        }

                        using (ZipArchive archive = ZipFile.Open(backupFile, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(ModelConfig.DATABASE_PATH, "application.db3", CompressionLevel.Optimal);
                        }

                        using (ZipArchive archive = ZipFile.OpenRead(filename))
                        {
                            archive.GetEntry("application.db3").ExtractToFile(ModelConfig.DATABASE_PATH, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (File.Exists(backupFile))
                            {
                                using (ZipArchive archive = ZipFile.OpenRead(backupFile))
                                {
                                    archive.GetEntry("application.db3").ExtractToFile(ModelConfig.DATABASE_PATH, true);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }

                        error = $"Грешка при възстановяване на данните от архива! {ex.Message}";
                        return false;
                    }

                    return true;
                });

            this.pendingTask.ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        this.DBHash = hash;
                    }

                    timer.Stop();
                    this.ErrorMessage = error;
                    this.pendingTask = null;
                    this.ProgressMessage = "";
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void startBackup(object obj)
        {
            if (this.pendingTask != null)
            {
                return;
            }

            this.ErrorMessage = "";
            this.ProgressMessage = "Архивиране ...";

            var ts = DateTime.Now;
            var dirName = Path.Combine(this.SelectedDrive.RootDirectory.FullName, BACKUP_DIR_NAME);
            var filename = Path.Combine(dirName, $"{ts.Year}-{ts.Month:D2}-{ts.Day:D2}_{ts.Hour:D2}-{ts.Minute:D2}-{ts.Second:D2}_{Environment.MachineName}_{this.DBHash}.zip");
            var error = "";

            int progress = 0;

            var timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.ApplicationIdle, (o, e) => {
                string[] dots = { "   ", ".  ", ".. ", "..." }; 
                this.ProgressMessage = $"Архивиране { dots[progress] }";

                progress = (progress + 1) % 4;
            }, this.Dispatcher);

            timer.Start();

            this.pendingTask = Task<bool>.Factory
                .StartNew(() =>
                {
                    try
                    {
                        if (!Directory.Exists(dirName))
                        {
                            Directory.CreateDirectory(dirName);
                        }

                        using (ZipArchive archive = ZipFile.Open(filename, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(ModelConfig.DATABASE_PATH, "application.db3", CompressionLevel.Optimal);
                        }
                    } 
                    catch (Exception ex)
                    {
                        error = $"Грешка при архивиране на данните! {ex.Message}";
                        return false;
                    }

                    return true;
                });

            this.pendingTask.ContinueWith(t =>
                {
                    if (t.Result)
                    {
                        var drive = this.SelectedDrive;
                        this.SelectedDrive = null;
                        this.SelectedDrive = drive;
                    }

                    timer.Stop();
                    this.pendingTask = null;
                    this.ErrorMessage = error;
                    this.ProgressMessage = "";
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            this.drivesPollingTimer.Stop();

            if (this.pendingTask != null)
            {
                this.pendingTask.Wait();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void onSelectedDriveChanged(DriveInfo oldValue, DriveInfo newValue)
        {
            try
            {
                this.DriveFreeSpace = newValue != null ? this.ToBytesCount(newValue.AvailableFreeSpace) : "";
            }
            catch(Exception)
            {
                this.DriveFreeSpace = "";
                newValue = null;
            }

            this.BackupsList.Clear();

            if (newValue != null)
            {
                this.BackupsList = this.getListOfBackups(newValue.RootDirectory.FullName);
                this.BackupEnabled = this.DBHash != "" && this.BackupsList.FirstOrDefault(bi => bi.Hash == this.DBHash) == null;
            } 
            else
            {
                this.BackupEnabled = false;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void onSelectedBackupChanged(BackupInfo oldValue, BackupInfo newValue)
        {
            if (newValue != null)
            {
                this.RestoreEnabled = this.DBHash != "" && newValue.Hash != this.DBHash;
            }
            else
            {
                this.RestoreEnabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private void onDBHashChanged(string oldValue, string newValue)
        {
            this.BackupEnabled = this.DBHash != "" && this.UsbDrives.Count > 0 && this.BackupsList.FirstOrDefault(bi => bi.Hash == this.DBHash) == null;
            this.RestoreEnabled = this.DBHash != "" && this.SelectedBackup != null && this.SelectedBackup.Hash != this.DBHash;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private ObservableCollection<BackupInfo> getListOfBackups(string fullName)
        {
            var result = new ObservableCollection<BackupInfo>();

            Regex bkpRegex = new Regex(@"\\(\d\d\d\d)-(\d\d)-(\d\d)_(\d\d)-(\d\d)-(\d\d)_([a-zA-Z0-9-]+)_([A-Fa-f0-9]{32})\.[zZ][iI][pP]$");
            var dirName = Path.Combine(fullName, BACKUP_DIR_NAME);

            var backups = Directory.Exists(dirName) ? Directory
                    .EnumerateFiles(dirName, "*.zip")
                    .Where(s => bkpRegex.IsMatch(s))
                    .OrderByDescending(s => s)
                    .Select(s => {
                        var match = bkpRegex.Match(s);
                        string year = match.Groups[1].Value;
                        string month = match.Groups[2].Value;
                        string day = match.Groups[3].Value;
                        string hour = match.Groups[4].Value;
                        string minute = match.Groups[5].Value;
                        string second = match.Groups[6].Value;
                        string machine = match.Groups[7].Value;
                        string hash = match.Groups[8].Value;

                        return new BackupInfo() {
                            Filename = s,
                            DisplayName = $"{day}.{month}.{year} - {hour}:{minute}:{second} ({machine})",
                            Hash = hash,
                            CreateDate = new DateTime(Int32.Parse(year), Int32.Parse(month), Int32.Parse(day), Int32.Parse(hour), Int32.Parse(minute), Int32.Parse(second))
                        };
                    }) : new List<BackupInfo>();

            foreach (var entry in backups)
            {
                result.Add(entry);
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string ToBytesCount(long bytes)
        {
            int unit = 1024;
            string unitStr = "B";

            if (bytes < unit)
            {
                return string.Format("{0} {1}", bytes, unitStr);
            }
            else
            {
                unitStr = unitStr.ToUpper();
            }

            int exp = (int)(Math.Log(bytes) / Math.Log(unit));

            return string.Format("{0:##.##} {1}{2}", bytes / Math.Pow(unit, exp), "KMGTPEZY"[exp - 1], unitStr);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 16 * 1024 * 1024))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pollUsbDrives(object sender, EventArgs e)
        {
            var drives = from driveInfo in DriveInfo.GetDrives()
                         where driveInfo.DriveType == DriveType.Removable && driveInfo.IsReady
                         select driveInfo;

            int count = this.UsbDrives.Count;

            foreach (var drive in drives)
            {
                if (!this.UsbDrives.Contains(drive, new DriveInfoComparer()))
                {
                    this.UsbDrives.Add(drive);
                }
            }

            if (count != this.UsbDrives.Count)
            {
                this.UsbDrives = new ObservableCollection<DriveInfo>(this.UsbDrives.OrderBy(d => d.RootDirectory.FullName));
            }

            List<DriveInfo> toRemove = new List<DriveInfo>();

            foreach (var drive in this.UsbDrives)
            {
                if (!drives.Contains(drive, new DriveInfoComparer()))
                {
                    toRemove.Add(drive);
                }
            }

            foreach (var drive in toRemove)
            {
                this.UsbDrives.Remove(drive);
            }

            if (this.UsbDrives.Count <= 0)
            {
                this.SelectedDrive = null;
            }
            else
            {
                if (this.SelectedDrive == null || !this.UsbDrives.Contains(this.SelectedDrive, new DriveInfoComparer()))
                {
                    this.SelectedDrive = this.UsbDrives[0];
                }
            }
        }
    }
}
