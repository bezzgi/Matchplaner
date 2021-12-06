CREATE TABLE Benutzer  (
   id_benutzer  INT NOT NULL IDENTITY (1,1),
   vorname  VARCHAR(40) NOT NULL,
   nachname  VARCHAR(40) NOT NULL,
   benutzername  VARCHAR(20) NOT NULL,
   passwort  VARCHAR(60) NOT NULL,
   PRIMARY KEY ( id_benutzer ));


-- -----------------------------------------------------
-- Table  Matchplaner . Mannschaft 
-- -----------------------------------------------------
CREATE TABLE Mannschaft  (
   id_mannschaft  INT NOT NULL IDENTITY (1,1),
   name  VARCHAR(30) NOT NULL,
   PRIMARY KEY ( id_mannschaft ));


-- -----------------------------------------------------
-- Table  Matchplaner . Qualifikation 
-- -----------------------------------------------------
CREATE TABLE Qualifikation  (
   id_qualifikation  INT NOT NULL IDENTITY (1,1),
   name  VARCHAR(20) NOT NULL,
   PRIMARY KEY ( id_qualifikation ));


-- -----------------------------------------------------
-- Table  Matchplaner . Match 
-- -----------------------------------------------------
CREATE TABLE Match  (
   id_match  INT NOT NULL IDENTITY (1,1),
   hallenname  VARCHAR(40) NOT NULL,
   ort  VARCHAR(30) NOT NULL,
   datum  DATE NOT NULL,
   uhrzeit  DATETIME NOT NULL,
   PRIMARY KEY ( id_match ));


-- -----------------------------------------------------
-- Table  Matchplaner . Match_has_Qualifikation 
-- -----------------------------------------------------
CREATE TABLE Match_has_Qualifikation  (
   match_id_match  INT NOT NULL,
   qualifikation_id_qualifikation  INT NOT NULL,
  PRIMARY KEY ( match_id_match ,  qualifikation_id_qualifikation ),
  INDEX  fk_Match_has_Qualifikation_Qualifikation1_idx  ( qualifikation_id_qualifikation  ASC),
  INDEX  fk_Match_has_Qualifikation_Match_idx  (match_id_match  ASC),
  CONSTRAINT  fk_Match_has_Qualifikation_Match 
    FOREIGN KEY (match_id_match)
    REFERENCES Match  (id_match)
	ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT fk_Match_has_Qualifikation_Qualifikation1 
    FOREIGN KEY (qualifikation_id_qualifikation)
    REFERENCES Qualifikation  (id_qualifikation)
	ON DELETE NO ACTION
    ON UPDATE NO ACTION);


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
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT  fk_Match_has_Mannschaft_Mannschaft1 
    FOREIGN KEY ( mannschaft_id_mannschaft )
    REFERENCES Mannschaft  ( id_mannschaft )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table  Matchplaner . Benutzer_has_Mannschaft 
-- -----------------------------------------------------
CREATE TABLE Benutzer_has_Mannschaft  (
   benutzer_id_benutzer  INT NOT NULL,
   mannschaft_id_mannschaft  INT NOT NULL,
  PRIMARY KEY ( benutzer_id_benutzer ,  mannschaft_id_mannschaft ),
  INDEX  fk_Benutzer_has_Mannschaft_Mannschaft1_idx  ( mannschaft_id_mannschaft  ASC),
  INDEX  fk_Benutzer_has_Mannschaft_Benutzer1_idx  ( benutzer_id_benutzer  ASC),
  CONSTRAINT  fk_Benutzer_has_Mannschaft_Benutzer1 
    FOREIGN KEY ( benutzer_id_benutzer )
    REFERENCES Benutzer  ( id_benutzer )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT  fk_Benutzer_has_Mannschaft_Mannschaft1 
    FOREIGN KEY ( mannschaft_id_mannschaft )
    REFERENCES Mannschaft  ( id_mannschaft )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);


-- -----------------------------------------------------
-- Table  Matchplaner . Benutzer_has_Qualifikation 
-- -----------------------------------------------------
CREATE TABLE Benutzer_has_Qualifikation  (
   benutzer_id_benutzer  INT NOT NULL,
   qualifikation_id_qualifikation  INT NOT NULL,
  PRIMARY KEY ( benutzer_id_benutzer ,  qualifikation_id_qualifikation ),
  INDEX  fk_Benutzer_has_Qualifikation_Qualifikation1_idx  ( qualifikation_id_qualifikation  ASC),
  INDEX  fk_Benutzer_has_Qualifikation_Benutzer1_idx  ( benutzer_id_benutzer  ASC),
  CONSTRAINT  fk_Benutzer_has_Qualifikation_Benutzer1 
    FOREIGN KEY ( benutzer_id_benutzer )
    REFERENCES Benutzer  ( id_benutzer )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT  fk_Benutzer_has_Qualifikation_Qualifikation1 
    FOREIGN KEY ( qualifikation_id_qualifikation )
    REFERENCES Qualifikation  ( id_qualifikation )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION);
