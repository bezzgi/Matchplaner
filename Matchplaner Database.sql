-- -----------------------------------------------------
-- Table  Matchplaner . Mannschaft 
-- -----------------------------------------------------
CREATE TABLE Mannschaft  (
   id_mannschaft  INT NOT NULL IDENTITY (1,1),
   name  NVARCHAR(30) NOT NULL,
   PRIMARY KEY ( id_mannschaft ));


-- -----------------------------------------------------
-- Table  Matchplaner . Benutzer 
-- -----------------------------------------------------
CREATE TABLE Benutzer  (
   id_benutzer  INT NOT NULL IDENTITY (1,1),
   vorname  NVARCHAR(40) NOT NULL,
   nachname  NVARCHAR(40) NOT NULL,
   benutzername  NVARCHAR(20) NOT NULL,
   passwort  NVARCHAR(68) NOT NULL,
   fk_mannschaft_id int,
   is_schiedsrichter int,
   is_spieler int,
   is_punkteschreiber int,
   admin int,
   PRIMARY KEY (id_benutzer),
   FOREIGN KEY (fk_mannschaft_id) references Mannschaft(id_mannschaft) on delete cascade);


-- -----------------------------------------------------
-- Table  Matchplaner . Qualifikation 
-- -----------------------------------------------------
CREATE TABLE Qualifikation  (
   id_qualifikation  INT NOT NULL IDENTITY (1,1),
   name  NVARCHAR(20) NOT NULL,
   PRIMARY KEY ( id_qualifikation ));


-- -----------------------------------------------------
-- Table  Matchplaner . Match 
-- -----------------------------------------------------
CREATE TABLE Match  (
   id_match  INT NOT NULL IDENTITY (1,1),
   hallenname  NVARCHAR(40) NOT NULL,
   ort  NVARCHAR(30) NOT NULL,
   datum  DATE NOT NULL,
   uhrzeit  DATETIME NOT NULL,
   PRIMARY KEY ( id_match ));


-- -----------------------------------------------------
-- Table  Matchplaner . Logger
-- -----------------------------------------------------
CREATE TABLE Logger (
   id_log int NOT NULL IDENTITY (1,1),
   fk_benutzer_id INT,
   logging NVARCHAR(40),
   zeit DATETIME2,
   FOREIGN KEY (fk_benutzer_id) REFERENCES Benutzer(id_benutzer));


-- -----------------------------------------------------
-- Table  Matchplaner . Match_has_Mannschaft 
-- -----------------------------------------------------
CREATE TABLE Match_has_Mannschaft  (
   match_id_match INT NOT NULL,
   mannschaft_id_mannschaft  INT NOT NULL,
  PRIMARY KEY ( match_id_match ,  mannschaft_id_mannschaft ),
  INDEX  fk_Match_has_Mannschaft_Mannschaft1_idx  ( mannschaft_id_mannschaft  ASC),
  INDEX  fk_Match_has_Mannschaft_Match1_idx  ( match_id_match  ASC),
  CONSTRAINT  fk_Match_has_Mannschaft_Match1 
    FOREIGN KEY ( match_id_match )
    REFERENCES Match  ( id_match )
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT  fk_Match_has_Mannschaft_Mannschaft1 
    FOREIGN KEY ( mannschaft_id_mannschaft )
    REFERENCES Mannschaft  ( id_mannschaft )
    ON DELETE CASCADE
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table  Matchplaner . Match_has_Benutzer
-- -----------------------------------------------------
CREATE TABLE Match_has_Benutzer (
   match_id_match INT NOT NULL,
   benutzer_id_benutzer  INT NOT NULL,
   benutzer_is_schiedsrichter int NOT NULL,
   benutzer_is_spieler INT NOT NULL,
   benutzer_is_punkteschreiber INT NOT NULL,
  PRIMARY KEY ( match_id_match ,  benutzer_id_benutzer ),
  INDEX  fk_Match_has_Benutzer_Benutzer1_idx  ( benutzer_id_benutzer  ASC),
  INDEX  fk_Match_has_Benutzer_Match1_idx  ( match_id_match  ASC),
  CONSTRAINT  fk_Match_has_Benutzer_Match1 
    FOREIGN KEY ( match_id_match )
    REFERENCES Match  ( id_match )
    ON DELETE CASCADE
    ON UPDATE NO ACTION,
  CONSTRAINT  fk_Match_has_Benutzer_Benutzer1
    FOREIGN KEY ( benutzer_id_benutzer )
    REFERENCES Benutzer  ( id_benutzer )
    ON DELETE CASCADE
    ON UPDATE NO ACTION);

insert into Mannschaft values ('Basketball Club Laufen ')
insert into Mannschaft values ('BC Allschwil')
insert into Mannschaft values ('BC Arlesheim')
insert into Mannschaft values ('BC Birsfelden')
insert into Mannschaft values ('BC Münchenstein')
insert into Mannschaft values ('BC Oberdorf')
insert into Mannschaft values ('BC Pratteln')
insert into Mannschaft values ('CVJM Birsfelden Basketball')
insert into Mannschaft values ('EFES Basket 96')
insert into Mannschaft values ('Liestal Basket 44')
insert into Mannschaft values ('Starwings Basket Regio Basel')
insert into Mannschaft values ('TV Muttenz Basketball')


insert into Qualifikation values ('Schiedsrichter')
insert into Qualifikation values ('Punkteschreiber')
insert into Qualifikation values ('Spieler')

insert into Benutzer values ('-','-','Administrator', 'ABTXF9PkLzipKmGiZYLgUjZ3bd24WRzcN1jI4digYAWXg9YwcJ2vm7fKzzdIrnFzMA==', 1,0,0,0,1)

Daten für die Erstanmeldung
Benutzername: Administrator
Passwort: 1234