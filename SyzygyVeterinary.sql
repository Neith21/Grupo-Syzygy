CREATE DATABASE SyzygyVeterinaryDB;
GO
USE SyzygyVeterinaryDB;
GO

CREATE TABLE Species (
    SpeciesId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    SpeciesName VARCHAR(100) NOT NULL
);

CREATE TABLE AnimalOwners (
    AnimalOwnerId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    AnimalOwnerName VARCHAR(100),
    AnimalOwnerContactInfo VARCHAR(255)
);

CREATE TABLE Animals (
    AnimalId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    AnimalName VARCHAR(100) NOT NULL,
    AnimalAge INT NOT NULL,
    AnimalGender CHAR(1) NOT NULL,
	AnimalOwnerId INT NOT NULL,
	SpeciesId INT NOT NULL,
    FOREIGN KEY (AnimalOwnerId) REFERENCES AnimalOwners(AnimalOwnerId),
	FOREIGN KEY (SpeciesId) REFERENCES Species(SpeciesId)
);

CREATE TABLE LabTechnicians (
    LabTechnicianId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    LabTechnicianName VARCHAR(100) NOT NULL,
    LabTechnicianSpecialization VARCHAR(100) NOT NULL
);

CREATE TABLE Veterinarians (
    VeterinarianId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    VeterinarianName VARCHAR(100) NOT NULL,
    VeterinarianSpecialization VARCHAR(100) NOT NULL
);

CREATE TABLE ClinicalExams (
    ClinicalExamId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	ClinicalExamDate DATE NOT NULL,
    AnimalId INT NOT NULL,
    LabTechnicianId INT NOT NULL,
    FOREIGN KEY (AnimalId) REFERENCES Animals(AnimalId),
    FOREIGN KEY (LabTechnicianId) REFERENCES LabTechnicians(LabTechnicianId),
);

CREATE TABLE ReferenceValues (
    ReferenceValueId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    AgeRange VARCHAR(50) NOT NULL,
    AnalysisType VARCHAR(255) NOT NULL,
    ReferenceData NVARCHAR(MAX) NOT NULL,
	SpeciesId INT NOT NULL,
    FOREIGN KEY (SpeciesId) REFERENCES Species(SpeciesId)
);

CREATE TABLE ExamAnalyses (
    ExamAnalysisId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    AnalysisType VARCHAR(255) NOT NULL,
    ResultData NVARCHAR(MAX) NOT NULL,
	ClinicalExamId INT NOT NULL,
    FOREIGN KEY (ClinicalExamId) REFERENCES ClinicalExams(ClinicalExamId)
);

CREATE TABLE Diagnostics (
    DiagnosticId INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
    DiagnosticResult NVARCHAR(MAX),
    DiagnosticDate DATE NOT NULL,
    DiagnosticObservations NVARCHAR(MAX) NOT NULL,
	VeterinarianId INT NOT NULL,
	ClinicalExamId INT NOT NULL,
    FOREIGN KEY (VeterinarianId) REFERENCES Veterinarians(VeterinarianId),
	FOREIGN KEY (ClinicalExamId) REFERENCES ClinicalExams(ClinicalExamId)
);

INSERT INTO Species (SpeciesName) VALUES 
('Canine'),
('Feline');

INSERT INTO AnimalOwners (AnimalOwnerName, AnimalOwnerContactInfo) VALUES 
('John Smith', 'john@example.com'),
('Jane Doe', 'jane@example.com');

INSERT INTO Animals (AnimalName, AnimalAge, AnimalGender, AnimalOwnerId, SpeciesId) VALUES 
('Rex', 5, 'M', 1, 1),
('Whiskers', 3, 'F', 2, 2);

INSERT INTO LabTechnicians (LabTechnicianName, LabTechnicianSpecialization) VALUES 
('John Doe', 'Hematology'),
('Jane Smith', 'Biochemistry');

INSERT INTO Veterinarians (VeterinarianName, VeterinarianSpecialization) VALUES 
('Dr. Brown', 'General Practice'),
('Dr. Green', 'Surgery');

INSERT INTO ClinicalExams (ClinicalExamDate, AnimalId, LabTechnicianId) VALUES 
('2024-06-01', 1, 1),
('2024-06-02', 2, 2);

INSERT INTO ReferenceValues (AgeRange, AnalysisType, ReferenceData, SpeciesId) VALUES 
('0-5', 'Glucosa', '{"references":[{"parameter":"Glucosa","minValue":70,"maxValue":110,"unit":"mg/dL"}]}', 1),
('0-5', 'Glucosa', '{"references":[{"parameter":"Glucosa","minValue":60,"maxValue":100,"unit":"mg/dL"}]}', 2);

INSERT INTO ExamAnalyses (AnalysisType, ResultData, ClinicalExamId) VALUES 
('Glucosa', N'{"results":[{"parameter":"Glucosa","value":85,"unit":"mg/dL"}]}', 1),
('Glucosa', N'{"results":[{"parameter":"Glucosa","value":95,"unit":"mg/dL"}]}', 2);

INSERT INTO Diagnostics (DiagnosticResult, DiagnosticDate, DiagnosticObservations, VeterinarianId, ClinicalExamId) VALUES 
('No abnormalities detected.', '2024-06-01', 'Healthy', 1, 1),
('Mild inflammation observed.', '2024-06-02', 'Prescribed antibiotics', 2, 2);

SELECT 
    ea.ExamAnalysisId,
    ce.ClinicalExamId AS ExamId,
    ea.AnalysisType,
    JSON_VALUE(ea.ResultData, '$.results[0].value') AS GlucosaResult,
    JSON_VALUE(rv.ReferenceData, '$.references[0].minValue') AS GlucosaMin,
    JSON_VALUE(rv.ReferenceData, '$.references[0].maxValue') AS GlucosaMax,
    CASE 
        WHEN JSON_VALUE(ea.ResultData, '$.results[0].value') BETWEEN JSON_VALUE(rv.ReferenceData, '$.references[0].minValue') AND JSON_VALUE(rv.ReferenceData, '$.references[0].maxValue')
        THEN 'Normal'
        ELSE 'Abnormal'
    END AS GlucosaStatus
FROM 
    ExamAnalyses ea
JOIN 
    ClinicalExams ce ON ea.ClinicalExamId = ce.ClinicalExamId
JOIN 
    Animals a ON ce.AnimalId = a.AnimalId
JOIN 
    ReferenceValues rv ON ea.AnalysisType = rv.AnalysisType
WHERE 
    ea.AnalysisType = 'Glucosa'
    AND JSON_VALUE(rv.ReferenceData, '$.references[0].parameter') = 'Glucosa'
    AND a.SpeciesId = 1  -- Canine
    AND rv.AgeRange = '0-5';

SELECT 
    rv.ReferenceValueId,
    s.SpeciesName AS Species,
    rv.AgeRange,
    rv.AnalysisType,
    JSON_VALUE(rv.ReferenceData, '$.references[0].parameter') AS Parameter,
    JSON_VALUE(rv.ReferenceData, '$.references[0].minValue') AS MinValue,
    JSON_VALUE(rv.ReferenceData, '$.references[0].maxValue') AS MaxValue,
    JSON_VALUE(rv.ReferenceData, '$.references[0].unit') AS Unit
FROM 
    ReferenceValues rv
JOIN 
    Species s ON rv.SpeciesId = s.SpeciesId;
GO

-- ::[Procedimientos almacenados para la tabla: ExamAnalysis]:: --
CREATE OR ALTER PROCEDURE dbo.spExamAnalyses_GetAll
AS
BEGIN
    SELECT 
        ea.ExamAnalysisId,
        ea.AnalysisType,
        ea.ResultData,
        ea.ClinicalExamId,
		ce.ClinicalExamId,
        ce.ClinicalExamDate,
        ce.AnimalId,
        ce.LabTechnicianId
    FROM 
        ExamAnalyses ea
    JOIN 
        ClinicalExams ce ON ea.ClinicalExamId = ce.ClinicalExamId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spExamAnalyses_Update
    @ExamAnalysisId INT,
    @AnalysisType NVARCHAR(255),
    @ResultData NVARCHAR(MAX),
    @ClinicalExamId INT
AS
BEGIN
    UPDATE ExamAnalyses
    SET 
        AnalysisType = @AnalysisType,
        ResultData = @ResultData,
        ClinicalExamId = @ClinicalExamId
    WHERE 
        ExamAnalysisId = @ExamAnalysisId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spExamAnalyses_Insert
    @AnalysisType NVARCHAR(255),
    @ResultData NVARCHAR(MAX),
    @ClinicalExamId INT
AS
BEGIN
    INSERT INTO ExamAnalyses (AnalysisType, ResultData, ClinicalExamId)
    VALUES (@AnalysisType, @ResultData, @ClinicalExamId);
END;
GO

CREATE OR ALTER PROCEDURE dbo.spExamAnalyses_Delete
    @ExamAnalysisId INT
AS
BEGIN
    DELETE FROM ExamAnalyses
    WHERE ExamAnalysisId = @ExamAnalysisId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spExamAnalyses_GetById
    @ExamAnalysisId INT
AS
BEGIN
    SELECT 
        ea.ExamAnalysisId,
        ea.AnalysisType,
        ea.ResultData,
        ea.ClinicalExamId
    FROM 
        ExamAnalyses ea
	WHERE
		ea.ExamAnalysisId = @ExamAnalysisId
END;
GO

-- ::[Procedimientos almacenados para la tabla: ReferenceValues]:: --
CREATE OR ALTER PROCEDURE dbo.spReferenceValues_GetAll
AS
BEGIN
    SELECT 
    rv.ReferenceValueId,
    rv.AgeRange,
    rv.AnalysisType,
    rv.ReferenceData,
	rv.SpeciesId,
    s.SpeciesId,
    s.SpeciesName
	FROM 
		ReferenceValues rv
	JOIN 
		Species s ON rv.SpeciesId = s.SpeciesId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spReferenceValues_Update
    @ReferenceValueId INT,
    @AgeRange VARCHAR(50),
    @AnalysisType VARCHAR(255),
    @ReferenceData NVARCHAR(MAX),
    @SpeciesId INT
AS
BEGIN
    UPDATE ReferenceValues
    SET 
        AgeRange = @AgeRange,
        AnalysisType = @AnalysisType,
        ReferenceData = @ReferenceData,
        SpeciesId = @SpeciesId
    WHERE 
        ReferenceValueId = @ReferenceValueId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spReferenceValues_Insert
    @AgeRange VARCHAR(50),
    @AnalysisType VARCHAR(255),
    @ReferenceData NVARCHAR(MAX),
    @SpeciesId INT
AS
BEGIN
    INSERT INTO ReferenceValues (AgeRange, AnalysisType, ReferenceData, SpeciesId)
    VALUES (@AgeRange, @AnalysisType, @ReferenceData, @SpeciesId);
END;
GO

CREATE OR ALTER PROCEDURE dbo.spReferenceValues_Delete
    @ReferenceValueId INT
AS
BEGIN
    DELETE FROM ReferenceValues
    WHERE ReferenceValueId = @ReferenceValueId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spReferenceValues_GetById
    @ReferenceValueId INT
AS
BEGIN
    SELECT 
		rv.ReferenceValueId,
		rv.AgeRange,
		rv.AnalysisType,
		rv.ReferenceData,
		rv.SpeciesId
	FROM 
		ReferenceValues rv
    WHERE 
        rv.ReferenceValueId = @ReferenceValueId;
END;
GO

---Proceso almacenados para tabla diagnostics:
CREATE OR ALTER PROCEDURE dbo.spDiagnostics_GetAll
AS
BEGIN
    SELECT 
        DiagnosticId,
        DiagnosticResult,
        DiagnosticDate,
        DiagnosticObservations,
        V.VeterinarianId,
		V.VeterinarianName,
        C.ClinicalExamId
    FROM 
        Diagnostics D
	INNER JOIN Veterinarians V ON v.VeterinarianId = D.VeterinarianId
	INNER JOIN ClinicalExams C ON D.ClinicalExamId = C.ClinicalExamId
	
END;
GO

EXEC dbo.spDiagnostics_GetAll;

CREATE OR ALTER PROCEDURE dbo.spDiagnostics_GetById
    @DiagnosticId INT
AS
BEGIN
    SELECT 
        DiagnosticId,
        DiagnosticResult,
        DiagnosticDate,
        DiagnosticObservations,
        V.VeterinarianId,
		V.VeterinarianName,
        C.ClinicalExamId
    FROM 
        Diagnostics D
		INNER JOIN Veterinarians V ON v.VeterinarianId = D.VeterinarianId
		INNER JOIN ClinicalExams C ON D.ClinicalExamId = C.ClinicalExamId
    WHERE 
        DiagnosticId = @DiagnosticId;
END;
GO

EXEC spDiagnostics_GetById 1


CREATE OR ALTER PROCEDURE dbo.spDiagnostics_Update
(@DiagnosticId INT, @DiagnosticResult NVARCHAR(MAX), @DiagnosticDate DATE, @DiagnosticObservations NVARCHAR(MAX), @VeterinarianId INT, @ClinicalExamId INT)
AS
BEGIN
    UPDATE Diagnostics
    SET 
        DiagnosticResult = @DiagnosticResult,
        DiagnosticDate = @DiagnosticDate,
        DiagnosticObservations = @DiagnosticObservations,
        VeterinarianId = @VeterinarianId,
        ClinicalExamId = @ClinicalExamId
    WHERE 
        DiagnosticId = @DiagnosticId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spDiagnostics_Insert
    @DiagnosticResult NVARCHAR(MAX),
    @DiagnosticDate DATE,
    @DiagnosticObservations NVARCHAR(MAX),
    @VeterinarianId INT,
    @ClinicalExamId INT
AS
BEGIN
    INSERT INTO Diagnostics (DiagnosticResult, DiagnosticDate, DiagnosticObservations, VeterinarianId, ClinicalExamId)
    VALUES (@DiagnosticResult, @DiagnosticDate, @DiagnosticObservations, @VeterinarianId, @ClinicalExamId);
END;
GO


CREATE OR ALTER PROCEDURE dbo.spDiagnostics_Delete
    @DiagnosticId INT
AS
BEGIN
    DELETE FROM Diagnostics
    WHERE DiagnosticId = @DiagnosticId;
END;
GO


--Proceso almacenados para tabla veterianarians
CREATE OR ALTER PROCEDURE dbo.spVeterinarians_GetAll
AS
BEGIN
    SELECT 
        VeterinarianId,
        VeterinarianName,
        VeterinarianSpecialization
    FROM 
        Veterinarians;
END;
GO

EXEC dbo.spVeterinarians_GetAll
go

CREATE OR ALTER PROCEDURE dbo.spVeterinarians_GetById
    @VeterinarianId INT
AS
BEGIN
    SELECT 
        VeterinarianId,
        VeterinarianName,
        VeterinarianSpecialization
    FROM 
        Veterinarians
    WHERE 
        VeterinarianId = @VeterinarianId;
END;
GO

exec dbo.spVeterinarians_GetById 1


CREATE OR ALTER PROCEDURE dbo.spVeterinarians_Insert
    @VeterinarianName VARCHAR(100),
    @VeterinarianSpecialization VARCHAR(100)
AS
BEGIN
    INSERT INTO Veterinarians (VeterinarianName, VeterinarianSpecialization)
    VALUES (@VeterinarianName, @VeterinarianSpecialization);
END;
GO


CREATE OR ALTER PROCEDURE dbo.spVeterinarians_Update
    @VeterinarianId INT,
    @VeterinarianName VARCHAR(100),
    @VeterinarianSpecialization VARCHAR(100)
AS
BEGIN
    UPDATE Veterinarians
    SET 
        VeterinarianName = @VeterinarianName,
        VeterinarianSpecialization = @VeterinarianSpecialization
    WHERE 
        VeterinarianId = @VeterinarianId;
END;
GO

CREATE OR ALTER PROCEDURE dbo.spVeterinarians_Delete
    @VeterinarianId INT
AS
BEGIN
    DELETE FROM Veterinarians
    WHERE VeterinarianId = @VeterinarianId;
END;
GO

---------------------------------------- SP_AnimalOwners------------------------------------------

-- Procedimiento almacenado para insertar un nuevo propietario de animal
CREATE PROCEDURE sp_AnimalOwner_Insert
    @AnimalOwnerName VARCHAR(100),
    @AnimalOwnerContactInfo VARCHAR(255)
AS
BEGIN
    INSERT INTO AnimalOwners (AnimalOwnerName, AnimalOwnerContactInfo)
    VALUES (@AnimalOwnerName, @AnimalOwnerContactInfo);
END;
GO

-- Procedimiento almacenado para actualizar un propietario de animal
CREATE PROCEDURE sp_AnimalOwner_Update
    @AnimalOwnerId INT,
    @AnimalOwnerName VARCHAR(100),
    @AnimalOwnerContactInfo VARCHAR(255)
AS
BEGIN
    UPDATE AnimalOwners
    SET AnimalOwnerName = @AnimalOwnerName,
        AnimalOwnerContactInfo = @AnimalOwnerContactInfo
    WHERE AnimalOwnerId = @AnimalOwnerId;
END;
GO

-- Procedimiento almacenado para eliminar un propietario de animal
CREATE PROCEDURE sp_AnimalOwner_Delete
    @AnimalOwnerId INT
AS
BEGIN
    DELETE FROM AnimalOwners
    WHERE AnimalOwnerId = @AnimalOwnerId;
END;
GO

-- Procedimiento almacenado para obtener información de un propietario de animal por ID
CREATE PROCEDURE sp_AnimalOwner_GetById
    @AnimalOwnerId INT
AS
BEGIN
    SELECT AnimalOwnerId, AnimalOwnerName, AnimalOwnerContactInfo
    FROM AnimalOwners
    WHERE AnimalOwnerId = @AnimalOwnerId;
END;
GO

-- Procedimiento almacenado para obtener la lista de todos los propietarios de animales
CREATE PROCEDURE sp_AnimalOwner_GetAll
AS
BEGIN
    SELECT AnimalOwnerId, AnimalOwnerName, AnimalOwnerContactInfo
    FROM AnimalOwners;
END;
GO

----
INSERT INTO AnimalOwners (AnimalOwnerName, AnimalOwnerContactInfo)
VALUES 
('John Doe', '5555-1234'),
('Jane Smith', '5555-5678');
----
GO
------------------------------ sp_Species------------------------------------------------

-- Procedimiento almacenado para insertar una nueva especie
CREATE PROCEDURE sp_Species_Insert
    @SpeciesName VARCHAR(100)
AS
BEGIN
    INSERT INTO Species (SpeciesName)
    VALUES (@SpeciesName);
END;
GO

-- Procedimiento almacenado para actualizar una especie
CREATE PROCEDURE sp_Species_Update
    @SpeciesId INT,
    @SpeciesName VARCHAR(100)
AS
BEGIN
    UPDATE Species
    SET SpeciesName = @SpeciesName
    WHERE SpeciesId = @SpeciesId;
END;
GO

-- Procedimiento almacenado para eliminar una especie
CREATE PROCEDURE sp_Species_Delete
    @SpeciesId INT
AS
BEGIN
    DELETE FROM Species
    WHERE SpeciesId = @SpeciesId;
END;
GO

-- Procedimiento almacenado para obtener información de una especie por ID
CREATE PROCEDURE sp_Species_GetById
    @SpeciesId INT
AS
BEGIN
    SELECT SpeciesId, SpeciesName
    FROM Species
    WHERE SpeciesId = @SpeciesId;
END;
GO

-- Procedimiento almacenado para obtener la lista de todas las especies
CREATE PROCEDURE sp_Species_GetAll
AS
BEGIN
    SELECT SpeciesId, SpeciesName
    FROM Species;
END;
GO

-----
INSERT INTO Species (SpeciesName)
VALUES 
('Canino'),
('Felino'),
('Conejo'),
('Pájaro');
-----
GO

------------------------spLabTechnicians----------------------------------------------

-- Procedimiento almacenado para insertar un nuevo técnico de laboratorio
CREATE PROCEDURE sp_LabTechnicians_Insert
    @LabTechnicianName VARCHAR(100),
    @LabTechnicianSpecialization VARCHAR(100)
AS
BEGIN
    INSERT INTO LabTechnicians (LabTechnicianName, LabTechnicianSpecialization)
    VALUES (@LabTechnicianName, @LabTechnicianSpecialization);
    
END;
GO

-- Procedimiento almacenado para actualizar un técnico de laboratorio
CREATE PROCEDURE sp_LabTechnicians_Update
    @LabTechnicianId INT,
    @LabTechnicianName VARCHAR(100),
    @LabTechnicianSpecialization VARCHAR(100)
AS
BEGIN
    UPDATE LabTechnicians
    SET LabTechnicianName = @LabTechnicianName,
        LabTechnicianSpecialization = @LabTechnicianSpecialization
    WHERE LabTechnicianId = @LabTechnicianId;
END;
GO

-- Procedimiento almacenado para eliminar un técnico de laboratorio
CREATE PROCEDURE sp_LabTechnicians_Delete
    @LabTechnicianId INT
AS
BEGIN
    DELETE FROM LabTechnicians
    WHERE LabTechnicianId = @LabTechnicianId;
END;
GO

-- Procedimiento almacenado para obtener información de un técnico de laboratorio por ID
CREATE PROCEDURE sp_LabTechnicians_GetById
    @LabTechnicianId INT
AS
BEGIN
    SELECT LabTechnicianId, LabTechnicianName, LabTechnicianSpecialization
    FROM LabTechnicians
    WHERE LabTechnicianId = @LabTechnicianId;
END;
GO

-- Procedimiento almacenado para obtener la lista de todos los técnicos de laboratorio
CREATE PROCEDURE sp_LabTechnicians_GetAll
AS
BEGIN
    SELECT LabTechnicianId, LabTechnicianName, LabTechnicianSpecialization
    FROM LabTechnicians;
END;
GO

-----
INSERT INTO LabTechnicians (LabTechnicianName, LabTechnicianSpecialization)
VALUES 
('Juan Pérez', 'Bioquímica'),
('María López', 'Microbiología'),
('Pedro González', 'Genética');
----