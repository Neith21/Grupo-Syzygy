CREATE DATABASE VetAppDB;
GO
USE VetAppDB;
GO

CREATE TABLE Species (
    SpeciesID INT PRIMARY KEY,
    Name VARCHAR(100)
);

CREATE TABLE Animals (
    AnimalID INT PRIMARY KEY,
    Name VARCHAR(255),
    SpeciesID INT,
    Age INT,
    Gender CHAR(1),
    FOREIGN KEY (SpeciesID) REFERENCES Species(SpeciesID)
);

CREATE TABLE LabTechnicians (
    LabTechnicianID INT PRIMARY KEY,
    Name VARCHAR(255),
    Specialization VARCHAR(255)
);

CREATE TABLE Veterinarians (
    VeterinarianID INT PRIMARY KEY,
    Name VARCHAR(255),
    Specialization VARCHAR(255)
);

CREATE TABLE ClinicalExams (
    ExamID INT PRIMARY KEY,
    AnimalID INT,
    LabTechnicianID INT,
    Date DATE,
    Observations TEXT,
    FOREIGN KEY (AnimalID) REFERENCES Animals(AnimalID),
    FOREIGN KEY (LabTechnicianID) REFERENCES LabTechnicians(LabTechnicianID)
);

CREATE TABLE ReferenceValues (
    ValueID INT PRIMARY KEY,
    ExamType VARCHAR(255),
    SpeciesID INT,
    MinAge INT,
    MaxAge INT,
    Gender CHAR(1),
    NormalValue FLOAT,
    FOREIGN KEY (SpeciesID) REFERENCES Species(SpeciesID)
);

CREATE TABLE ExamAnalysis (
    ExamAnalysisID INT PRIMARY KEY,
    ExamID INT,
    LabTechnicianID INT,
    Result TEXT,
    Date DATE,
    FOREIGN KEY (ExamID) REFERENCES ClinicalExams(ExamID),
    FOREIGN KEY (LabTechnicianID) REFERENCES LabTechnicians(LabTechnicianID)
);

CREATE TABLE Diagnostics (
    DiagnosticID INT PRIMARY KEY,
    VeterinarianID INT,
    Result TEXT,
    Date DATE,
    Observations TEXT,
    FOREIGN KEY (VeterinarianID) REFERENCES Veterinarians(VeterinarianID)
);