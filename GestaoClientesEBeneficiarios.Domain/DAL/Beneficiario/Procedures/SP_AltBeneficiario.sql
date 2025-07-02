CREATE PROC SP_AltBeneficiario
    @Id BIGINT,
    @Nome VARCHAR(100),
    @CPF VARCHAR(14),
    @IdCliente BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE BENEFICIARIOS
    SET Nome = @Nome,
        CPF = @CPF,
        IdCliente = @IdCliente
    WHERE Id = @Id
END
