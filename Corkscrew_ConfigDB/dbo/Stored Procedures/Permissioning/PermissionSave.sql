CREATE PROCEDURE [PermissionSave] 
	@PrincipalId	uniqueidentifier,
	@CorkscrewUri	nvarchar(max),
	@Read			bit, 
	@Contribute		bit, 
	@FullControl	bit
AS
BEGIN 

	--- Optimize 
	IF (@FullControl = 1) 
	BEGIN
		SET @Contribute = 0
		SET @Read = 0 
	END
	ELSE IF (@Contribute = 1) 
		BEGIN
			SET @Read = 0 
		END

	IF (@PrincipalId = dbo.GUIDDEFAULT()) 
	BEGIN
		RAISERROR ( N'PrincipalId must be a valid value.', 16, 1 );
		RETURN -1;
	END

	IF ((@Read = 0) AND (@Contribute = 0) AND (@FullControl = 0))
	BEGIN
		DELETE FROM [Permissions] 
			WHERE (([PrincipalId] = @PrincipalId) AND ([CorkscrewUri] = @CorkscrewUri))
		
		RETURN;
	END

	IF (NOT EXISTS (SELECT 1 FROM [Permissions] WITH (NOLOCK) WHERE (([PrincipalId] = @PrincipalId) AND ([CorkscrewUri] = @CorkscrewUri)))) 
	BEGIN
		INSERT INTO [Permissions] 
			( 
				[PrincipalId], [CorkscrewUri], 
					[IsRead], [IsContribute], [IsFullControl], 
						[IsChildAccess] 
			) 
			VALUES 
			(
				@PrincipalId, @CorkscrewUri, 
					@Read, @Contribute, @FullControl, 
						0 -- Child access is set by a different sproc	
			)
	END
	ELSE 
		BEGIN 

			UPDATE [Permissions] 
				SET 
					[IsRead] = @Read, 
					[IsContribute] = @Contribute, 
					[IsFullControl] = @FullControl, 
					
					/* Reset child access if we are setting explicit permissions */
					[IsChildAccess] = 
						CASE 
							WHEN ((@Read = 1) OR (@Contribute = 1) OR (@FullControl = 1)) 
								THEN 0
							ELSE ISNULL([IsChildAccess], 0)
						END 
			WHERE (([PrincipalId] = @PrincipalId) AND ([CorkscrewUri] = @CorkscrewUri))

		END

	--- If we have any permissions will all permissions zeroed out, remove the row
	DELETE FROM [Permissions] 
		WHERE (([IsRead] = 0) AND ([IsContribute] = 0) AND ([IsFullControl] = 0) AND ([IsChildAccess] = 0))

END