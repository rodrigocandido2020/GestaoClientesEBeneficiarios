CREATE PROCEDURE FI_SP_ListarBeneficiarios
    @IdCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, CPF, IdCliente
    FROM BENEFICIARIOS
    WHERE IdCliente = @IdCliente
END
