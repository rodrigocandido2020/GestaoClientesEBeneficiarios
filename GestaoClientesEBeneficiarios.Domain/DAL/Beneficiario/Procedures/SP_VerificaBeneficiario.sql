﻿CREATE PROC SP_VerificaBeneficiarios
    @CPF NVARCHAR(14),
    @Id INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, CPF
    FROM BENEFICIARIOS
    WHERE CPF = @CPF
      AND (@Id IS NULL OR Id <> @Id)
END
