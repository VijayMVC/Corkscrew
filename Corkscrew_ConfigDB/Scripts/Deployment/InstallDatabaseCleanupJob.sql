
/* Create the ConfigDB_DatabaseCleanupJob job */
USE [msdb];
GO

DECLARE @ReturnCode		INT					= 0,
		@jobId			uniqueidentifier	= NULL, 
		@ScheduleUID	uniqueidentifier	= NEWID()

SELECT @jobId = job_id FROM msdb.dbo.sysjobs WHERE (name = N'ConfigDB_DatabaseCleanupJob')
IF (@jobId IS NOT NULL)
	GOTO EndSave


BEGIN TRANSACTION InstallDatabaseCleanupJob

IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[CorkscrewCMS Database Jobs]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[CorkscrewCMS Database Jobs]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END


EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'ConfigDB_DatabaseCleanupJob', 
		@enabled=1, 
		@notify_level_eventlog=2, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'This is a Aquarius Corkscrew CMS Job.
----------------------------------------------------------------------------
(c) Copyright 2014, Aquarius Operating Systems, India. All Rights Reserved.
----------------------------------------------------------------------------

This SQL Job cleans up orphan Corkscrew CMS SiteDB databases. 


Job last modified: Dec 31, 2014.
', 
		@category_name=N'[CorkscrewCMS Database Jobs]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback


EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Execute Cleanup', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC [CleanupSites];', 
		@database_name=N'$(DatabaseName)', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'ConfigDB_DatabaseCleanupJob_Schedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=8, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20141217, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=@ScheduleUID
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback


COMMIT TRANSACTION InstallDatabaseCleanupJob

	GOTO EndSave

QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION InstallDatabaseCleanupJob

EndSave:

GO