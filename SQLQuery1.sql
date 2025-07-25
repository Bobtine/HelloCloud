BACKUP DATABASE TestProduitsDB
TO DISK = 'C:\Backups\TestProduitsDB.bak'
WITH FORMAT,
     MEDIANAME = 'TestProduitsBackup',
     NAME = 'Full Backup of TestProduitsDB';
