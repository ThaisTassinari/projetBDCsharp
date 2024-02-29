CREATE DATABASE College1en;
GO

USE College1en;

CREATE TABLE Programs (ProgId VARCHAR(5) NOT NULL, 
ProgName VARCHAR(50) NOT NULL,
CONSTRAINT PK_Programs PRIMARY KEY (ProgId)
);
GO
CREATE TABLE Courses (CId VARCHAR(7) NOT NULL, 
CName VARCHAR(50) NOT NULL,
ProgId VARCHAR(5) NOT NULL,
CONSTRAINT PK_Courses PRIMARY KEY (CId),
CONSTRAINT FK_Courses_Programs 
FOREIGN KEY(ProgId) REFERENCES Programs (ProgId)
   ON DELETE CASCADE
   ON UPDATE CASCADE,
);
GO
CREATE TABLE Students (StId VARCHAR(10) NOT NULL, 
StName VARCHAR(50) NOT NULL,
ProgId VARCHAR(5) NOT NULL,
CONSTRAINT PK_Students PRIMARY KEY (StId),
CONSTRAINT FK_Students_Programs 
FOREIGN KEY(ProgId) REFERENCES Programs (ProgId)
   ON DELETE NO ACTION
   ON UPDATE CASCADE,
);
GO
CREATE TABLE Enrollments (StId VARCHAR(10) NOT NULL, 
CId VARCHAR(7) NOT NULL,
FinalGrade INT,
CONSTRAINT PK_Enrollments PRIMARY KEY (StId, CId),
CONSTRAINT FK_Enrollments_Students 
FOREIGN KEY(StId) REFERENCES Students (StId)
   ON DELETE CASCADE
   ON UPDATE CASCADE,
CONSTRAINT FK_Enrollments_Courses 
FOREIGN KEY(CId) REFERENCES Courses (CId)
   ON DELETE NO ACTION
   ON UPDATE NO ACTION,
);
GO

INSERT Programs (ProgId,  ProgName) VALUES ('P0100', 'Technology');
INSERT Programs (ProgId,  ProgName) VALUES ('P0200', 'Jeux Video');
INSERT Programs (ProgId,  ProgName) VALUES ('P0300', 'Informatique');
GO
INSERT Courses (CId, CName, ProgId) VALUES ('C000001', 'DBA', 'P0100');
INSERT Courses (CId, CName, ProgId) VALUES ('C000002', 'POO', 'P0300');
INSERT Courses (CId, CName, ProgId) VALUES ('C000003', 'IOS', 'P0200');
INSERT Courses (CId, CName, ProgId) VALUES ('C000004', 'WEB', 'P0200');
GO
INSERT Students (StId, StName, ProgId) VALUES ('S000000001', 'Thais','P0100');
INSERT Students (StId, StName, ProgId) VALUES ('S000000002', 'Juliana', 'P0100');
INSERT Students (StId, StName, ProgId) VALUES ('S000000005', 'Wassin', 'P0200');
INSERT Students (StId, StName, ProgId) VALUES ('S000000003', 'Marcelo', 'P0200');
INSERT Students (StId, StName, ProgId) VALUES ('S000000006', 'Mehdi', 'P0300');
INSERT Students (StId, StName, ProgId) VALUES ('S000000004', 'Monica', 'P0300');
GO