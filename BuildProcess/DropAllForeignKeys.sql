------------
--TODO: set the name of your database owner here.
Declare @DbOwner varchar(30)
select @DbOwner = 'dbo'

----START SCRIPT
select 'Start: There are ' + cast(count(*) as varchar(10)) + ' foreign keys.' 
from dbo.sysobjects where OBJECTPROPERTY(id, N'IsForeignKey') = 1

DECLARE cKeys CURSOR FOR
  select [name], object_name(parent_object_id) from sys.objects
  where type = 'F'

BEGIN TRY

  OPEN cKeys
  declare @FKName varchar(50)
  declare @TableName varchar(50)
  declare @sqlcmd nvarchar(200)

  FETCH NEXT FROM cKeys
  into @FKName, @TableName

  WHILE (@@FETCH_STATUS = 0)
  BEGIN
    set @sqlcmd = 'alter table ' + @DbOwner + '.[' + @TableName + '] drop constraint ' + @FKName
    exec sp_executesql @sqlcmd
    --select @sqlcmd

    FETCH NEXT FROM cKeys
    into @FKName, @TableName
  END

  --Close Cursor
  CLOSE cKeys
  DEALLOCATE cKeys

END TRY
BEGIN CATCH
  --Always close cursor
  CLOSE cKeys
  DEALLOCATE cKeys
END CATCH

--CLOSING RESULT
declare @intKeyCount int
select @intKeyCount = count(*)
from dbo.sysobjects where OBJECTPROPERTY(id, N'IsForeignKey') = 1

if (@intKeyCount > 0)
  raiserror 50000 'Not all foreign keys were deleted.'

select 'Finish: There are now ' + cast(@intKeyCount as varchar(10)) + ' foreign keys.'
GO
